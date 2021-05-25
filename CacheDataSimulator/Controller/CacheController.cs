namespace CacheDataSimulator.Controller
{
    class CacheController
    {
        static int CacheHit;
        static int CacheMiss;

        void init(bool firstExecution)
        {
            if(firstExecution)
            {
                CacheHit = 0;
                CacheMiss = 0;
            }
        }

        void CacheSimulation(bool hit)
        {
            if (hit)
                CacheHit++;
            else
                CacheMiss++;
        }

        void ComputeHit()
        {

        }

        void ComputeCacheMiss()
        {

        }

        public static string CacheHitRate()
        {
            return (CacheHit / (CacheMiss + CacheHit)).ToString();
        }
    }
}
