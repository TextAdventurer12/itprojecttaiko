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
        private int selectedIndex = 0;
        private SolidBrush brush;
        public Menu()
        {
            InitializeComponent();
            library = loadLibrary().ToList();
            brush = new SolidBrush(Color.Black);
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
            canvas.Invalidate();
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            Font drawFont = new Font("Arial", 16);
            e.Graphics.Clear(Color.White);
            for (int i = 0; i < library.Count; i++)
            {
                if (i == selectedIndex)
                    brush.Color = Color.Blue;
                else
                    brush.Color = Color.Black;
                e.Graphics.DrawString($"{library[i].Name} [{library[i].DifficultyName}] - {library[i].Difficulty:F1}*", drawFont, brush, new Point(25, 50 + i * 20));
            }
        }

        private void Menu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
                selectedIndex++;
            if (e.KeyCode == Keys.Up)
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
    }
}
