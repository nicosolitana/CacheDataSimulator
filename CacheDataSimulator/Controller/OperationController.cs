using CacheDataSimulator.Common;
using CacheDataSimulator.Data;
using System;
using System.Collections.Generic;

namespace CacheDataSimulator.Controller
{
    class OperationController
    {
        public static List<Register> ExecuteOperation(TextSegment tx, List<DataSegment> dx, List<Register> rxSG)
        {
            switch (tx.Operation.ToUpper())
            {
                case "LW":
                    rxSG = LW(tx, dx, rxSG);
                    break;
                case "ADD":
                    rxSG = ADD(tx, dx, rxSG);
                    break;
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
            int rsTwo = Int32.Parse(Converter.ConvertHexToDec(tx.Params.Immediate + tx.Params.RSourceTwo));
            string rsDest = "0x" + DataCleaner.PadHexValue(8, Converter.ConvertDecToHex((rsOne - rsTwo).ToString()));
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
            //int rsOne = GetRegisterValue(rxSG, GetRegisterIndex(rxSG, tx.Params.RDestination));
            
            string addr = "0x"+ DataCleaner.PadHexValue(8, Converter.ConvertBinToHex(tx.Params.Immediate));
            int index = dx.FindIndex(p => p.Addr == addr);
            string value = dx[index].Value[0];

            rxSG[GetRegisterIndex(rxSG, tx.Params.RDestination)].Value = "0x" + DataCleaner.PadHexValue(8, Converter.ConvertDecToHex(value));
            return rxSG;
        }

        // LOAD
        private static List<Register> SW(TextSegment tx, List<DataSegment> dx, List<Register> rxSG)
        {
            // SW x1, x2
            rxSG[GetRegisterIndex(rxSG, tx.Params.RSourceOne)].Value = rxSG[GetRegisterIndex(rxSG, tx.Params.RSourceTwo)].Value;
            return rxSG;
        }

        // BRANCH

    }
}
