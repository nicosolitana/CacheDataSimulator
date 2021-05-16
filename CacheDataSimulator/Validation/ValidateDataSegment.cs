using CacheDataSimulator.Common;
using CacheDataSimulator.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CacheDataSimulator.Validation
{
    class ValidateDataSegment
    {
        public static List<DataSegment> ValidateDS(List<string> code, out string err)
        {
            List<string> dsCode = DataCleaner.ExtractSegment(code, ".data", ".text");
            List<DataSegment> dataSG = new List<DataSegment>();
            dsCode = CheckFormat(code, dsCode, out err, out dataSG);
            dataSG = SetStoredValue(dataSG);
            dataSG = SetAddress(dataSG);
            return dataSG;
            //return dsCode;
        }

        private static List<DataSegment> SetAddress(List<DataSegment> dataSG)
        {
            int i = 0;
            foreach (var ds in dataSG)
            {
                ds.Addr = "0x" + DataCleaner.PadHexValue(8,Converter.ConvertDecToHex(i.ToString()));
                int count = ds.StoredValue.Count;
                i = i + (4 * count);
            }
            return dataSG;
        }

        private static List<DataSegment> SetStoredValue(List<DataSegment> dataSG)
        {
            foreach (var ds in dataSG)
            {
                int valueLength = CheckMaxValueLength(ds.Type);
                List<string> tempSV = new List<string>();
                string tempVal = string.Empty;
                int maxItr = 8 / valueLength;
                int i = 0;
                foreach (var value in ds.Value)
                {
                    if(i != maxItr)
                    {
                        tempVal += DataCleaner.PadHexValue(valueLength, value);
                        i++;
                    } 
                    
                    if(i == maxItr) {
                        tempSV.Add("0x" + tempVal);
                        tempVal = string.Empty;
                        i = 0;
                    }
                }

                if (!string.IsNullOrEmpty(tempVal))
                {
                    tempSV.Add("0x" + DataCleaner.PadHexValue(8, tempVal));
                }
                ds.StoredValue = tempSV;
            }
            return dataSG;
        }

        private static int CheckMaxValueLength(string type)
        {
            if (type.ToLower() == ".word")
                return 8;

            if (type.ToLower() == ".half")
                return 4;

            return 2;
        }

        private static List<string> CheckFormat(List<string> code, List<string> dataCode, out string errMsg, out List<DataSegment> dataSG)
        {
            errMsg = string.Empty;
            List<string> cleanedDS = new List<string>();
            List<string> varNameLst = new List<string>();
            dataSG = new List<DataSegment>();
            foreach (string line in dataCode)
            {
                string msg ;
                string[] varData = line.Split(' ');
                string varName, varDir, varValue;

                Validate(varData, code, line, out varName, out varDir, 
                    out varValue, out msg);
                errMsg += msg;
                varNameLst.Add(varName);
                cleanedDS.Add(varName + " " + varDir + " " + varValue);

                dataSG.Add(new DataSegment()
                {
                    Name = varName,
                    Type = varDir,
                    Value = varValue.Split(',').ToList<string>(),
                    StoredValue = null,
                    Addr = null
                }) ;

            }

            if (varNameLst.Distinct().Count() != varNameLst.Count())
                errMsg += "ERROR: There are duplicate variable names. " + "\r\n";

            if (!string.IsNullOrEmpty(errMsg))
                errMsg += "Ensure code follows the format [varname: .directive value1, value2, ...]." + "\r\n";

            return cleanedDS;
        }


        private static void Validate(string[] varData, List<string> code, string line,
            out string varName, out string varDir, out string varValue, out string err)
        {
            varName = string.Empty;
            varDir = string.Empty;
            varValue = string.Empty;
            err = string.Empty;

            string msg;
            string itemMsg = string.Empty;
            varName = CheckVarName(varData[0].Trim(), out msg);
            if (string.IsNullOrEmpty(varName))
                itemMsg += msg;

            if (varData.Length > 1)
            {
                varDir = CheckDirective(varData[1].Trim(), out msg);
                if (string.IsNullOrEmpty(varDir))
                    itemMsg += msg;

                List<string> varValueLst = ExtractVarValues(varData);
                varValue = CheckValues(varValueLst, out msg);
            }
            else
            {
                itemMsg += " \t- No directive and initial value specified." + "\r\n";
            }

            if ((!string.IsNullOrEmpty(msg)) || (!string.IsNullOrEmpty(itemMsg)))
            {
                int lineNum = code.FindIndex(x => x.Contains(line)) + 1;
                itemMsg += msg;
                err += "ERROR: The following are encountered on line " + lineNum.ToString() + "\r\n" + itemMsg;
            }
        }

        private static string CheckValues(List<string> storedValue, out string err)
        {
            err = string.Empty;
            List<string> cleanedVal = new List<string>();

            if (storedValue.Count == 0)
            {
                err = " \t- Please initialize variable." + "\r\n";
                return string.Empty;
            }

            foreach (var sv in storedValue)
            {
                string tmp = sv;
                if (tmp.Length > 3)
                {
                    if (tmp.Substring(0, 2) == "0x")
                        tmp = sv.Remove(0, 2);
                }
                NUM_TYPES type = DataCleaner.CheckNumberType(tmp);
                if((type == NUM_TYPES.DEC) || (type == NUM_TYPES.HEX) || (type == NUM_TYPES.BIN))
                    cleanedVal.Add(Converter.ConvertToHex(tmp));
                else
                    err = " \t- Specified an invalid value (" + tmp + ") that is neither an INT nor HEX." + "\r\n";
            }
            return string.Join(",", cleanedVal);
        }

        private static bool CheckNumType(string num, string format)
        {
            Regex regex = new Regex(format);
            Match match = regex.Match(num);
            if (match.Success)
                return true;
            return false;

        }

        private static List<string> ExtractVarValues(string[] varData)
        {
            List<string> storedValue = new List<string>();
            if (varData.Length <= 2)
            {
                return storedValue;
            }
            for (int i = 2; i < varData.Length; i++)
            {
                string[] splitData = varData[i].Split(',');
                foreach (var num in splitData)
                {
                    if (!string.IsNullOrEmpty(num.Trim()))
                        storedValue.Add(num.Trim());
                }
            }
            return storedValue;
        }

        private static string CheckVarName(string varName, out string err)
        {
            err = string.Empty;
            if (varName.EndsWith(":"))
                return varName;

            err = " \t- " + varName + " does not end with ':'" + "\r\n";
            return string.Empty;
        }

        private static string CheckDirective(string directive, out string err)
        {
            err = string.Empty;
            if (CheckSupportedDirective(directive.ToLower()))
                return directive;

            err = " \t- Specified directive is not supported." + "\r\n";
            return string.Empty;
        }

        private static bool CheckSupportedDirective(string directive)
        {
            if (directive == ".word")
                return true;

            if (directive == ".half")
                return true;

            if (directive == ".byte")
                return true;

            return false;
        }
    }
}
