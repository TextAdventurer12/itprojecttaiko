using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace  taikoclone.Utils
{
    public static class IOUtils
    {
        public static IEnumerable<string> dumpFile(StreamReader source)
        {
            string line;
            while ((line = source.ReadLine()) != null)
                yield return line;
        }
    }
}