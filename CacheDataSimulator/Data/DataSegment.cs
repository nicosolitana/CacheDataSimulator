using System.Collections.Generic;

namespace CacheDataSimulator.Data
{
    class DataSegment
    {
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

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

        private List<string> _value;
        public List<string> Value
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

        private List<string> _storedValue;
        public List<string> StoredValue
        {
            get
            {
                return _storedValue;
            }
            set
            {
                _storedValue = value;
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
    }
}
