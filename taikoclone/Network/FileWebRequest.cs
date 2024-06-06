// Code is a modified version of the FileWebRequest class of https://github.com/ppy/osu-framework/tree/master/osu.Framework/IO/Network

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace taikoclone.Network
{
    internal class FileWebRequest : WebRequest
    {
        public string Filename;

        protected override Stream CreateOutputStream()
        {
            string path = Path.GetDirectoryName(Filename);
            if (!string.IsNullOrEmpty(path)) Directory.CreateDirectory(path);

            return new FileStream(Filename, FileMode.Create, FileAccess.Write, FileShare.Write, 32768);
        }

        public FileWebRequest(string filename, string url)
            : base (url)
        {
            Timout *= 2;
            Filename = filename;
        }
    }
}
