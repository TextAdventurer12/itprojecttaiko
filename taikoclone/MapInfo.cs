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
        public string AudioFile => $"{MapFolder}{Name}/audio.mp3";
        private double? difficulty;
        public double Difficulty
            => difficulty is null ? (difficulty = map.Difficulty()).Value : difficulty.Value;
        private Map mapData;
        private Image background;
        public Image Background
            => background?? InitialiseBackgruond();
        public Map map
        {
            get => mapData?? InitialiseMapData();
        }
        private Map InitialiseMapData()
        {
            mapData = MapParser.FromFile(Filepath);
            return mapData;
        }
        private Image InitialiseBackgruond()
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