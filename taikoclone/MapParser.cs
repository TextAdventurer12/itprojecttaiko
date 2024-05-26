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
            Dictionary<string, List<string>> packages = ReadFile(filepath);
            foreach (var package in packages)
            {
                Console.WriteLine(package.Key);
                foreach (var line in package.Value)
                    Console.WriteLine(line);
                Console.WriteLine();
            }
            return null;
        }
        private static Dictionary<string, List<string>> ReadFile(string filepath)
        {
            // Declare variables
            Dictionary<string, List<string>> packages = new Dictionary<string, List<string>>();
            string line;
            string header = "";
            List<string> content = new List<string>();
            // Read in data from file
            StreamReader reader = new StreamReader(filepath);
            while ((line = reader.ReadLine()) != null)
            {
                // All lines containing [ indicate a header
                if (line.Contains("["))
                {
                    // Add the previous section to the packages dictionary and begin a new section
                    packages.Add(header, content);
                    content = new List<string>();
                    header = line.Replace("[", "").Replace("]", "");
                }
                else
                    content.Add(line);
            }
            // Add the final package
            packages.Add(header, content);
            // Remove the empty section which is added to the start of the dictionary
            if (packages.Keys.Contains(""))
                packages.Remove("");
            return packages;
        }
    }
}
