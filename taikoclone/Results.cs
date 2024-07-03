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
        public ScoreInfo score;
        static Font font = new Font("Arial", 30);
        static SolidBrush brush = new SolidBrush(Color.Black);
        public Results(List<Judgement> judgements, List<int> comboes, MapInfo map, Leaderboard targetBoard)
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            InitializeComponent();
            canvas.Invalidate();
            this.score = new ScoreInfo(judgements, comboes, map);
            this.map = map;
            Popup nameGetter = new Popup();
            nameGetter.ShowDialog();
            string name = nameGetter.field;
            targetBoard.scores.Add(new LeaderboardScore(this.score, name));
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawString($"{map.Name} [{map.DifficultyName}]", font, brush, new Point(25, 25));
            e.Graphics.DrawString($"Great: {score.CountGreat}", font, brush, new Point(25, 75));
            e.Graphics.DrawString($"Ok: {score.CountOk}", font, brush, new Point(25, 125));
            e.Graphics.DrawString($"Miss: {score.CountMiss}", font, brush, new Point(25, 175));
            e.Graphics.DrawString($"{score.Accuracy * 100:F2}%", font, brush, new Point(25, 225));
            e.Graphics.DrawString($"Max Combo: {score.MaxCombo}", font, brush, new Point(25, 275));
            e.Graphics.DrawString($"Score: {score.Score:F0}", font, brush, new Point(25, 325));
        }
        private void Results_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }
    }
    public class ScoreInfo
    {
        Dictionary<Judgement, int> judgementCounts;
        List<int> comboes;
        private int? maxCombo = null;
        private int InitialiseCombo()
            => (maxCombo = comboes.Max()).Value;
        public int MaxCombo
            => maxCombo?? InitialiseCombo();
        public int CountGreat => judgementCounts[Judgement.Great];
        public int CountOk => judgementCounts[Judgement.Ok];
        public int CountMiss => judgementCounts[Judgement.Miss];
        public double Accuracy
            => judgementCounts.Sum(judgement => judgement.Value * (int)judgement.Key) / (judgementCounts.Sum(judgement => judgement.Value * (int)Judgement.Great) * (int)Judgement.Great);
        public MapInfo map;
        public ScoreInfo(List<Judgement> judgements, List<int> comboes, MapInfo map)
        {
            this.comboes = new List<int>(comboes);
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
        public ScoreInfo(string Csv)
        {
            string[] entries = Csv.Split(',');
            if (entries.Count() == 0)
                throw new ArgumentNullException("Found no entries for csv");
            score = double.Parse(entries[0]);
            maxCombo = int.Parse(entries[1]);
            this.judgementCounts = new Dictionary<Judgement, int>()
            {
                {Judgement.Great, int.Parse(entries[2]) },
                {Judgement.Ok, int.Parse(entries[3])   },
                { Judgement.Miss, int.Parse(entries[4]) }
            };
        }
        private double? score = null;
        public double Score
        {
            get => score?? CalculateScore();
            set => score = value;
        }
        private double CalculateScore()
        {
            if (map is null)
                throw new ArgumentNullException("Attempted to calculate score on null map");
            score = 1000000 * Accuracy * (comboes.Sum(c => Math.Pow(c, 2)) / Math.Pow(map.map.MaxCombo, 2));
            return score.Value;
        }
        public string ToCsv()
            => $"{Score}, {MaxCombo}, {CountGreat}, {CountOk}, {CountMiss}";
    }
}
