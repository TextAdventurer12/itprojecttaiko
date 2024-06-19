using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taikoclone
{
    public partial class Results : Form
    {
        MapInfo map;
        Dictionary<Judgement, int> judgementCounts;
        static Font font = new Font("Arial", 30);
        static SolidBrush brush = new SolidBrush(Color.Black);
        public Results(List<Judgement> judgements, MapInfo map)
        {
            InitializeComponent();
            canvas.Invalidate();
            judgementCounts = new Dictionary<Judgement, int>()
            {
                { Judgement.Great, 0},
                { Judgement.Ok, 0},
                { Judgement.Miss, 0},
            };
            foreach (Judgement judgement in judgements)
                judgementCounts[judgement]++;
            this.map = map;
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawString($"{map.Name} [{map.DifficultyName}]", font, brush, new Point(25, 25));
            e.Graphics.DrawString($"{Accuracy * 100:F2}%", font, brush, new Point(25, 225));
            e.Graphics.DrawString($"Great: {judgementCounts[Judgement.Great]}", font, brush, new Point(25, 75));
            e.Graphics.DrawString($"Ok: {judgementCounts[Judgement.Ok]}", font, brush, new Point(25, 125));
            e.Graphics.DrawString($"Miss: {judgementCounts[Judgement.Miss]}", font, brush, new Point(25, 175));
        }
        private double Accuracy
            => (double)(judgementCounts[Judgement.Great] * 300 + judgementCounts[Judgement.Ok] * 100) / (judgementCounts.Values.Sum() * 300);

        private void Results_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }
    }
}
