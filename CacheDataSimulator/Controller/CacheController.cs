using CacheDataSimulator.Common;
using CacheDataSimulator.Data;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CacheDataSimulator.Controller
{
    class CacheController
    {
        public static int CacheHit;
        public static int CacheMiss;
        public static int blockSize;
        public static int wordSize;
        public static int tagSize;

        public static void Init(int tSize, int wSize, int bSize)
        {
            CacheHit = 0;
            CacheMiss = 0;
            wordSize = wSize;
            tagSize = tSize;
            blockSize = bSize;
        }

        private static List<Cache> UpdateCacheAge(List<Cache> cacheLst, int lowerBound, int blockSize)
        {
            int maxAge = cacheLst.Max(x => x.Age);
            int minAge = cacheLst.Min(x => x.Age);
            int upperBound = lowerBound + (blockSize * 4);
            int limit = cacheLst.Count / (blockSize * 4);
            int i = 0;
            foreach (var cache in cacheLst)
            {
                //4,5,6,7, i = 1 
                if ((i < lowerBound) || (upperBound < i))
                {
                    if(cache.Age != 0)
                        cache.Age++;

                    if (cache.Age > limit)
                        cache.Age = 1;
                }
                i++;
            }
            return cacheLst;
        }


        private static int GetRowIndex(DataTable dxDT, string addr)
        {
            int i = 0;
            foreach (DataRow row in dxDT.Rows)
            {
                if ((string)row["Address"] == addr)
                    break;
                i++;
            }
            return i;
        }

        public static List<Cache> UpdateCache(TextSegment tx, DataTable dxDT, List<Cache> cacheLst, bool IsMRU)
        {
            if ((tx.Operation.ToLower() == "lw") ||
                (tx.Operation.ToLower() == "lh") ||
                (tx.Operation.ToLower() == "lb"))
            {
                string addr = "0x" + DataCleaner.PadHexValue(8, Converter.ConvertBinToHex(tx.Params.Immediate));
                int IsInCache = cacheLst.FindIndex(p => p.Addr == addr);

                if (IsInCache == -1)
                {
                    CacheMiss++;

                    int item = cacheLst.Min(x => x.Age);
                    if (item != 0)
                    {
                        if(IsMRU)
                            item = cacheLst.Max(x => x.Age);
                        else
                            item = cacheLst.Min(x => x.Age);
                    }

                    IsInCache = cacheLst.FindIndex(p => p.Age == item);
                    int rowIndex = GetRowIndex(dxDT, addr);
                    int cacheIndex = IsInCache;
                    for (int i= 0; i < (blockSize * 4); i++)
                    {

                        if ((rowIndex < dxDT.Rows.Count) && (cacheIndex < cacheLst.Count))
                        {
                            //cacheLst[cacheIndex].Age++;
                            cacheLst[cacheIndex].Age = 1;
                            cacheLst[cacheIndex].Addr = dxDT.Rows[rowIndex]["Address"].ToString();
                            cacheLst[cacheIndex].Value = dxDT.Rows[rowIndex]["Value"].ToString();
                            string binValue = Converter.ConvertHexToBin(cacheLst[cacheIndex].Addr.Replace("0x", ""));

                            if (binValue.Length < 11)
                                binValue = DataCleaner.PadHexValue(11, binValue);

                            //string tagWord = binValue.Substring(binValue.Length - 12, 11);
                            cacheLst[cacheIndex].Word = binValue.Substring(tagSize, wordSize);
                            cacheLst[cacheIndex].Tag = binValue.Substring(0, tagSize);
                            rowIndex++;
                            cacheIndex++;
                        }
                    }
                } else
                {
                    CacheHit++;
                    int cacheIndex = IsInCache;
                    int lLimit = cacheIndex - (cacheIndex % (blockSize * 4));
                    int uLimit = lLimit + (blockSize * 4);
                   
                    int limit = cacheIndex % (blockSize * 4);
                    for (int i = 0; i < (blockSize * 4); i++)
                    {
                        if ((i > lLimit) && (i < uLimit))
                        {
                            cacheLst[cacheIndex].Age = 1;
                        }
                        //    cacheLst[cacheIndex].Age++;
                        //cacheIndex++;
                    }
                    IsInCache = lLimit;
                }
                cacheLst = UpdateCacheAge(cacheLst, IsInCache, blockSize);
            }
            return cacheLst;
        }

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
