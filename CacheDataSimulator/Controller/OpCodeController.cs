using CacheDataSimulator.Common;
using CacheDataSimulator.Data;
using System;
using System.Collections.Generic;

namespace CacheDataSimulator.Controller
{
    class OpCodeController
    {
        public static List<TextSegment> GenerateOpCode(List<TextSegment> txSegment)
        {
            foreach (var tx in txSegment)
            {
                string format = GetParamCount(tx.Operation);
                string type = GetType(tx.Operation);
                string opCode = string.Empty;
                if ((type == "Register") || (type == "Immediate") || (type == "Load"))
                {

                    string secondParam = "00000";
                    string thirdParam = string.Empty;
                    if (!string.IsNullOrEmpty(tx.Params.RSourceOne))
                        secondParam = tx.Params.RSourceOne;

                    if (!string.IsNullOrEmpty(tx.Params.RSourceTwo))
                        thirdParam = DataCleaner.PadHexValue(12, tx.Params.RSourceTwo);
                    
                    if (!string.IsNullOrEmpty(tx.Params.Immediate))
                        thirdParam = DataCleaner.PadHexValue(12, tx.Params.Immediate);
                    
                    if (string.IsNullOrEmpty(thirdParam))
                        thirdParam = "000000000000";

                    opCode = String.Format(format, tx.Params.RDestination, secondParam, thirdParam).Replace(" ", "");
                } else
                {
                    string firstImm = string.Empty;
                    string secondImm = string.Empty;
                    if (type == "Store")
                    {
                        if (tx.Params.Immediate == null)
                            tx.Params.Immediate = "000000000000";
                        secondImm = tx.Params.Immediate.Substring(0, 7);
                        firstImm = tx.Params.Immediate.Substring(7, 5);
                    } else
                    {

                        string thirdParam = tx.SourceCode.Split(' ')[3];
                        int currentAddr = Int32.Parse(Converter.ConvertHexToDec(tx.Address));
                        int i = txSegment.FindIndex(p => p.SourceCode.Contains(thirdParam+":"));
                        int lblAddr = Int32.Parse(Converter.ConvertHexToDec(txSegment[i].Address));
                        int sub = lblAddr - currentAddr;
                        string imm = DataCleaner.PadHexValue(12, Converter.ConvertDecToBin(sub.ToString()));

                        secondImm = imm.Substring(0, 1) + imm.Substring(2, 6);
                        firstImm = imm.Substring(8, 4) + imm.Substring(1, 1);
                        tx.Params.Immediate = secondImm + firstImm;
                    }
                    opCode = String.Format(format, firstImm, tx.Params.RDestination, tx.Params.RSourceOne, secondImm).Replace(" ", "");
                }
                tx.OpCode = opCode;
                tx.OpCodeHex = GenerateOpCodeHex(opCode);
            }
            return txSegment;
        }

        private static string GetParamCount(string directive)
        {
            foreach (var dir in StaticData.sysDataLst)
            {
                if (dir.Operation == directive.ToUpper())
                {
                    return dir.Format;
                }
            }
            return string.Empty;
        }

        private static string GetType(string directive)
        {
            foreach (var dir in StaticData.sysDataLst)
            {
                if(dir.Operation == directive.ToUpper())
                {
                    return dir.Type;
                }
            }
            return string.Empty;
        }

        private static string GenerateOpCodeHex(string binvalue)
        {
            int pos = 0;
            string opcode = "0x";
            for (int i = 0; i < 8; i++)
            {
                string hx = binvalue.Substring(pos, 4);
                pos = pos + 4;
                opcode = opcode + Convert.ToString(Convert.ToInt64(hx, 2), 16);
            }
            return opcode;
        }
    }
}
