using System;
using System.Collections.Generic;
using System.IO;
using taikoclone.Utils;
using System.Windows.Forms;
using System.Drawing;

namespace taikoclone
{
    public class Leaderboard
    {
        public string Header;
        public List<LeaderboardScore> scores;
        private static Font HeaderFont = new Font("Arial", 32);
        private static Font UIFont = new Font("Arial", 24);
        private static SolidBrush fontBrush = new SolidBrush(Color.White);
        private static SolidBrush backgroundBrush = new SolidBrush(Color.FromArgb(200, 0, 0, 32));
        public void SaveToFile(StreamWriter target)
        {
            target.WriteLine(Header);
            foreach (LeaderboardScore score in scores)
                target.WriteLine($"{score.ToCsv()}");
        }
        public Leaderboard(StreamReader source)
        {
            Header = source.ReadLine();
            this.scores = new List<LeaderboardScore>();
            IEnumerable<string> csv = IOUtils.dumpFile(source);
            foreach (string line in csv)
                scores.Add(new LeaderboardScore(line));
        }
        public Leaderboard(string Header)
        {
            this.Header = Header;
            this.scores = new List<LeaderboardScore>();
        }
        public void Draw(Graphics target, PictureBox canvas)
        {
            Rectangle header = new Rectangle(16, 16, 700, 128);
            target.FillRectangle(backgroundBrush, header);
            target.DrawString(Header, HeaderFont, fontBrush, header);
            for (int i = 0; i < scores.Count; i++)
            {
                Rectangle scoreBox = new Rectangle(16, 32 + 128 + i * 64, 700, 64);
                if (scoreBox.Bottom > canvas.Height - 128)
                    break;
                target.FillRectangle(backgroundBrush, scoreBox);
                target.DrawString($"{scores[i].Name}: {scores[i].Score}", UIFont, fontBrush, scoreBox);
            }
        }
    }
    public class LeaderboardScore
    {
        public ScoreInfo Score;
        public string Name;
        public LeaderboardScore(ScoreInfo Score, string Name)
        {
            this.Score = Score;
            this.Name = Name;
        }
        public LeaderboardScore(string csv)
        {
            string[] entries = csv.Split(',');
            if (entries.Length == 0)
                throw new ArgumentNullException("Found no entries for csv");
            this.Name = entries[0];
            string scoreData = "";
            for (int i = 1; i < entries.Length; i++)
                scoreData += entries[i];
            this.Score = new ScoreInfo(scoreData);
        }
        public string ToCsv()
            => $"{Name}, {Score.ToCsv()}";
    }
}