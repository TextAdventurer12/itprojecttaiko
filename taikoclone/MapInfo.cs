using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace taikoclone
{
    public class MapInfo
    {
        public const string MapFolder = "../../Maps/";
        public string Name;
        public string DifficultyName;
        private string Directory;
        public string Filepath => $"{MapFolder}{Name}/{Name} [{DifficultyName}].osu";
        public string AudioFile => $"{MapFolder}{Name}/audio.mp3";
        private double? difficulty;
        public double Difficulty
            => difficulty is null ? (difficulty = map.Difficulty()).Value : difficulty.Value;
        private Map mapData;
        private Image background;
        public Map map
        {
            get => mapData ?? InitialiseMapData();
        }
        private Map InitialiseMapData()
        {
            mapData = MapParser.FromFile(Filepath);
            return mapData;
        }
        public MapInfo(string name, string difficultyName, string directory)
        {
            Name = name;
            DifficultyName = difficultyName;
            Directory = directory;
        }
    }
}
