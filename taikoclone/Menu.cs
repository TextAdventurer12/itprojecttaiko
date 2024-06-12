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

namespace taikoclone
{
    public partial class Menu : Form
    {
        private List<MapInfo> library;
        public Menu()
        {
            InitializeComponent();
            library = loadLibrary().ToList();
        }
        private IEnumerable<MapInfo> loadLibrary()
        {
            foreach (string set in Directory.GetDirectories(MapInfo.MapFolder))
            {
                foreach (string mapPath in Directory.GetFiles(set))
                {
                    string map = Path.GetFileName(mapPath);
                    Console.WriteLine(map);
                    if (!map.Contains(".osu"))
                        continue;
                    string[] halves = map.Split('[');
                    string name = halves[0];
                    string difficultyName = halves[1].Split('.')[0].Replace("]", "");
                    yield return new MapInfo(name, difficultyName);
                }
            }
        }

        private void Menu_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void tick_Tick(object sender, EventArgs e)
        {
            canvas.Invalidate();
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {

            Pen pen = new Pen(new SolidBrush(Color.Black));
            Font drawFont = new Font("Arial", 16);
            e.Graphics.Clear(Color.White);
            Console.WriteLine(library.Count);
            for (int i = 0; i < library.Count; i++)
                e.Graphics.DrawString($"{library[i].Name} [{library[i].DifficultyName}]", drawFont, new SolidBrush(Color.Black), new Point(75, 50 + i * 20));
        }
    }
}
