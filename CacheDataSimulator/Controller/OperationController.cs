using CacheDataSimulator.Common;
using CacheDataSimulator.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CacheDataSimulator.Controller
{
    class OperationController
    {
        public static string NextAddr;
        private static TextSegment tx;
        //private static DataSegment dx
        private static DataTable dxDT;
        private static List<Register> rxSG;
        //public static List<Register> ExecuteOperation(TextSegment tx, List<DataSegment> dx, List<Register> rxSG)
        public static List<Register> ExecuteOperation(TextSegment _tx, DataTable _dx, List<Register> _rxSG)
        {
            if (_tx.Address == NextAddr)
            {
                tx = _tx;
                dxDT = _dx;
                rxSG = _rxSG;

                if (tx.Params.Immediate == null)
                    tx.Params.Immediate = "000000";

                string next = (Int32.Parse(Converter.ConvertHexToDec(NextAddr.Replace("0x", ""))) + 4).ToString();
                NextAddr = "0x" + DataCleaner.PadHexValue(8, Converter.ConvertDecToHex(next));

                switch (tx.Operation.ToUpper())
                {
                    case "LW": rxSG = LW(); break;
                    case "LH": rxSG = LH(); break;
                    case "LB": rxSG = LB(); break;
                    case "ADD": rxSG = ADD(); break;
                    case "SLT": rxSG = SLT(); break;
                    case "SUB": rxSG = SUB(); break;
                    case "ADDI": rxSG = ADDI(); break;
                    case "SLTI": rxSG = SLTI(); break;
                    case "SW":                                  // should sign extend???
                    case "SH":
                    case "SB": rxSG = SW();  break;
                    case "BEQ": rxSG = BEQ(); break;
                    case "BNE": rxSG = BNE(); break;
                    case "BLT": rxSG = BLT(); break;
                    case "BGE": rxSG = BGE(); break;
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
        private static List<Register> ADD()
        {
            // ADD x1, x2, x3
            int rsOne = GetRegisterValue(rxSG, GetRegisterIndex(rxSG, tx.Params.RSourceOne));
            int rsTwo = GetRegisterValue(rxSG, GetRegisterIndex(rxSG, tx.Params.RSourceTwo));
            string rsDest = "0x" + DataCleaner.PadHexValue(8,Converter.ConvertDecToHex((rsOne + rsTwo).ToString()));
            int index = GetRegisterIndex(rxSG, tx.Params.RDestination);
            rxSG[index].Value = rsDest;
            return rxSG;
        }

        private static List<Register> SLT()
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

        private static List<Register> SUB()
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
        private static List<Register> ADDI()
        {
            // ADDI x1, x2, x3
            int rsOne = GetRegisterValue(rxSG, GetRegisterIndex(rxSG, tx.Params.RSourceOne));
            //if()
            int rsTwo = Int32.Parse(Converter.ConvertBinToDec(tx.Params.Immediate + tx.Params.RSourceTwo));
            string rsDest = "0x" + DataCleaner.PadHexValue(8, Converter.ConvertDecToHex((rsOne + rsTwo).ToString()));
            int index = GetRegisterIndex(rxSG, tx.Params.RDestination);
            rxSG[index].Value = rsDest;
            return rxSG;
        }

        private static List<Register> SLTI()
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
        private static List<Register> LW()
        {
            //// LW x1, var1
            //string addr = "0x"+ DataCleaner.PadHexValue(8, Converter.ConvertBinToHex(tx.Params.Immediate));
            //int index = dx.FindIndex(p => p.Addr == addr);
            //string value = string.Empty;
            ////for (int i = 0; i < 4; i++)
            //for (int i = 0; i < dx[index].StoredValue.Count; i++)
            //{
            //    value = dx[index].StoredValue[i] + value;
            //}
            //value = value.Replace("0x", "");
            //rxSG[GetRegisterIndex(rxSG, tx.Params.RDestination)].Value = "0x" + DataCleaner.PadHexValue(8, value);
            ////rxSG[GetRegisterIndex(rxSG, tx.Params.RDestination)].Value = dx[index].Addr; ;
            int imm = Int32.Parse(Converter.ConvertBinToDec(tx.Params.Immediate));
            int sourceOne = 0;
            if (tx.Params.RSourceOne != null)
            {

                sourceOne = Int32.Parse(Converter.ConvertHexToDec(
                        rxSG[GetRegisterIndex(rxSG, tx.Params.RSourceOne)].Value.Replace("0x", "")
                    ));
            }

            string addr = "0x" + DataCleaner.PadHexValue(8, Converter.ConvertDecToHex((imm + sourceOne).ToString()));
            string value = GetRowValue(addr, 4);
            value = value.Replace("0x", "");
            rxSG[GetRegisterIndex(rxSG, tx.Params.RDestination)].Value = "0x" + DataCleaner.PadHexValue(8, value);
            return rxSG;
        }

        private static List<Register> LH()
        {
            // LW x1, var1
            //string addr = "0x" + DataCleaner.PadHexValue(8, Converter.ConvertBinToHex(tx.Params.Immediate));
            //int index = dx.FindIndex(p => p.Addr == addr);
            //string value = string.Empty;
            //for (int i = 0; i < 2; i++)
            //{
            //    value = dx[index].StoredValue[i] + value;
            //}
            //value = value.Replace("0x", "");
            //rxSG[GetRegisterIndex(rxSG, tx.Params.RDestination)].Value = "0x" + DataCleaner.PadHexValue(8, value);
            ////rxSG[GetRegisterIndex(rxSG, tx.Params.RDestination)].Value = dx[index].Addr; ;

            int imm = Int32.Parse(Converter.ConvertBinToDec(tx.Params.Immediate));
            int sourceOne = 0;
            if (tx.Params.RSourceOne != null)
            {

                sourceOne = Int32.Parse(Converter.ConvertHexToDec(
                        rxSG[GetRegisterIndex(rxSG, tx.Params.RSourceOne)].Value.Replace("0x", "")
                    ));
            }

            string addr = "0x" + DataCleaner.PadHexValue(8, Converter.ConvertDecToHex((imm + sourceOne).ToString()));
            string value = GetRowValue(addr, 2);
            value = value.Replace("0x", "");
            rxSG[GetRegisterIndex(rxSG, tx.Params.RDestination)].Value = "0x" + DataCleaner.PadHexValue(8, value);
            return rxSG;
        }

        private static List<Register> LB()
        {
            // LW x1, var1
            int imm = Int32.Parse(Converter.ConvertBinToDec(tx.Params.Immediate));
            int sourceOne = 0;
            if (tx.Params.RSourceOne != null)
            {
                
                sourceOne = Int32.Parse(Converter.ConvertHexToDec(
                        rxSG[GetRegisterIndex(rxSG, tx.Params.RSourceOne)].Value.Replace("0x","")
                    ));
            }

            string addr = "0x" + DataCleaner.PadHexValue(8,Converter.ConvertDecToHex((imm + sourceOne).ToString()));
            string value = GetRowValue(addr, 1);
            value = value.Replace("0x", "");
            rxSG[GetRegisterIndex(rxSG, tx.Params.RDestination)].Value = "0x" + DataCleaner.PadHexValue(8, value);
            //rxSG[GetRegisterIndex(rxSG, tx.Params.RDestination)].Value = dx[index].Addr; ;
            return rxSG;
        }

        // STORE
        private static List<Register> SW()
        {
            // SW x1, x2
            rxSG[GetRegisterIndex(rxSG, tx.Params.RDestination)].Value = rxSG[GetRegisterIndex(rxSG, tx.Params.RSourceOne)].Value;
            return rxSG;
        }

        // BRANCH
        private static List<Register> BEQ()
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

        private static List<Register> BNE()
        {
            string imm = tx.Params.Immediate;
            string offset = imm.Substring(0, 1) + imm.Substring(11, 1) + imm.Substring(1, 10);
            int num;
            if (offset.Substring(0,1) == "1")
            {
                offset = DataCleaner.PadHexOpValue(32, offset, '1');
                num = Int32.Parse(Convert.ToString(Convert.ToInt32(offset, 2), 10));
            } else
            {
                num = Int32.Parse(Converter.ConvertBinToDec(offset));
            }
            int ioffSet = (num * 2) + Int32.Parse(Converter.ConvertHexToDec(tx.Address.Replace("0x", "")));
            int paramOne = GetRegisterValue(rxSG, GetRegisterIndex(rxSG, tx.Params.RSourceOne));
            int paramTwo = GetRegisterValue(rxSG, GetRegisterIndex(rxSG, tx.Params.RDestination));
            if (paramOne != paramTwo)
            {
                NextAddr = "0x" + DataCleaner.PadHexValue(8, Converter.ConvertDecToHex(ioffSet.ToString()));
            }
            return rxSG;
        }

        private static List<Register> BLT()
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

        private static List<Register> BGE()
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

        private static string GetRowValue(string addr, int limit)
        {
            int i = -1;
            int oLimit = limit;
            string value = string.Empty;
            foreach (DataRow row in dxDT.Rows)
            {
                if(oLimit != limit)
                {
                    value = (string)row["Value"] + value.Replace("0x","");
                    limit--;
                }

                if ((string)row["Address"] == addr)
                {
                    value = (string)row["Value"] + value;
                    limit--;
                }


                if(limit == 0)
                    break;

                i++;
            }

            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace("0x", "");
                int bigE = Int32.Parse(Converter.ConvertHexToDec(value.Substring(0, 1)));

                if (bigE < 8)
                    value = DataCleaner.PadHexOpValue(8, value, '0');
                else
                    value = DataCleaner.PadHexOpValue(8, value, 'F');
            }

            return value;
        }
    }
}
