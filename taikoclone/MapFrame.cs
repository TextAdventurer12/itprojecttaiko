using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace taikoclone
{
    internal class MapFrame
    {
        public MapInfo map;
        public Map mapData => map.map;
        private static Size size = new Size(400, 100);
        private static Brush background = new SolidBrush(Color.FromArgb(128, 0, 0, 0));
        private static Brush fontColor = new SolidBrush(Color.White);
        private static Font headerFont = new Font("Arial", 15);
        private static Font difficultyFont = new Font("Arial", 10);
        public void Draw(Graphics target, Point location)
        {
            Rectangle box = new Rectangle(location, size);
            target.FillRectangle(background, box);
            target.DrawString(map.Name, headerFont, fontColor, new Point(location.X + 5, location.Y + 5));
            target.DrawString(map.DifficultyName, difficultyFont, fontColor, new Point(location.X + 5, location.Y + 30));
            target.DrawString($"{map.Difficulty:F2} stars", difficultyFont, fontColor, new Point(location.X + 5, location.Y + 50));
            Rectangle starRating = new Rectangle(new Point(location.X + 5, location.Y + 75),
                new Size((int)((map.Difficulty / 10) * 200), 20));
            target.FillRectangle(fontColor, starRating);
        }
        public MapFrame(MapInfo map)
        {
            this.map = map;
        }
    }
}
