using CacheDataSimulator.Common;
using CacheDataSimulator.Data;
using System;
using System.Collections.Generic;

namespace CacheDataSimulator.Controller
{
    class OperationController
    {
        public static string NextAddr;
        public static List<Register> ExecuteOperation(TextSegment tx, List<DataSegment> dx, List<Register> rxSG)
        {
            if (tx.Address == NextAddr)
            {
                if (tx.Params.Immediate == null)
                    tx.Params.Immediate = "000000";

                string next = (Int32.Parse(Converter.ConvertHexToDec(NextAddr.Replace("0x", ""))) + 4).ToString();
                NextAddr = "0x" + DataCleaner.PadHexValue(8, Converter.ConvertDecToHex(next));

                switch (tx.Operation.ToUpper())
                {
                    case "LW": rxSG = LW(tx, dx, rxSG); break;
                    case "LH": rxSG = LH(tx, dx, rxSG); break;
                    case "LB": rxSG = LB(tx, dx, rxSG); break;
                    case "ADD": rxSG = ADD(tx, dx, rxSG); break;
                    case "SLT": rxSG = SLT(tx, dx, rxSG); break;
                    case "SUB": rxSG = SUB(tx, dx, rxSG); break;
                    case "ADDI": rxSG = ADDI(tx, dx, rxSG); break;
                    case "SLTI": rxSG = SLTI(tx, dx, rxSG); break;
                    case "SW":
                    case "SH":
                    case "SB": rxSG = SW(tx, dx, rxSG);  break;
                    case "BEQ": rxSG = BEQ(tx, dx, rxSG); break;
                    case "BNE": rxSG = BNE(tx, dx, rxSG); break;
                    case "BLT": rxSG = BLT(tx, dx, rxSG); break;
                    case "BGE": rxSG = BGE(tx, dx, rxSG); break;
                }
            }

            return rxSG;
        }

        private static int GetRegisterIndex(List<Register> rxSG, string param)
        {
            string paramName = "x" + Converter.ConvertHexToDec(Converter.ConvertBinToHex(param));
            return rxSG.FindIndex(p => p.Name == paramName);
        }

        private static int GetRegisterValue(List<Register> rxSG, int index)
        {
            return Int32.Parse(Converter.ConvertHexToDec(rxSG[index].Value.Replace("0x", "")));
        }

        // REGISTER
        private static List<Register> ADD(TextSegment tx, List<DataSegment> dx, List<Register> rxSG)
        {
            // ADD x1, x2, x3
            int rsOne = GetRegisterValue(rxSG, GetRegisterIndex(rxSG, tx.Params.RSourceOne));
            int rsTwo = GetRegisterValue(rxSG, GetRegisterIndex(rxSG, tx.Params.RSourceTwo));
            string rsDest = "0x" + DataCleaner.PadHexValue(8,Converter.ConvertDecToHex((rsOne + rsTwo).ToString()));
            int index = GetRegisterIndex(rxSG, tx.Params.RDestination);
            rxSG[index].Value = rsDest;
            return rxSG;
        }

        private static List<Register> SLT(TextSegment tx, List<DataSegment> dx, List<Register> rxSG)
        {
            // SLT x1, x2, x3
            int rsOne = GetRegisterValue(rxSG, GetRegisterIndex(rxSG, tx.Params.RSourceOne));
            int rsTwo = GetRegisterValue(rxSG, GetRegisterIndex(rxSG, tx.Params.RSourceTwo));
            string rsDest = string.Empty;
            if (rsOne < rsTwo)
                rsDest = "0x" + DataCleaner.PadHexValue(8, Converter.ConvertDecToHex((1).ToString()));
            else
                rsDest = "0x" + DataCleaner.PadHexValue(8, Converter.ConvertDecToHex((0).ToString()));
            int index = GetRegisterIndex(rxSG, tx.Params.RDestination);
            rxSG[index].Value = rsDest;
            return rxSG;
        }

        private static List<Register> SUB(TextSegment tx, List<DataSegment> dx, List<Register> rxSG)
        {
            // SUB x1, x2, x3
            int rsOne = GetRegisterValue(rxSG, GetRegisterIndex(rxSG, tx.Params.RSourceOne));
            int rsTwo = GetRegisterValue(rxSG, GetRegisterIndex(rxSG, tx.Params.RSourceTwo));
            string rsDest = "0x" + DataCleaner.PadHexValue(8, Converter.ConvertDecToHex((rsOne - rsTwo).ToString()));
            int index = GetRegisterIndex(rxSG, tx.Params.RDestination);
            rxSG[index].Value = rsDest;
            return rxSG;
        }

