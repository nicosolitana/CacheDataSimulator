namespace CacheDataSimulator.Data
{
    class TextSegment
    {
        private string _address;
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
            }
        }

        private OpParameters _params;
        public OpParameters Params
        {
            get
            {
                return _params;
            }
            set
            {
                _params = value;
            }
        }

        private string _operation;
        public string Operation
        {
            get
            {
                return _operation;
            }
            set
            {
                _operation = value;
            }
        }

        private string _opCode;
        public string OpCode
        {
            get
            {
                return _opCode;
            }
            set
            {
                _opCode = value;
            }
        }

        private string _opCodeHex;
        public string OpCodeHex
        {
            get
            {
                return _opCodeHex;
            }
            set
            {
                _opCodeHex = value;
            }
        }

        private string _sourceCode;
        public string SourceCode
        {
            get
            {
                return _sourceCode;
            }
            set
            {
                _sourceCode = value;
            }
        }
    }
}
