using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using taikoclone.Network;

namespace taikoclone
{
    internal static class MapDownloader
    {
        public static StreamReader GetMap(int mapID)
        {
            string cachePath = $"../../{mapID}.osu";
            if (File.Exists(cachePath))
                return new StreamReader(cachePath);
            Console.WriteLine($"Downloading {mapID}.osu");
            var request = new FileWebRequest(cachePath, $"https://osu.ppy.sh/osu/{mapID}");
            request.Perform();
            while (!request.Completed) ;
            return new StreamReader(cachePath);
        }
    }
}