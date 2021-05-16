namespace CacheDataSimulator.Data
{
    class OpParameters
    {

        private string _rDestination;
        public string RDestination
        {
            get
            {
                return _rDestination;
            }
            set
            {
                _rDestination = value;
            }
        }
        private string _rSourceOne;
        public string RSourceOne
        {
            get
            {
                return _rSourceOne;
            }
            set
            {
                _rSourceOne = value;
            }
        }

        private string _rSourceTwo;
        public string RSourceTwo
        {
            get
            {
                return _rSourceTwo;
            }
            set
            {
                _rSourceTwo = value;
            }
        }

        private string _immediate;
        public string Immediate
        {
            get
            {
                return _immediate;
            }
            set
            {
                _immediate = value;
            }
        }
    }
}
