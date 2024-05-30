using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;

namespace taikoclone
{
    // Source code from: https://github.com/ppy/osu-tools/blob/master/PerformanceCalculator/ProcessorWorkingBeatmap.cs#L59
    internal class MapDownloader
    {
        public StreamReader GetMap(int mapID)
        {
            string cachePath = $"{mapID}.osu";
            if (File.Exists($"../../{cachePath}"))
                return new StreamReader($"../../{cachePath}");
            var request = new FileWebRequest($"../../{cachePath}", $"https://osu.ppy.sh/osu/{mapID}");
        }
    }
}
