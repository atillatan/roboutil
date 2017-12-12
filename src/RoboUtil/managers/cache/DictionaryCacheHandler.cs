//using System.Runtime.Caching;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

//using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace RoboUtil.managers.cache
{
    /// <summary>
    /// System.Collections.Concurrent.ConcurrentDictionary is threadsafe. Multiple concurrent threads can read and write a ConcurrentDictionary instance.
    /// Internally thread-safety is automatically handled to ensure the cache is updated in a consistent manner.
    /// What this might be referring to is that data stored within the cache may itself not be threadsafe.
    /// For example if a List is placed in the cache, and two separate threads both get a reference to the cached List,
    /// the two threads will end up stepping on each other if they both attempt to update the list simultaneously.
    /// </summary>
    public class DictionaryCacheHandler : ICache
    {
        #region Properties

        private string _name;
        public string Name { get { return _name; } }
        public DateTime StartTimeForExpiration { get; set; }

        private static bool _timerCanceled;
        public long Count { get { return (long)_cacheItems.Count; } }

        private ConcurrentDictionary<string, CacheItem> _cacheItems;

        public T CacheItems<T>() where T : class
        {
            return _cacheItems as T;
        }

        public ConcurrentDictionary<string, CacheItem> DictionaryCache { get { return _cacheItems; } }
        public MemoryCache MemoryCache { get { throw new Exception("CacheCollectionType isn't a CacheCollectionType.MemoryCache"); } }
        private CacheProperties _cacheProperties { get; set; }
        public CacheProperties CacheProperties { get { return _cacheProperties; } }

        #endregion Properties

        #region Constructors

        private void Initialize(string cacheName, CacheProperties cacheProperties)
        {
            _cacheProperties = cacheProperties;

            if (_cacheProperties == null)
            {
                //Defaults
                _cacheProperties = new CacheProperties();
                _cacheProperties.CacheItemExpireDuration = int.MaxValue;
                _cacheProperties.ControlPeriod = 1000;
                _cacheProperties.IsSlidingExpiration = true;
                _cacheProperties.CacheExpireFunction = null;
                _cacheProperties.CacheExpireDuration = null;
                _cacheProperties.Capacity = null;
            }

            if (cacheName == null) throw new ArgumentNullException("cacheName does not null!");
            _name = cacheName;

            //Fix values
            if (_cacheProperties.CacheItemExpireDuration == null && _cacheProperties.ControlPeriod == null) { _cacheProperties.ControlPeriod = int.MaxValue; _cacheProperties.CacheItemExpireDuration = int.MaxValue; };
            if (_cacheProperties.CacheItemExpireDuration == null && _cacheProperties.ControlPeriod != null) _cacheProperties.ControlPeriod = int.MaxValue;
            if (_cacheProperties.CacheItemExpireDuration != null && _cacheProperties.ControlPeriod == null) _cacheProperties.ControlPeriod = 1000;

            if (_cacheProperties.CacheExpireDuration == null && _cacheProperties.ControlPeriod == null) { _cacheProperties.ControlPeriod = int.MaxValue; _cacheProperties.CacheExpireDuration = int.MaxValue; };
            if (_cacheProperties.CacheExpireDuration == null && _cacheProperties.CacheItemExpireDuration == null && _cacheProperties.ControlPeriod != null) _cacheProperties.ControlPeriod = int.MaxValue;
            if (_cacheProperties.CacheExpireDuration != null && (_cacheProperties.ControlPeriod == null || _cacheProperties.ControlPeriod == int.MaxValue)) _cacheProperties.ControlPeriod = 1000;

            _cacheProperties.IsSlidingExpiration = _cacheProperties.IsSlidingExpiration ?? true;

            if (_cacheProperties.Capacity != null && _cacheProperties.Capacity > 0) _cacheItems = new ConcurrentDictionary<string, CacheItem>(1, (int)_cacheProperties.Capacity);
            else _cacheItems = new ConcurrentDictionary<string, CacheItem>();

            StartTimeForExpiration = DateTime.Now;
            Console.WriteLine("DictionaryCacheHandler:{0}  initialized!", _name);
        }

        public DictionaryCacheHandler(string cacheName)
        {
            Initialize(cacheName, null);
        }

        public DictionaryCacheHandler(string cacheName, CacheProperties cacheProperties)
        {
            Initialize(cacheName, cacheProperties);
        }

        #endregion Constructors

        #region CRUD operations

        private void PutCacheItem(CacheItem cacheItem)
        {
            //if (cacheItem.Value.GetType().GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICollection<>)))
            //{
            //    throw new Exception("You cannot add any collection for preventing Thread lock");
            //}

            CacheItem oldCacheItem = null;
            CacheItem item = cacheItem;

            if (_cacheItems.TryGetValue(item.Key, out oldCacheItem))
            {
                item.Created = oldCacheItem.Created;
                //Console.WriteLine("DictionaryCacheHandler:{0} key:{1} UPDATE. Count:{2}", _name, elem.Key, _cacheItems.Count.ToString());
            }
            else
            {
                item.Created = DateTime.Now;
                //Console.WriteLine("DictionaryCacheHandler:{0} key:{1} ADD Count:{2}", _name, elem.Key, _cacheItems.Count.ToString());
            }

            //item.Value = GeneralUtil.ToJson(item.Value); atilla:it is disabled, for use deepcloning
            item.LastUsedTime = DateTime.Now;
            item.Updated = DateTime.Now;
            _cacheItems[item.Key] = item;
        }

        public void Add(string key, object value)
        {
            CacheItem cacheItem = new CacheItem(key, value);
            PutCacheItem(cacheItem);
        }

        /// <summary>
        /// Insert value into the cacheHandler using
        /// appropriate name/value pairs
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="value">Item to be cached</param>
        /// <param name="key">Name of item</param>
        public void Add<T>(string key, T value) where T : class
        {
            CacheItem cacheItem = new CacheItem(key, value);
            PutCacheItem(cacheItem);
        }

        public void AddAll(IDictionary<string, object> dictionary)
        {
            if (dictionary != null && dictionary.Count > 0)
            {
                foreach (var key in dictionary.Keys)
                {
                    CacheItem cacheItem = new CacheItem(key, dictionary[key]);
                    PutCacheItem(cacheItem);
                }
            }
        }

        /// <summary>
        /// Retrieve cached item with type of cached item
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Name of cached item</param>
        /// <returns>Cached item as type</returns>
        public T GetValue<T>(string key) where T : class
        {
            CacheItem cacheItem = null;
            if (_cacheItems.TryGetValue(key, out cacheItem))
            {
                Console.WriteLine("DictionaryCacheHandler.GetValue({0}.{1}), Total cache size:{2}", _name, key, _cacheItems.Count());
                if ((bool)_cacheProperties.IsSlidingExpiration) cacheItem.LastUsedTime = DateTime.Now;
                //return JsonConvert.DeserializeObject<T>(cacheItem.Value as string);
                //return GeneralUtil.Clone<T>(cacheItem.Value as T);
                return cacheItem.Value as T;
            }
            return null;
        }

        public IList<T> GetValues<T>() where T : class
        {
            IList<T> result = new List<T>();

            foreach (string key in _cacheItems.Keys)
            {
                CacheItem ci = _cacheItems[key];

                if (_cacheProperties.IsSlidingExpiration != null && (bool)_cacheProperties.IsSlidingExpiration)
                {
                    ci.LastUsedTime = DateTime.Now;
                }

                //T realObj = GeneralUtil.Clone<T>(ci.Value as T);
                //T realObj = JsonConvert.DeserializeObject<T>(ci.Value as string);
                result.Add(ci.Value as T);
            }

            return result;
        }

        public IList<T> GetValues<T>(IList<string> keys) where T : class
        {
            var res = from a in _cacheItems
                      where keys.Contains(a.Key)
                      select a.Value as CacheItem;

            IList<CacheItem> temp = res.ToList<CacheItem>();
            IList<T> result = new List<T>();

            foreach (CacheItem item in temp)
            {
                if (_cacheProperties.IsSlidingExpiration != null && (bool)_cacheProperties.IsSlidingExpiration)
                {
                    item.LastUsedTime = DateTime.Now;
                }

                //T realObj = JsonConvert.DeserializeObject<T>(item.Value as string);
                result.Add(item.Value as T);
            }
            return result;
        }

        public bool Remove(string key)
        {
            CacheItem val = null;
            return _cacheItems.TryRemove(key, out val);
        }

        public void RemoveKeys(List<string> list)
        {
            foreach (string key in list)
            {
                Remove(key);
            }
        }

        public bool ContainsKey(string key)
        {
            return _cacheItems.ContainsKey(key);
        }

        #endregion CRUD operations

        #region Extended operations

        public ICollection<string> Keys
        {
            get { return _cacheItems.Keys; }
        }

        public long Size
        {
            get
            {
                long cacheSize = 0;
                //foreach (var v in _cacheItems)
                //{
                //    using (Stream s = new MemoryStream())
                //    {
                //        if (v.Value is IQueryable)
                //        {
                //            long listSize = 0;

                //            foreach (var q in (IQueryable)v.Value)
                //            {
                //                BinaryFormatter bf = new BinaryFormatter();
                //                bf.Serialize(s, q);
                //                listSize += s.Length;
                //            }
                //            cacheSize += listSize;
                //            continue;
                //        }

                //        BinaryFormatter formatter = new BinaryFormatter();
                //        formatter.Serialize(s, v.Value);
                //        cacheSize += s.Length;
                //    }
                //}
                return cacheSize;
            }
        }

        public void Clear()
        {
            Console.WriteLine("Cache cleared: [" + _name + "] DictionaryCacheHandler Count:" + _cacheItems.Count + " expire duration:" + _cacheProperties.CacheExpireDuration + " minutes");
            StartTimeForExpiration = DateTime.Now;
            _cacheItems.Clear();
        }

        public void Dispose()
        {
            _cacheItems.Clear();
            if (_cacheItems != null && _cacheItems is IDisposable)
            {
                ((IDisposable)_cacheItems).Dispose();
            }
            _cacheItems = null;
            //GC.SuppressFinalize(this);
        }

        public void RunTimer()
        {
            try
            {
                ThreadState threadState = new ThreadState(1, _timerCanceled);
                Timer timer = new Timer(new TimerCallback(TimerTask), threadState, 2000, (int)_cacheProperties.ControlPeriod);
                threadState.TimerReference = timer;

                //keep alive main thread, with timer
                while (!_timerCanceled)
                {
                    System.Threading.Thread.Sleep(5000);//check Timer is canceled
                }
            }
            //catch (ThreadAbortException thEx)
            //{
            //    Console.WriteLine(thEx.Message, thEx);
            //}
            catch (Exception ex1)
            {
                Console.WriteLine(ex1.Message, ex1);
                throw;
            }
        }

        private void TimerTask(object stateObj)
        {
            Thread.CurrentThread.Name = "DictionaryCacheHandler TimerTaskThread";
            ThreadState threadState = (ThreadState)stateObj;
            System.Threading.Interlocked.Increment(ref threadState.IncrementValue);

            try
            {
                if (_cacheProperties.CacheExpireDuration != null && _cacheProperties.CacheExpireDuration > 0)
                {
                    Console.WriteLine("Checking expiration for cache [" + _name + "] DictionaryCacheHandler Count:" + _cacheItems.Count + " Cache expire duration:" + _cacheProperties.CacheExpireDuration + " minutes");
                    int remainingTime = (DateTime.Now - StartTimeForExpiration).Minutes;

                    if (remainingTime >= _cacheProperties.CacheExpireDuration)
                    {
                        this.Clear();
                        //if (_cacheProperties.CacheExpireFunction != null) _cacheProperties.CacheExpireFunction.Method.Invoke(null, new object[] { this });
                        return;
                    }
                }

                if (_cacheProperties.CacheItemExpireDuration == null || _cacheProperties.CacheItemExpireDuration == int.MaxValue)
                {
                    return;
                }

                Console.WriteLine("Checking expired objects in [{0}] DictionaryCacheHandler Count:{1} CacheExpireDuration:{2} minutes  CacheItemExpireDuration:{3} minutes", _name, Count, _cacheProperties.CacheExpireDuration, _cacheProperties.CacheItemExpireDuration);

                List<CacheItem> removeList;
                if ((bool)_cacheProperties.IsSlidingExpiration)
                {
                    removeList = (from CacheItem elem in _cacheItems.Values
                                  where (DateTime.Now - elem.LastUsedTime).Minutes > _cacheProperties.CacheItemExpireDuration
                                  select elem).ToList();
                }
                else
                {
                    removeList = (from CacheItem elem in _cacheItems.Values
                                  where (DateTime.Now - elem.Created).Minutes > _cacheProperties.CacheItemExpireDuration
                                  select elem).ToList();
                }

                if (removeList != null && removeList.Count > 0)
                {
                    foreach (CacheItem elem in removeList)
                    {
                        CacheItem val = elem;

                        if (_cacheItems.ContainsKey(elem.Key))
                        {
                            try
                            {
                                if (_cacheItems.TryRemove(elem.Key, out val))
                                    Console.WriteLine("Remove expired cache item [{0}].[{1}], Total cacheHandler size:{2}", _name, elem.Key, _cacheItems.Count.ToString());
                                else
                                    Console.WriteLine("ERROR! Can't remove expired [{0}].[{1}], Total cacheHandler size:{2}", _name, elem.Key, _cacheItems.Count.ToString());
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("ERROR! Can't remove expired [{0}].[{1}], Total cacheHandler size:{2}", _name, elem.Key, _cacheItems.Count.ToString());
                                Console.WriteLine(ex.StackTrace);
                            }
                        }

                        if (_cacheItems.TryRemove(elem.Key, out val))
                            Console.WriteLine("Remove expired cache item [{0}].[{1}], Total cacheHandler size:{2}", _name, elem.Key, _cacheItems.Count.ToString());
                        //else
                        //if (_cacheProperties.CacheItemExpireFunction != null) _cacheProperties.CacheItemExpireFunction.Method.Invoke(null, new object[] { elem });
                    }
                }

                if (threadState.TimerCanceled)
                {
                    threadState.TimerReference.Dispose();
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message, exc);
            }
        }

        public void StopTimer()
        {
            _timerCanceled = true;
        }

        #endregion Extended operations

        #region disabled

        //public T GetValue<T>(string key) where T : class
        //{
        //    try
        //    {
        //        return GetValue(key) as T;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}
        //public object this[string key]
        //{
        //    get { return GetValue(key); }
        //}

        #endregion disabled
    }
}