using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using taikoclone.Utils;

namespace taikoclone
{
    public partial class Menu : Form
    {
        public List<MapInfo> library;
        private List<MapFrame> frames = new List<MapFrame>();
        private int selectedIndex = 0;
        private SolidBrush backgroundTint;
        private SolidBrush buttonColour;
        private SolidBrush textColour;
        private Rectangle goButton;
        private ulong tickIndex = 0;
        public Menu()
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            InitializeComponent();
            library = loadLibrary().OrderBy(x => x.Difficulty).ToList();
            foreach (var map in library)
                frames.Add(new MapFrame(map));
            backgroundTint = new SolidBrush(Color.FromArgb(64, 0, 0, 0));
            buttonColour = new SolidBrush(Color.FromArgb(200, 0, 0, 32));
            textColour = new SolidBrush(Color.White);
            goButton = new Rectangle(25, canvas.Height - 60, 150, 50);
            canvas.Dock = DockStyle.Fill;
        }
        private IEnumerable<MapInfo> loadLibrary()
        {
            foreach (string set in Directory.GetDirectories(MapInfo.MapFolder))
            {
                foreach (string mapPath in Directory.GetFiles(set))
                {
                    string map = Path.GetFileName(mapPath);
                    if (!map.Contains(".osu"))
                        continue;
                    string[] halves = map.Split('[');
                    string name = halves[0].RemoveEndingWhitespace();
                    string difficultyName = halves[1].Split('.')[0].Replace("]", "").RemoveEndingWhitespace();
                    yield return new MapInfo(name, difficultyName);
                }
            }
        }

        private void tick_Tick(object sender, EventArgs e)
        {
            tickIndex++;
            if (tickIndex == 100)
            {
                GC.Collect();
                tickIndex = 0;
            }
            canvas.Invalidate();
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            Font drawFont = new Font("Arial", 31);
            e.Graphics.Clear(Color.White);
            Image bgImage = library[selectedIndex].Background;
            if (!(bgImage is null))
                e.Graphics.DrawImage(library[selectedIndex].Background, new Rectangle(0, 0, canvas.Width, canvas.Height));
            e.Graphics.FillRectangle(backgroundTint, new Rectangle(0, 0, canvas.Width, canvas.Height));
            for (int i = 0; i < library.Count; i++)
            {
                double x = (i == selectedIndex) ? canvas.Width - 450 : canvas.Width - 425;
                double absoluteY = i * 150 + 50;
                double relativeY = absoluteY - (selectedIndex - 1) * 150;
                Point location = new Point((int)x, (int)relativeY);
                if (relativeY < 0 || relativeY > canvas.Height)
                    library[i].CleanBackground();
                else
                    frames[i].Draw(e.Graphics, location);
            }
            goButton = new Rectangle(25, canvas.Height - 60, 150, 50);
            e.Graphics.FillRectangle(buttonColour, goButton);
            e.Graphics.DrawString("Go!", drawFont, textColour, goButton.Location);
            Rectangle upThumbnail = new Rectangle(canvas.Width - 20, canvas.Height - 45, 20, 20);
            Rectangle downThumbnail = new Rectangle(canvas.Width - 20, canvas.Height - 20, 20, 20);
            e.Graphics.DrawImage(Gameplay.keyThumbnails[Keys.W], upThumbnail);
            e.Graphics.DrawImage(Gameplay.keyThumbnails[Keys.S], downThumbnail);
        }

        private void Menu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                tick.Enabled = false;
                this.Close();
            }
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
                selectedIndex++;
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
                selectedIndex--;
            if (selectedIndex < 0)
                selectedIndex = library.Count - 1;
            selectedIndex %= library.Count;
            if (e.KeyCode != Keys.Enter)
                return;
            Gameplay gameplay = new Gameplay(library[selectedIndex]);
            this.Hide();
            gameplay.ShowDialog();
            this.Show();
        }

        private void Menu_MouseClick(object sender, MouseEventArgs e)
        {
            if (goButton.Contains(e.Location))
            {
                Gameplay gameplay = new Gameplay(library[selectedIndex]);
                this.Hide();
                gameplay.ShowDialog();
                this.Show();
            }
        }
    }
}
