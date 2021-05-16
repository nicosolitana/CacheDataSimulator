using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CacheDataSimulator.Common
{
    enum NUM_TYPES
    {
        BIN,
        HEX,
        DEC,
        OTH
    }

    class DataCleaner
    {
        public static string IsComment(string code)
        {
            var re = new Regex(@"#[A-Za-z0-9\s+(~`!@#$%^*\(\),.|)*]*");
            string result = (re.Replace(code, string.Empty)).Trim();
            return result;
        }

        public static List<string> ExtractSegment(List<string> code, string typeStart, string typeEnd)
        {
            List<string> codeLst = new List<string>();
            bool isFound = false;
            foreach (string line in code)
            {
                if ((line == typeEnd) && (!string.IsNullOrEmpty(typeEnd)))
                    break;

                string result = IsComment(line);
                if ((isFound) && (!string.IsNullOrEmpty(result)))
                    codeLst.Add(result);

                if (line == typeStart)
                    isFound = true;
            }
            return codeLst;
        }

        public static NUM_TYPES CheckNumberType(string number)
        {
            if (IsNumType(number, @"\A[0-1]+\Z"))
                return NUM_TYPES.BIN;

            if (IsNumType(number, @"\A[0-9]+\Z"))
                return NUM_TYPES.DEC;

            if(IsNumType(number, @"\A[A-Fa-f0-9]+\Z"))
                return NUM_TYPES.HEX;

            return NUM_TYPES.OTH;
        }

        public static bool IsNumType(string number, string format)
        {
            Regex rx = new Regex(format);
            if (rx.IsMatch(number))
                return true;
            else
                return false;
        }

        public static string PadHexValue(int maxLen, string hexValue)
        {
            string tmp = hexValue;
            if (tmp.StartsWith("Imm_"))
                tmp = tmp.Replace("Imm_", "");
            int diff = maxLen - tmp.Length;
            return String.Empty.PadRight(diff, '0') + tmp;
        }
    }
}
