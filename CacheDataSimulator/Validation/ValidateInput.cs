using System;

namespace CacheDataSimulator.Validation
{
    class ValidateInput
    {
        public static string IsBlockCacheSize(string value, string type)
        {
            string err = string.Empty;

            if (value == "Enter " + type + " Size")
                err = err + "ERROR: Invalid input on " + type + " Size." + "\r\n";
            else
            {
                bool IsNum = Int32.TryParse(value, out int res);
                if (!IsNum)
                {
                    err = err + "ERROR: Enter only numeric values on " + type + " Size." + "\r\n";
                }
                else
                {
                    if (res < 1)
                        err = err + "ERROR: Value on " + type + " Size must be greater than 0." + "\r\n";
                }
            }
            return err;
        }

        //public static string IsReplaceAlgoSet(bool mru, bool lru)
        //{
        //    string err = string.Empty;
        //    if ((!mru) && (!lru))
        //        err += "ERROR: Replacement Algorithm is not set." + "\r\n";

        //    return err;
        //}

        public static string HasCode(string code)
        {
            string err = string.Empty;
            if (string.IsNullOrEmpty(code))
                err += "ERROR: There is no source code on the editor." + "\r\n";
            return err;
        }

        public static string IsValidPath(string filePath)
        {
            string err = string.Empty;
            if (filePath == "Select *.asm file.")
                err += "ERROR: No *.asm file has been selected." + "\r\n";
            return err;
        }

        public static string NoErr()
        {
            string err = string.Empty;
            err += "\r\n" + "SUCCESS: The code has been built successfully with no errors." + "\r\n";
            return err;
        }

        public static string ExecuteMsg()
        {
            string err = string.Empty;
            err += "\r\n" + "SUCCESS: Code Execution has been completed." + "\r\n";
            return err;
        }

        public static string AssembleMsg()
        {
            string err = string.Empty;
            err += "\r\n" + "ASSEMBLE: Source code is being assembled!" + "\r\n";
            return err;
        }

    }
}
