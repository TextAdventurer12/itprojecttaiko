using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace taikoclone
{
    public class MapInfo
    {
        public const string MapFolder = "../../Maps/";
        public string Name;
        public string DifficultyName;
        public string Filepath => $"{FileLocation}{Name} [{DifficultyName}].osu";
        private string FileLocation => $"{MapFolder}{Name}/";
        private string audioFile;
        public string AudioFile => audioFile ?? InitialiseAudio();
        private double? difficulty;
        public double Difficulty
            => difficulty is null ? (difficulty = map.Difficulty()).Value : difficulty.Value;
        private Map mapData;
        private Image background;
        public Image Background
            => background?? InitialiseBackground();
        public Map map
            => mapData?? InitialiseMapData();
        private Map InitialiseMapData()
        {
            mapData = MapParser.FromFile(Filepath);
            return mapData;
        }
        private string InitialiseAudio()
        {
            foreach (string filePath in Directory.GetFiles($"{MapFolder}{Name}"))
            {
                string file = Path.GetFileName(filePath);
                if (file.Contains(".mp3"))
                {
                    return filePath;
                }
            }
            return null;
        }
        private Image InitialiseBackground()
        {
            foreach (string filePath in Directory.GetFiles($"{MapFolder}{Name}"))
            {
                string file = Path.GetFileName(filePath);
                if (file.Contains(".jpg"))
                {
                    background = Image.FromFile($"{FileLocation}{file}");
                    return background;
                }
            }
            return null;
        }
        public void CleanBackground()
        {
            background = null;
        }
        public MapInfo(string name, string difficultyName)
        {
            Name = name;
            DifficultyName = difficultyName;
        }
    }
}