using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taikoclone
{
    internal class MapInfo
    {
        public const string MapFolder = "../../Maps/";
        public string Name;
        public string DifficultyName;
        public string Filepath => $"{MapFolder}{Name}/{Name} [{DifficultyName}].osu";
        private Map mapData;
        public Map map
        {
            get => mapData ?? InitialiseMapData();
        }
        private Map InitialiseMapData()
        {
            mapData = MapParser.FromFile(Filepath);
            return mapData;
        }
        public MapInfo(string name, string difficultyName)
        {
            Name = name;
            DifficultyName = difficultyName;
        }
    }
}
