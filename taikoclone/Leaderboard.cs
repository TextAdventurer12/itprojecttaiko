using System;
using System.Collections.Generic;
using System.IO;
using taikoclone.Utils;

namespace taikoclone
{
    public class Leaderboard
    {
        public List<LeaderboardScore> scores;
        public void SaveToFile(StreamWriter target)
        {
            foreach (LeaderboardScore score in scores)
                target.WriteLine(score.ToCsv());
        }
        public Leaderboard(StreamReader source)
        {
            this.scores = new List<LeaderboardScore>();
            IEnumerable<string> csv = IOUtils.dumpFile(source);
            foreach (string line in csv)
                scores.Add(new LeaderboardScore(line));
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
            string[] entries = csv.Split(",");
            if (entries.Count() == 0)
                throw new ArgumentNullException("Found no entries for csv");
            this.Name = entries[0];
            string scoreData = "";
            for (int i = 1; i < entries.Count(); i++)
                scoreData += entries[i];
            this.Score = new ScoreInfo(scoreData);
        }
        public string ToCsv()
            => $"{Name}, {Score.ToCsv()}";
    }
}