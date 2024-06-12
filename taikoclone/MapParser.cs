using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace taikoclone
{
    internal class MapParser
    {
        public static Map FromFile(string filepath)
        {
            StreamReader stream = new StreamReader(filepath);
            Dictionary<string, List<string>> sections = ReadFile(stream);
            return new Map(Gameplay.preempt, Gameplay.hitWindow, Gameplay.hitWindow + Gameplay.hitWindowMiss, ParsePackages(sections).ToList());
        }
        private static IEnumerable<HitObject> ParsePackages(Dictionary<string, List<string>> sections)
        {
            if (!sections.Keys.Contains("HitObjects"))
                throw new ArgumentException("Could not find hit objects section");
            List<string> objectData = sections["HitObjects"];
            foreach (string data in objectData)
            {
                string[] fields = data.Split(',');
                yield return new HitObject(double.Parse(fields[2]), fields[4] == "0" ? ObjectType.LEFT : ObjectType.RIGHT);
            }
        }
        private static Dictionary<string, List<string>> ReadFile(StreamReader stream)
        {
            // Declare variables
            Dictionary<string, List<string>> sections = new Dictionary<string, List<string>>();
            string line;
            string header = "";
            List<string> content = new List<string>();
            // Read in data from file
            while ((line = stream.ReadLine()) != null)
            {
                // All lines containing [ indicate a header
                if (line.Contains("["))
                {
                    // Add the previous section to the packages dictionary and begin a new section
                    sections.Add(header, content);
                    content = new List<string>();
                    header = line.Replace("[", "").Replace("]", "");
                }
                else
                    content.Add(line);
            }
            // Add the final package
            sections.Add(header, content);
            // Remove the empty section which is added to the start of the dictionary
            if (sections.Keys.Contains(""))
                sections.Remove("");
            return sections;
        }
    }
}
