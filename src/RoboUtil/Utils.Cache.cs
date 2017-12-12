using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboUtil
{
    partial class Utils
    {
        private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, object>> CacheTable = new ConcurrentDictionary<string, ConcurrentDictionary<string, object>>();

        public static ConcurrentDictionary<string, object> Cache(string key)
        {
            ConcurrentDictionary<string, object> internalCache;
            if (CacheTable.TryGetValue(key, out internalCache)) return internalCache;

            ConcurrentDictionary<string, object> newCache = new ConcurrentDictionary<string, object>();

            CacheTable[key] = newCache;

            return newCache;
        }
    }
}