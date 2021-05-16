using System;

namespace CacheDataSimulator.Common
{
    class Converter
    {
        public static string ConvertDecToBin(string number)
        {
            return Convert.ToString(Convert.ToInt64(number, 10), 2);
        }

        public static string ConvertDecToHex(string number)
        {
            return Convert.ToString(Convert.ToInt64(number, 10), 16).ToUpper();
        }
        public static string ConvertHexToDec(string number)
        {
            return Convert.ToString(Convert.ToInt64(number, 16), 10);
        }

        public static string ConvertHexToBin(string number)
        {
            return Convert.ToString(Convert.ToInt64(number, 16), 2);
        }

        public static string ConvertBinToHex(string number)
        {
            return Convert.ToString(Convert.ToInt64(number, 2), 16).ToUpper();
        }

        public static string ConvertToHex(string number)
        {
            if (number.StartsWith("0x"))
                number = number.Remove(0, 2);

            NUM_TYPES type = DataCleaner.CheckNumberType(number);
            if (type == NUM_TYPES.DEC)
                return ConvertDecToHex(number);

            return number;
        }

        public static string ConvertNumber(string number)
        {
            NUM_TYPES type = DataCleaner.CheckNumberType(number);
            if (type == NUM_TYPES.DEC)
                return ConvertDecToBin(number);

            if (type == NUM_TYPES.HEX)
                return ConvertHexToBin(number);

            return number;
        }
    }
}
