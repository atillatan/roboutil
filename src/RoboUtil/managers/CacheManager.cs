using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using RoboUtil.managers.cache;


namespace RoboUtil.managers
{
    public class CacheManager
    {
        #region Singleton Implementation

        private static CacheManager _cacheManager = null;

        private static object syncRoot = new Object();
        private CacheManager()
        {
            Initialize();
        }
        public static CacheManager Instance
        {
            get
            {
                if (_cacheManager == null)
                {
                    lock (syncRoot)
                    {
                        if (_cacheManager == null)
                            _cacheManager = new CacheManager();
                    }
                }
                return _cacheManager;
            }
        }

        #endregion Singleton Implementation

        /// <summary>
        /// Thread Safe collection
        /// </summary>
        private static ConcurrentDictionary<string, ICache> _cache = null;

        /// <summary>
        /// you must add only thread safe collection, into Caches
        /// using Add methods in CacheManager more safe then using Caches.Add()...
        /// </summary>
        public ConcurrentDictionary<string, ICache> Caches
        {
            get
            {
                return _cache;
            }
        }
        private void Initialize()
        {
            _cache = new ConcurrentDictionary<string, ICache>();
        }

        #region Constructors
        private ICache _create(string cacheName, CacheProperties cacheProperties, IDictionary<string, object> cacheItems)
        {

            CacheCollectionType? cacheCollectionType = CacheCollectionType.MemoryCache;//default
            if (cacheProperties != null && cacheProperties.CacheCollectionType!=null) cacheCollectionType = cacheProperties.CacheCollectionType;          

            //return existed cache, when cache is already exist
            if (_cache.ContainsKey(cacheName)) throw new Exception("CacheName already exist");

            ICache cache = null;

            if (cacheCollectionType.Equals(CacheCollectionType.DictionaryCache))
                cache = new DictionaryCacheHandler(cacheName, cacheProperties);
            else
                cache = new MemoryCacheHandler(cacheName, cacheProperties);

            if (cacheItems!=null)
            {
                foreach (string key in cacheItems.Keys)
                    cache.Add(key, cacheItems[key]);
            }
            _cache[cacheName] = cache;           

            Thread sentinelThread = new Thread(cache.RunTimer);
            sentinelThread.Name = cacheName + "-Sentinel";
            sentinelThread.Start();

            return GetCache(cacheName);
        }
        public ICache CreateCache(string cacheName)
        {
            return _create(cacheName, null, null);
        }
        public ICache CreateCache(string cacheName, CacheProperties cacheProperties)
        {
            return _create(cacheName, cacheProperties, null);
        }
        public ICache CreateCache(string cacheName, CacheProperties cacheProperties, IDictionary<string, object> cacheItems)
        {
            return _create(cacheName, cacheProperties, cacheItems);
        }

        #endregion

