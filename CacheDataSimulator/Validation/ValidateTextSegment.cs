using CacheDataSimulator.Common;
using CacheDataSimulator.Controller;
using CacheDataSimulator.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CacheDataSimulator.Validation
{
    class ValidateTextSegment
    {
        private static List<string> rgList = new List<string>();
        private static List<DataSegment> dxSG;

        public static List<string> ValidateTS(List<DataSegment> dataSG, List<string> code, out string err, out List<TextSegment> txSegment)
        {
            dxSG = dataSG;
            err = string.Empty;
            string msg = string.Empty;
            List<string> txCode = DataCleaner.ExtractSegment(code, ".text", string.Empty);
            txSegment = new List<TextSegment>();
            txCode = CheckFormat(txCode, code, out err, out txSegment);
            if (string.IsNullOrEmpty(err))
            {
                txSegment = SetAddress(txSegment);
                txSegment = OpCodeController.GenerateOpCode(txSegment);
            }
            return txCode;
        }

        private static List<TextSegment> SetAddress(List<TextSegment> txSegment)
        {
            int i = 4096;
            foreach (var tx in txSegment)
            {
                tx.Address = "0x" + DataCleaner.PadHexValue(8, Converter.ConvertDecToHex(i.ToString()));
                i += 4;;
            }
            return txSegment;
        }

        private static List<string> CheckFormat(List<string> txCode, List<string> code, 
            out string errMsg, out List<TextSegment> txSegment)
        {
            errMsg = string.Empty;
            string err = string.Empty;
            List<string> cleanTxCode = new List<string>();
            txSegment = new List<TextSegment>();
            foreach (string tx in txCode)
            {
                string param = string.Empty;
                string lbl = string.Empty;
                string dir = string.Empty;
                string[] arr = tx.Split(' ');
                Validate(arr, tx, code, out lbl, out dir, out param, out err);
                errMsg += err;
                if (string.IsNullOrEmpty(err))
                {
                    cleanTxCode.Add(lbl + " " + dir + " " + " " + param);
                    txSegment.Add(new TextSegment()
                    {
                        Address = string.Empty,
                        Operation = dir,
                        Params = GetParameterObj(param),
                        OpCode = string.Empty,
                        SourceCode = tx,
                        OpCodeHex = string.Empty
                    });
                }
            }
            return cleanTxCode;
        }

        private static OpParameters GetParameterObj(string param)
        {
            string[] prms = param.Split(',');
            OpParameters paramObj = new OpParameters();
            for (int x = 0; x < prms.Length; x++)
            {
                if (x == 0)
                    paramObj.RDestination = DataCleaner.PadHexValue(5,prms[x]);

                
                if (x == 1)
                {
                    if (!prms[x].StartsWith("Imm_"))
                    {
                        paramObj.RSourceOne = DataCleaner.PadHexValue(5, prms[x]);
                    } else
                    {
                        string varName = prms[x].Replace("Imm_", "");
                        int index = dxSG.FindIndex(p => p.Name.Replace(":","") == varName);
                        if (index > -1)
                        {
                            paramObj.Immediate = DataCleaner.PadHexValue(12,Converter.ConvertDecToBin(Converter.ConvertHexToDec(dxSG[index].Addr.Replace("0x", ""))));
                        }
                    }
                }

                if (x == 2)
                {
                    if (!prms[x].StartsWith("Imm_"))
                        paramObj.RSourceTwo = DataCleaner.PadHexValue(5, prms[x]);
                    else
                    {
                        string varName = prms[x].Replace("Imm_", "");
                        int index = dxSG.FindIndex(p => p.Name.Replace(":", "") == varName);
                        if (index > -1)
                        {
                            paramObj.Immediate = DataCleaner.PadHexValue(12, Converter.ConvertDecToBin(Converter.ConvertHexToDec(dxSG[index].Addr.Replace("0x", ""))));
                        }
                    }
                }
            }

            if (prms[prms.Length - 1].StartsWith("Imm_"))
            {
                string varName = prms[prms.Length - 1].Replace("Imm_", "");
                int index = dxSG.FindIndex(p => p.Name.Replace(":", "") == varName);
                if (index > -1)
                {
                    paramObj.Immediate = DataCleaner.PadHexValue(12, Converter.ConvertDecToBin(Converter.ConvertHexToDec(dxSG[index].Addr.Replace("0x", ""))));
                } else
                {
                    paramObj.Immediate = DataCleaner.PadHexValue(12, prms[prms.Length - 1]).Replace("Imm_",""); 
                }
            }

            return paramObj;
        }

        public static void Validate(string[] arr, string line, List<string> code, 
            out string lbl, out string dir, out string param, out string err)
        {
            err = string.Empty;
            string msg;
            string itemMsg = string.Empty;
            int start = 0;
            bool HasDirective = false;
            int paramCount = 0;

            lbl = string.Empty;
            param = string.Empty;
            dir = string.Empty;

            if (!line.StartsWith("."))
            {
                lbl = CheckBranch(arr[0], out msg);

                if (string.IsNullOrEmpty(lbl))
                {
                    start = 0;
                    dir = arr[0];
                }

                if ((string.IsNullOrEmpty(dir)) && (arr.Length > 1))
                {
                    start = 1;
                    dir = arr[1];
                }

                if (!string.IsNullOrEmpty(dir)){
                    paramCount = CheckDirective(dir.ToUpper(), out msg);
                    if (paramCount == 0)
                        itemMsg += msg;
                    HasDirective = true;
                }

                List<string> paramLst = ExtractParameters(arr, start);
                param = CheckParams(paramLst, HasDirective, paramCount, out msg);
            }
            else
            {
                string[] tmp = line.Split(' ');
                msg = " \t- Invalid directive " + tmp[0] + "." + "\r\n";
            }

            if ((!string.IsNullOrEmpty(msg)) || (!string.IsNullOrEmpty(itemMsg)))
            {
                int lineNum = code.FindIndex(x => x.Contains(line)) + 1;
                itemMsg += msg;
                err += "ERROR: The following are encountered on line " + lineNum.ToString() + "\r\n" + itemMsg;
            }
        }

        private static string CheckBranch(string code, out string err)
        {
            err = string.Empty;
            if (code.EndsWith(":"))
                return code;

            return string.Empty;
        }

        private static string CheckParams(List<string> paramLst, bool HasDirective, int paramCount,out string err)
        {
            List<string> cleanParamLst = new List<string>();
            err = string.Empty;
            if ((paramLst.Count == 0) && (HasDirective))
                err += " \t- No parameters were specified." + "\r\n";
            else
            {
                paramLst.RemoveAll(s => string.IsNullOrWhiteSpace(s));
                foreach (var param in paramLst)
                {
                    if (!string.IsNullOrEmpty(param))
                    {
                        string format;
                        int type;
                        string paramBin = param;
                        GetNumTypeFormat(param, out type, out format);

                        Regex regex = new Regex(format);
                        Match match = regex.Match(param);
                        if ((!match.Success) || (!IsValidWord(param)))
                            err += " \t- Parameter " + param + " is not valid." + "\r\n";
                        else
                        {
                            string msg = string.Empty;
                            paramBin = ExtractValue(type, param, out msg);
                            err += msg;
                        }
                        cleanParamLst.Add(paramBin);
                    }
                }
                if((paramCount != 0) && (paramCount != paramLst.Count))
                    err += " \t- The number of parameters exceed allowable count." + "\r\n";
            }
            return string.Join(",", cleanParamLst);
        }

        private static string ExtractValue(int type, string param, out string err)
        {
            err = string.Empty;
            string num = GetNumber(param);
            if ((!string.IsNullOrEmpty(num)) && (type > 0) && (Int32.Parse(num) > 32))
            {
                err += " \t- Parameter " + param + " has exceeded the maximum register count." + "\r\n";
                return num;
            }
            else
            {
                if (type == 0)
                    return "Imm_" + Converter.ConvertNumber(param);

                if (type == 1)
                    return Converter.ConvertDecToBin(num);

                if (type == 2)
                {
                    string paramBin = Converter.ConvertDecToBin(num);
                    num = Converter.ConvertNumber(GetImmediate(param));
                    if (num != "0") { 
                        if (DataCleaner.IsNumType(num, @"\A[0-1]+\Z"))
                            return paramBin + "," + "Imm_" + num;
                        else
                            err += " \t- Immediate value " + num + " is neither a hexadecimal, binary or decimal." + "\r\n";
                    } 
                    return paramBin;
                }
            }
            return string.Empty;
        }

        private static void GetNumTypeFormat(string param, out int type, out string format)
        {
            format = @"[A-Fa-f0-9]+";
            type = 0;
            if (param.Contains('x'))
            {
                format = @"x[0-9]+";
                type = 1;

                if (param.Contains('('))
                {
                    type = 2;
                    format = @"[A-Fa-f0-9]*[(]x[0-9]+[)]";  // @"0[(]x[0-9]+[)]"
                }
            }
        }

        private static string GetNumber(string str)
        {
            bool IsX = false;
            string tmp = string.Empty;
            foreach (var c in str)
            {
                if ((IsX) && (!Char.IsDigit(c)))
                    break;

                if ((IsX) && (Char.IsDigit(c)))
                    tmp = tmp + c;

                if (c == 'x')
                    IsX = true;
            }
            return tmp;
        }

        private static string GetImmediate(string str)
        {
            string tmp = string.Empty;
            foreach (var c in str)
            {
                if (c == '(')
                    break;

                tmp = tmp + c;
            }
            return tmp;
        }

        public static int IsBalancedParenthesis(char[] res)
        {
            List<char> temp = new List<char>();
            for (int i = 0; i < res.Length; i++)
            {
                if (res[i] == ')')
                {
                    if ((temp.Count > 0) &&
                        (temp[temp.Count - 1] == '('))
                        temp.RemoveAt(temp.Count - 1);
                    else
                        temp.Add(res[i]);
                }
                else
                    temp.Add(res[i]);
            }
            return temp.Count;
        }

        public static bool IsValidWord(string str)
        {
            var res = (Regex.Replace(str, @"[A-Za-z0-9]*", string.Empty)).ToCharArray();
            if (res.Length == 0)
                return true;
            if (IsBalancedParenthesis(res) == 0)
                return true;

            return false;
        }


        private static List<string> ExtractParameters(string[] arr, int start)
        {
            List<string> paramLst = new List<string>();
            for (int i = start+1; i < arr.Length; i++)
            {
                string[] splt = arr[i].Split(',');
                foreach (var s in splt)
                {
                    paramLst.Add(s.Trim());
                }
            }
            return paramLst;
        }

        private static int CheckDirective(string directive, out string err)
        {
            err = string.Empty;
            int count = GetParamCount(directive);
            if (count != 0)
                return count;
            err = " \t- Specified an invalid directive (" + directive + ")." + "\r\n";
            return 0;
        }

        private static int GetParamCount(string directive)
        {
            foreach (var dir in StaticData.sysDataLst)
            {
                if (dir.Operation == directive.ToUpper())
                {
                    int index = StaticData.sysDataLst.IndexOf(dir);
                    if ((StaticData.sysDataLst[index].Type == "Load") ||
                       (StaticData.sysDataLst[index].Type == "Store"))
                        return 2;
                    return 3;
                }
            }
            return 0;
        }
    }
}
