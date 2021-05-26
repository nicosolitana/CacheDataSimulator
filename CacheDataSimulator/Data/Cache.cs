namespace CacheDataSimulator.Data
{
    class Cache
    {
        private string _tag;
        public string Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                _tag = value;
            }
        }

        private string _word;
        public string Word
        {
            get
            {
                return _word;
            }
            set
            {
                _word = value;
            }
        }

        private string _value;
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        private string _addr;
        public string Addr
        {
            get
            {
                return _addr;
            }
            set
            {
                _addr = value;
            }
        }


        private int _age;
        public int Age
        {
            get
            {
                return _age;
            }
            set
            {
                _age = value;
            }
        }
    }
}