        #region CRUD operations
        private bool TryAdd(ICache cacheHandler)
        {
            bool result = false;

            if (_cache.TryAdd(cacheHandler.Name, cacheHandler))//if TryAdd return false, that means key already exists
            {                
                Thread sentinelThread = new Thread(cacheHandler.RunTimer);
                sentinelThread.Name = cacheHandler.Name + "-Sentinel";
                sentinelThread.Start();
                result = true;
            }

            return result;
        }
        private ICache Update(ICache cacheHandler)
        {
            if (_cache.ContainsKey(cacheHandler.Name))
            {
                if (Remove(cacheHandler.Name))
                {
                    TryAdd(cacheHandler);                   
                    return _cache[cacheHandler.Name];
                }
            }
            return null;
        }
        public ICache GetCache(string cacheName)
        {
            ICache result = null;
            if (!_cache.TryGetValue(cacheName, out result))
            {              
                Console.WriteLine("Key:{0} does not exist in the Cache", cacheName);
            }
            return result;
        }
        public T GetCache<T>(string cacheName) where T : class
        {
            try
            {
                return GetCache(cacheName) as T;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public bool Remove(string cacheName)
        {
            ICache result = null;
            if (!_cache.TryRemove(cacheName, out result))
                Console.WriteLine("key:{0} didn't removed", cacheName);

            return result != null;
        }
        public bool ContainsKey(string cacheName)
        {
            return _cache.ContainsKey(cacheName);
        }
        #endregion

        #region Extended operations
        public ICache CreateOrGetCache(string cacheName)
        {
            //return existed cache, when cache is already exist
            if (_cache.ContainsKey(cacheName)) return GetCache(cacheName);

            return CreateCache(cacheName);
        }
        public ICache CreateOrGetCache(string cacheName, CacheProperties cacheProp)
        {
            //return existed cache, when cache is already exist
            if (_cache.ContainsKey(cacheName)) return GetCache(cacheName);

            return CreateCache(cacheName, cacheProp);
        }
        public ICache CreateOrGetCache(string cacheName, CacheProperties cacheProp, IDictionary<string, object> cacheItems)
        {
            //return existed cache, when cache is already exist
            if (_cache.ContainsKey(cacheName)) return GetCache(cacheName);

            return CreateCache(cacheName, cacheProp, cacheItems);
        }


        /*


        public ICache Add(string cacheName, int? expireDuration, int? controlPeriod, int? capacity, bool? isSlidingExpiration, Func<string, bool> expireFunction, CacheType? cacheType)
        {
            return CreateCache(cacheName, expireDuration, controlPeriod, capacity, isSlidingExpiration, expireFunction, cacheType);
        }
        public ICache Add(string cacheName, int expireDuration, int controlPeriod, int capacity, Func<string, bool> expireFunction, CacheType? cacheType)
        {
            return CreateCache(cacheName, expireDuration, controlPeriod, capacity, null, expireFunction, cacheType);
        }
        public ICache Add(string cacheName, int expireDuration, int capacity, CacheType? cacheType)
        {
            return CreateCache(cacheName, expireDuration, capacity, null, null, null, cacheType);
        }
        public ICache Add(string cacheName, int expireDuration, CacheType? cacheType)
        {
            return CreateCache(cacheName, expireDuration, null, null, null, null, cacheType);
        }
        public ICache Add(string cacheName, CacheType? cacheType)
        {
            return CreateCache(cacheName, null, null, null, null, null, cacheType);
        }
        public ICache Add(string cacheName)
        {
            return CreateCache(cacheName, null, null, null, null, null, CacheType.DictionaryCache);
        }

        /// <summary>
        /// Retrieve ICacheHandler included MemoryCacheHandler, because Default cacheType is MemoryCacheHandler
        /// </summary>
        /// <param name="cacheName"></param>
        /// <param name="cacheItems"></param>
        /// <returns></returns>
        public ICache Add(string cacheName, IDictionary cacheItems)
        {
            if (_cache.ContainsKey(cacheName))
            {
                return Get(cacheName);
            }

            MemoryCacheHandler mc = new MemoryCacheHandler(cacheName);

            foreach (var item in cacheItems.Keys)
            {
                mc.Put(item.ToString(), cacheItems[item]);
            }

            return mc;

        }
        public ICache Add(string cacheName, IDictionary cacheItems, CacheType cacheType)
        {
            if (_cache.ContainsKey(cacheName))
            {
                return Get(cacheName);
            }

            if (cacheType.Equals(CacheType.DictionaryCache))
            {
                DictionaryCacheHandler dcc = new DictionaryCacheHandler(cacheName);
                foreach (var item in cacheItems.Keys)
                {
                    dcc.Put(item.ToString(), cacheItems[item]);
                }
                return dcc;
            }
            else
            {
                return Create(cacheName, cacheItems);
            }
        }
  
        public void Set(string cacheName, IDictionary cacheItems)
        {
            MemoryCacheHandler mc = new MemoryCacheHandler(cacheName);

            foreach (var item in cacheItems.Keys)
            {
                mc.Put(item.ToString(), cacheItems[item]);
            }

            if (!Update(mc))
            {
                TryAdd(mc);
            }
        }

        public void ShutDownAllCaches()
        {
            foreach (ICache cac in _cache.Values)
            {
                if (cac != null) cac.StopTimer();
            }
        }
        public void ClearAllCaches()
        {
            foreach (ICache cac in _cache.Values)
            {
                if (cac != null) cac.Clear();
            }
        }

        */
        #endregion
    }
}


