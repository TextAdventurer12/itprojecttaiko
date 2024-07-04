using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taikoclone.Utils
{
    internal static class StringUtils
    {
        public static string RemoveEndingWhitespace(this string str)
        {
            int end;
            for (end = str.Length - 1; end > 0 && char.IsWhiteSpace(str[end]); end--) ;
            if (end == 0)
                return "";
            ++end;
            return str.Remove(end, str.Length - end);
        }
        public static string AsDigits(string str)
        {
            List<char> digitList = str.Where(ch => char.IsDigit(ch) || ch == '.').ToList();
            return new string(digitList.ToArray());
        }
    }
}
