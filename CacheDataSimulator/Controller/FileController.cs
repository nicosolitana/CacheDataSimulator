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
                string[] lines ={
                    "Load        LB        {2}{1}000{0}0000011",
                    "Load        LH        {2}{1}001{0}0000011",
                    "Load        LW        {2}{1}010{0}0000011",
                    "Immediate   ADDI      {2}{1}000{0}0010011",
                    "Immediate   SLTI      {2}{1}010{0}0010011",
                    "Register    ADD       0000000{2}{1}000{0}0110011",
                    "Register    SLT       0000000{2}{1}010{0}0110011",
                    "Register    SUB       0100000{2}{1}000{0}0110011",
                    "Store       SB        {3}{2}{1}000{0}0100011",
                    "Store       SH        {3}{2}{1}001{0}0100011",
                    "Store       SW        {3}{2}{1}010{0}0100011",
                    "Branch      BEQ       {3}{2}{1}000{0}1100011",
                    "Branch      BNE       {3}{2}{1}001{0}1100011",
                    "Branch      BLT       {3}{2}{1}100{0}1100011",
                    "Branch      BGE       {3}{2}{1}101{0}1100011"  };
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
                    Operation = arr[1],
                    Format = arr[2]
                });
            }
            return sysDataLst;
        }
    }
}
