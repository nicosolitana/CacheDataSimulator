using CacheDataSimulator.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CacheDataSimulator.Controller
{
    class FileController
    {
        public static List<string> ReadFile(string filePath, bool IsSysData)
        {
            if (File.Exists(filePath))
                return File.ReadAllLines(filePath).ToList<string>();

            if (IsSysData)
            {
                string[] lines ={   "Register        ADD,SLT,SUB             0000000{2}{1}000{0}0110011",
                                    "Immediate       ADDI,SLTI               {2}{1}000{0}0000011",
                                    "Load            LB,LH,LW                {2}{1}000{0}0000011",
                                    "Store           SB,SH,SW                {3}{2}{1}000{0}0100011",
                                    "Branch          BEQ,BNE,BLT,BGE         {3}{2}{1}000{0}0100011"  };
                File.WriteAllLines(filePath, lines);
            }
            return null;
        }

        public static List<SystemData> ReadSystemData()
        {
            const string SYS_FILE_PATH = "system_input.txt";
            List<string> sysData = null;
            List<SystemData> sysDataLst = new List<SystemData>();

            while(sysData == null)
                sysData = ReadFile(SYS_FILE_PATH, true);
            
            foreach (var data in sysData)
            {
                string[] arr = Regex.Split(data, @"[\s]+");
                sysDataLst.Add(new SystemData() { 
                    Type = arr[0],
                    OpList = arr[1].Split(',').ToList<string>(),
                    Format = arr[2]
                });
                //arr[1].Split(',').ToList<string>()
            }
            return sysDataLst;
        }
    }
}
