using CacheDataSimulator.Common;
using CacheDataSimulator.Data;
using CacheDataSimulator.Validation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace CacheDataSimulator.Controller
{
    class MainController
    {
        public List<TextSegment> txSG;
        public List<DataSegment> dxSG;
        public List<Register> rxSG;
        public List<Cache> cacheList;
        public DataTable DataSGDT;
        private DataTable TextSGDT;
        private DataTable RegisterDT;
        private DataTable CacheDT;

        public MainController()
        {

        }

        public List<Cache> InitializeCache(int rowCount, int blockSize)
        {
            int wordSize = DataCleaner.BitCounter(blockSize);
            int mmSize = 11;
            int tagSize = mmSize - wordSize;

            cacheList = new List<Cache>();
            for (int i = 0; i < rowCount; i++)
            {
                cacheList.Add(new Cache()
                {
                    Tag = DataCleaner.PadHexLeftValue(tagSize, ""),
                    Word = DataCleaner.PadHexLeftValue(wordSize, ""),
                    Addr = "0",
                    Value = "0",
                    Age = 0
                }); 
            }
            return cacheList;
        }

        public DataTable GenerateCacheDT()
        {
            CacheDT = new DataTable();
            CacheDT.Columns.Add("Tag", typeof(string));
            CacheDT.Columns.Add("Word", typeof(string));
            CacheDT.Columns.Add("Data", typeof(string));

            for (int i = 0; i < cacheList.Count; i++)
            {
                CacheDT.Rows.Add(cacheList[i].Tag, cacheList[i].Word, cacheList[i].Value);
            }
            return CacheDT;
        }

        public List<Register> GenerateRegister()
        {
            rxSG = new List<Register>();
            for (int i = 0; i < 32; i++)
            {
                rxSG.Add(new Register()
                {
                    Name = "x" + i.ToString(),
                    Value = "0x00000000"
                });
            }
            return rxSG;
        }

        public DataTable GenerateRegisterSGDT()
        {
            RegisterDT = new DataTable();
            RegisterDT.Columns.Add("Name", typeof(string));
            RegisterDT.Columns.Add("Value", typeof(string));

            for (int i = 0; i < 32; i++)
            {
                RegisterDT.Rows.Add(rxSG[i].Name, rxSG[i].Value);
            }
            return RegisterDT;
        }


        // MAIN EVENTS

        public string BuildSourceCode(string blockSize, string cacheSize, string sourceCode, bool IsMRU)
        {
            string initErr = string.Empty;
            string codeErr = string.Empty;
            //List<string> code = Regex.Split(sourceCode, Environment.NewLine).ToList<string>();
            char[] delims = new[] { '\r', '\n' };
            List<string> code = sourceCode.Split(delims, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
            dxSG = new List<DataSegment>();
            txSG = new List<TextSegment>();

            try
            {
                initErr = InitValidation(blockSize, cacheSize, sourceCode, IsMRU);
                codeErr = CodeValidation(code, out dxSG, out txSG);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return initErr + codeErr;
        }

        public DataTable GenerateDataSGDT()
        {
            DataSGDT = new DataTable();
            string initAddr = "0x00000000";

            // Create Columns
            DataSGDT.Columns.Add("Address", typeof(string));
            DataSGDT.Columns.Add("Name", typeof(string));
            DataSGDT.Columns.Add("Value", typeof(string));

            foreach (var data in dxSG)
            {
                initAddr = data.Addr;
                foreach (var value in data.StoredValue)
                {
                    DataSGDT.Rows.Add(initAddr, data.Name.Remove(data.Name.Length - 1, 1), value);
                    int addr = Convert.ToInt32(Converter.ConvertHexToDec(initAddr.Remove(0, 2))) + 1;
                    initAddr = "0x" + DataCleaner.PadHexValue(8, Converter.ConvertDecToHex(addr.ToString()));
                }
            }

            int limit = 2047 - Int32.Parse(Converter.ConvertHexToDec(initAddr.Remove(0, 2)));
            for (int i=0; i < limit; i++)
            {
                int addr = Convert.ToInt32(Converter.ConvertHexToDec(initAddr.Remove(0, 2))) + 1;
                initAddr = "0x" + DataCleaner.PadHexValue(8, Converter.ConvertDecToHex(addr.ToString()));
                DataSGDT.Rows.Add(initAddr, "", "0x00");
            }
            return DataSGDT;
        }

        public DataTable GenerateTextSGDT()
        {
            TextSGDT = new DataTable();

            // Create Columns
            TextSGDT.Columns.Add("Address", typeof(string));
            TextSGDT.Columns.Add("Opcode", typeof(string));
            TextSGDT.Columns.Add("Source Code", typeof(string));

            foreach (var text in txSG)
            {
                TextSGDT.Rows.Add(text.Address, text.OpCodeHex, text.SourceCode);
            }
            return TextSGDT;
        }


        // VALIDATION EVENTS
        private string InitValidation(string blockSize, string cacheSize, string sourceCode, bool IsMRU)
        {
            string err = string.Empty;
            err += ValidateInput.IsBlockCacheSize(blockSize, "Block");
            err += ValidateInput.IsBlockCacheSize(cacheSize, "Cache");
            err += ValidateInput.HasCode(sourceCode);
            //err += ValidateInput.IsReplaceAlgoSet(IsMRU);
            return err;
        }

        private string CodeValidation(List<string> code, out List<DataSegment> dataSG, out List<TextSegment> txSegment)
        {
            string err, msgErr;
            msgErr = string.Empty;
            dataSG = ValidateDataSegment.ValidateDS(code, out err);
            msgErr += err;

            txSegment = new List<TextSegment>();
            err = string.Empty;
            List<string> txCode = ValidateTextSegment.ValidateTS(dataSG, code, out err, out txSegment);
            msgErr += err;

            return msgErr;
        }
    }
}
