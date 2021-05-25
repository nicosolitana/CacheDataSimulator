namespace CacheDataSimulator.Data
{
    class SystemData
    {
        private string _type;
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
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

        //private List<string> _opList;
        //public List<string> OpList
        //{
        //    get
        //    {
        //        return _opList;
        //    }
        //    set
        //    {
        //        _opList = value;
        //    }
        //}

        private string _format;
        public string Format
        {
            get
            {
                return _format;
            }
            set
            {
                _format = value;
            }
        }

    }
}
