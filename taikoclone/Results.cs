using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.SoundFont;

namespace taikoclone
{
    public partial class Results : Form
    {
        MapInfo map;
        public ScoreInfo score;
        static SolidBrush backgroundTint = new SolidBrush(Color.FromArgb(200, 0, 0, 32));
        static SolidBrush textColor = new SolidBrush(textColor.White);
        static Font bigFont = new Font("Arial", 64);
        static Font medFont = new Font("Arial", 32);
        static Font smallFont = new Font("Arial", 24);
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
            e.Graphics.Clear(Color.White);
            Image bgImage = map.Background;
            if (!(bgImage is null))
                e.Graphics.DrawImage(map.Background, new Rectangle(0, 0, canvas.Width, canvas.Height));
            Rectangle mapTitle = new Rectangle(16, 16, 512, 64);
            Rectangle diffTitle = new Rectangle(16, 96, 256, 32);
            e.Graphics.FillRectangle(mapTitle, backgroundTint);
            e.Graphics.DrawString(map.Name, bigFont, textColor, mapTitle);
            e.Graphics.FillRectangle(diffTitle, backgroundTint);
            e.Graphics.DrawString(map.Difficulty, medFont, textColor, diffTitle);

            Rectangle judgementBox = new Rectangle(32, 144, 780, canvas.Height - 144 - 32);
            e.Graphics.FillRectangle(judgementBox, backgroundTint);

            Rectangle greatBox = new Rectangle(32, 200, 256, 52);
            e.Graphics.FillRectangle(greatBox, backgroundTint);
            e.Graphics.DrawString($"Great: {score.CountGreat}", smallFont, textColor);
            Rectangle okBox = new Rectangle(320, 200, 256, 52);
            e.Graphics.FillRectangle(greatBox, backgroundTint);
            e.Graphics.DrawString($"OK: {score.CountOk}", smallFont, textColor);
            Rectangle missBox = new Rectangle(32, 264, 256, 52);
            e.Graphics.FillRectangle(greatBox, backgroundTint);
            e.Graphics.DrawString($"Miss: {score.CountMiss}", smallFont, textColor);
            Rectangle comboBox = new Rectangle(32, 328, 256, 64);
            e.Graphics.FillRectangle(greatBox, backgroundTint);
            e.Graphics.DrawString($"{score.MaxCombo}x", medFont, textColor);

            Rectangle scoreBox = new Rectangle(32, 392, 256, 128);
            e.Graphics.FillRectangle(greatBox, backgroundTint);
            e.Graphics.DrawString($"{score.Score:F0}x", bigFont, textColor);
            
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