        // IMMEDIATE
        private static List<Register> ADDI(TextSegment tx, List<DataSegment> dx, List<Register> rxSG)
        {
            // ADDI x1, x2, x3
            int rsOne = GetRegisterValue(rxSG, GetRegisterIndex(rxSG, tx.Params.RSourceOne));
            int rsTwo = Int32.Parse(Converter.ConvertBinToDec(tx.Params.Immediate + tx.Params.RSourceTwo));
            string rsDest = "0x" + DataCleaner.PadHexValue(8, Converter.ConvertDecToHex((rsOne + rsTwo).ToString()));
            int index = GetRegisterIndex(rxSG, tx.Params.RDestination);
            rxSG[index].Value = rsDest;
            return rxSG;
        }

        private static List<Register> SLTI(TextSegment tx, List<DataSegment> dx, List<Register> rxSG)
        {
            // SLTI x1, x2, x3
            int rsOne = GetRegisterValue(rxSG, GetRegisterIndex(rxSG, tx.Params.RSourceOne));
            int rsTwo = Int32.Parse(Converter.ConvertHexToDec(tx.Params.Immediate + tx.Params.RSourceTwo));
            string rsDest = string.Empty;
            if (rsOne < rsTwo)
                rsDest = "0x" + DataCleaner.PadHexValue(8, Converter.ConvertDecToHex((1).ToString()));
            else
                rsDest = "0x" + DataCleaner.PadHexValue(8, Converter.ConvertDecToHex((0).ToString()));

            int index = GetRegisterIndex(rxSG, tx.Params.RDestination);
            rxSG[index].Value = rsDest;
            return rxSG;
        }

        // LOAD
        private static List<Register> LW(TextSegment tx, List<DataSegment> dx, List<Register> rxSG)
        {
            // LW x1, var1
            string addr = "0x"+ DataCleaner.PadHexValue(8, Converter.ConvertBinToHex(tx.Params.Immediate));
            int index = dx.FindIndex(p => p.Addr == addr);
            string value = string.Empty;
            //for (int i = 0; i < 4; i++)
            for (int i = 0; i < dx[index].StoredValue.Count; i++)
            {
                value = dx[index].StoredValue[i] + value;
            }
            value = value.Replace("0x", "");
            rxSG[GetRegisterIndex(rxSG, tx.Params.RDestination)].Value = "0x" + DataCleaner.PadHexValue(8, value);
            //rxSG[GetRegisterIndex(rxSG, tx.Params.RDestination)].Value = dx[index].Addr; ;
            return rxSG;
        }

        private static List<Register> LH(TextSegment tx, List<DataSegment> dx, List<Register> rxSG)
        {
            // LW x1, var1
            string addr = "0x" + DataCleaner.PadHexValue(8, Converter.ConvertBinToHex(tx.Params.Immediate));
            int index = dx.FindIndex(p => p.Addr == addr);
            string value = string.Empty;
            for (int i = 0; i < 2; i++)
            {
                value = dx[index].StoredValue[i] + value;
            }
            value = value.Replace("0x", "");
            rxSG[GetRegisterIndex(rxSG, tx.Params.RDestination)].Value = "0x" + DataCleaner.PadHexValue(8, value);
            //rxSG[GetRegisterIndex(rxSG, tx.Params.RDestination)].Value = dx[index].Addr; ;
            return rxSG;
        }

        private static List<Register> LB(TextSegment tx, List<DataSegment> dx, List<Register> rxSG)
        {
            // LW x1, var1
            string addr = "0x" + DataCleaner.PadHexValue(8, Converter.ConvertBinToHex(tx.Params.Immediate));
            int index = dx.FindIndex(p => p.Addr == addr);
            string value = string.Empty;
            value = dx[index].StoredValue[0] + value;
            value = value.Replace("0x", "");
            rxSG[GetRegisterIndex(rxSG, tx.Params.RDestination)].Value = "0x" + DataCleaner.PadHexValue(8, value);
            //rxSG[GetRegisterIndex(rxSG, tx.Params.RDestination)].Value = dx[index].Addr; ;
            return rxSG;
        }

        // STORE
        private static List<Register> SW(TextSegment tx, List<DataSegment> dx, List<Register> rxSG)
        {
            // SW x1, x2
            rxSG[GetRegisterIndex(rxSG, tx.Params.RDestination)].Value = rxSG[GetRegisterIndex(rxSG, tx.Params.RSourceOne)].Value;
            return rxSG;
        }

        // BRANCH
        private static List<Register> BEQ(TextSegment tx, List<DataSegment> dx, List<Register> rxSG)
        {
            string imm = tx.Params.Immediate;
            string offset = imm.Substring(0, 1) + imm.Substring(11, 1) + imm.Substring(1, 9);
            int ioffSet = (Int32.Parse(Converter.ConvertBinToDec(offset)) * 2 ) + Int32.Parse(Converter.ConvertHexToDec(tx.Address.Replace("0x","")));
            int paramOne = GetRegisterValue(rxSG, GetRegisterIndex(rxSG, tx.Params.RSourceOne));
            int paramTwo = GetRegisterValue(rxSG, GetRegisterIndex(rxSG, tx.Params.RDestination));
            if (paramOne == paramTwo)
            {
                NextAddr = "0x" + DataCleaner.PadHexValue(8, Converter.ConvertDecToHex(ioffSet.ToString()));
            }
            return rxSG;
        }

        private static List<Register> BNE(TextSegment tx, List<DataSegment> dx, List<Register> rxSG)
        {
            string imm = tx.Params.Immediate;
            string offset = imm.Substring(0, 1) + imm.Substring(11, 1) + imm.Substring(1, 9);
            int ioffSet = (Int32.Parse(Converter.ConvertBinToDec(offset)) * 2) + Int32.Parse(Converter.ConvertHexToDec(tx.Address.Replace("0x", "")));
            int paramOne = GetRegisterValue(rxSG, GetRegisterIndex(rxSG, tx.Params.RSourceOne));
            int paramTwo = GetRegisterValue(rxSG, GetRegisterIndex(rxSG, tx.Params.RDestination));
            if (paramOne != paramTwo)
            {
                NextAddr = "0x" + DataCleaner.PadHexValue(8, Converter.ConvertDecToHex(ioffSet.ToString()));
            }
            return rxSG;
        }

        private static List<Register> BLT(TextSegment tx, List<DataSegment> dx, List<Register> rxSG)
        {
            string imm = tx.Params.Immediate;
            string offset = imm.Substring(0, 1) + imm.Substring(11, 1) + imm.Substring(1, 9);
            int ioffSet = (Int32.Parse(Converter.ConvertBinToDec(offset)) * 2) + Int32.Parse(Converter.ConvertHexToDec(tx.Address.Replace("0x", "")));
            int paramOne = GetRegisterValue(rxSG, GetRegisterIndex(rxSG, tx.Params.RSourceOne));
            int paramTwo = GetRegisterValue(rxSG, GetRegisterIndex(rxSG, tx.Params.RDestination));
            if (paramTwo < paramOne)
            {
                NextAddr = "0x" + DataCleaner.PadHexValue(8, Converter.ConvertDecToHex(ioffSet.ToString()));
            }
            return rxSG;
        }

        private static List<Register> BGE(TextSegment tx, List<DataSegment> dx, List<Register> rxSG)
        {
            string imm = tx.Params.Immediate;
            string offset = imm.Substring(0, 1) + imm.Substring(11, 1) + imm.Substring(1, 9);
            int ioffSet = (Int32.Parse(Converter.ConvertBinToDec(offset)) * 2) + Int32.Parse(Converter.ConvertHexToDec(tx.Address.Replace("0x", "")));
            int paramOne = GetRegisterValue(rxSG, GetRegisterIndex(rxSG, tx.Params.RSourceOne));
            int paramTwo = GetRegisterValue(rxSG, GetRegisterIndex(rxSG, tx.Params.RDestination));
            if (paramTwo > paramOne)
            {
                NextAddr = "0x" + DataCleaner.PadHexValue(8, Converter.ConvertDecToHex(ioffSet.ToString()));
            }
            return rxSG;
        }
    }
}
