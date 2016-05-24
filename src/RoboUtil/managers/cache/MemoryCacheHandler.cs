using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Caching;
using RoboUtil.utils;
 

namespace RoboUtil.managers.cache
{
    /// <summary>
    /// System.Runtime.Caching.MemoryCache is threadsafe. Multiple concurrent threads can read and write a MemoryCache instance. 
    /// Internally thread-safety is automatically handled to ensure the cache is updated in a consistent manner.
    /// What this might be referring to is that data stored within the cache may itself not be threadsafe.
    /// For example if a List is placed in the cache, and two separate threads both get a reference to the cached List, 
    /// the two threads will end up stepping on each other if they both attempt to update the list simultaneously.
    /// </summary>
    public class MemoryCacheHandler : ICache
    {
        #region Properties
        private string _name;
        public string Name { get { return _name; } }
        public DateTime StartTimeForExpiration { get; set; }

        private static bool _timerCanceled;

        private System.Runtime.Caching.CacheItemPolicy _cacheItemPolicy = null;
        public long Count { get { return _cacheItems.GetCount(); } }

        private System.Runtime.Caching.MemoryCache _cacheItems;
        public ConcurrentDictionary<string, CacheItem> DictionaryCache { get { throw new Exception("CacheCollectionType isn't a CacheCollectionType.MemoryCache"); } }
        public System.Runtime.Caching.MemoryCache MemoryCache { get { return _cacheItems; } }
        public T CacheItems<T>() where T : class { return _cacheItems as T; }
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
                _cacheProperties.IsSlidingExpiration = true;
                _cacheProperties.CacheItemExpireDuration = null;
                _cacheProperties.ControlPeriod = null;
                _cacheProperties.CacheExpireFunction = null;
                _cacheProperties.CacheExpireDuration = null;
                _cacheProperties.Capacity = null;
            }

            if (cacheName == null) throw new ArgumentNullException("cacheName");
            _name = cacheName;

            //create cacheHandler item policy
            if (_cacheProperties.CacheItemExpireDuration != null)
            {
                _cacheItemPolicy = new System.Runtime.Caching.CacheItemPolicy();

                if (_cacheProperties.IsSlidingExpiration != null && _cacheProperties.IsSlidingExpiration == true)
                    _cacheItemPolicy.SlidingExpiration = new TimeSpan(0, 0, (int)_cacheProperties.CacheItemExpireDuration);
                else _cacheItemPolicy.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes((int)_cacheProperties.CacheItemExpireDuration);

                if (_cacheProperties.CacheItemExpireFunction != null) _cacheItemPolicy.RemovedCallback = new CacheEntryRemovedCallback(this.RemovedCallback);
            }

            if (_cacheProperties.CacheExpireDuration != null)
            {
                _cacheItemPolicy = new System.Runtime.Caching.CacheItemPolicy();
                _cacheItemPolicy.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes((int)_cacheProperties.CacheExpireDuration);
                if (_cacheProperties.CacheExpireFunction != null) _cacheItemPolicy.RemovedCallback = new CacheEntryRemovedCallback(this.RemovedCallback);
            }

            if (_cacheProperties.CacheItemExpireDuration == null && _cacheProperties.CacheExpireDuration == null)
            {
                _cacheItemPolicy = null;
            }

            if (_cacheProperties.Capacity != null && _cacheProperties.Capacity > 0)
            {
                //Atilla:Referans https://msdn.microsoft.com/en-us/library/dd941874(v=vs.110).aspx
                var cacheConfig = new NameValueCollection();
                cacheConfig.Add("pollingInterval", "00:05:00"); //5 minutes
                cacheConfig.Add("physicalMemoryLimitPercentage", "0");
                cacheConfig.Add("cacheMemoryLimitMegabytes", _cacheProperties.Capacity.ToString()); //500MB limit
                _cacheItems = new System.Runtime.Caching.MemoryCache(_name, cacheConfig);
            }
            else
            {
                _cacheItems = new System.Runtime.Caching.MemoryCache(_name);
            }

            StartTimeForExpiration = DateTime.Now;
            Console.WriteLine("MemoryCacheHandler:{0}  initialized!", _name);
        }
        public MemoryCacheHandler(string cacheName)
        {
            Initialize(cacheName, null);
        }
        public MemoryCacheHandler(string cacheName, CacheProperties cacheProperties)
        {
            Initialize(cacheName, cacheProperties);
        }

        #endregion Constructors

        #region CRUD operations
        private void PutCacheItem(CacheItem cacheItem)
        {
            if (GeneralUtil.IsCollection(cacheItem.Value))
            {
                throw new Exception("You cannot add any collection for preventing Thread lock");
            }

            CacheItem _mycacheItem = cacheItem;

            if (this._cacheItems.Contains(_mycacheItem.Key))
            {
                this._cacheItems.Set(_mycacheItem.Key, GeneralUtil.ToJson(_mycacheItem.Value), _cacheItemPolicy);//update
                //Console.WriteLine("MemoryCacheHandler:{0}.{1} icindeki element guncellendi! Total cacheHandler Count:{2}", _name, _mycacheItem.Key, this._cacheItems.Count().ToString());
            }
            else
            {
                this._cacheItems.Add(new System.Runtime.Caching.CacheItem(_mycacheItem.Key, GeneralUtil.ToJson(_mycacheItem.Value)), _cacheItemPolicy);//add
                //Console.WriteLine("MemoryCacheHandler:{0}.{1} icine element eklenemedi! Total cacheHandler Count:{2}", _name, _mycacheItem.Key, this._cacheItems.Count().ToString());
            }
        }
        public void Add(string key, object value)
        {
            CacheItem ci = new CacheItem(key, value);
            PutCacheItem(ci);
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
        /// Retrieve cached item
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Name of cached item</param>
        /// <returns>Cached item as type</returns>
        public T GetValue<T>(string key) where T : class
        {
            try
            {
                Console.WriteLine("MemoryCacheHandler.GetValue({0}.{1}), Total cache size:{2}", _name, key, _cacheItems.Count());
                return GeneralUtil.FromJson<T>(_cacheItems.Get(key) as string);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public IList<T> GetValues<T>() where T : class
        {
            IList<T> resultList = new List<T>();

            //var res = from a in _cacheItems                     
            //          select a.Value; ;

            //IList<object> templist = res.ToList<object>();
            //foreach (string ci in templist)
            //{
            //    resultList.Add(GeneralUtil.FromJson<T>(ci));
            //}

            _cacheItems.ForEach(a => resultList.Add(GeneralUtil.FromJson<T>(a.Value as string)));

            return resultList;
        }
        public IList<T> GetValues<T>(IList<string> keys)
        {
            var res = from a in _cacheItems
                      where keys.Contains(a.Key)
                      select a.Value; ;

            IList<object> templist = res.ToList<object>();
            IList<T> resultList = new List<T>();

            foreach (string ci in templist)
            {
                resultList.Add(GeneralUtil.FromJson<T>(ci));
            }

            return resultList;
        }

        public bool Remove(string key)
        {
            //Console.WriteLine("MemoryCacheHandler:{0}  item removed:{1}", _name, key);
            return _cacheItems.Remove(key) != null;
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
            return _cacheItems.Contains(key);
        }
        #endregion

        #region Extended operations
        public long Size
        {
            get
            {
                long cacheSize = 0;
                foreach (var v in _cacheItems)
                {
                    using (Stream s = new MemoryStream())
                    {

                        if (v.Value is IQueryable)
                        {
                            long listSize = 0;

                            foreach (var q in (IQueryable)v.Value)
                            {
                                BinaryFormatter bf = new BinaryFormatter();
                                bf.Serialize(s, q);
                                listSize += s.Length;
                            }
                            cacheSize += listSize;
                            continue;
                        }

                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(s, v.Value);
                        cacheSize += s.Length;
                    }
                }
                return cacheSize;
            }
        }
        public void Clear()
        {
            Console.WriteLine("Cache clearing: [" + _name + "] MemoryCacheHandler Count:" + _cacheItems.GetCount() + " expire duration:" + _cacheProperties.CacheExpireDuration + " minutes");

            foreach (string key in Keys)
            {
                Remove(key);
            }

            Initialize(_name, _cacheProperties);
        }
        public void Dispose()
        {
            if (_cacheItems != null && _cacheItems is IDisposable)
            {
                ((IDisposable)_cacheItems).Dispose();
            }
            _cacheItems = null;
            //GC.SuppressFinalize(this);
        }
        public void StopTimer()
        {
            _timerCanceled = true;
        }
        public void RunTimer()
        {
            try
            {
                int controlPeriod = (_cacheProperties != null && _cacheProperties.ControlPeriod != null) ? (int)_cacheProperties.ControlPeriod : 1000;
                ThreadState threadState = new ThreadState(1, _timerCanceled);
                Timer timer = new Timer(new TimerCallback(TimerTask), threadState, 2000, controlPeriod);
                threadState.TimerReference = timer;

                //keep alive main thread, with timer
                while (!_timerCanceled)
                {
                    System.Threading.Thread.Sleep(5000);//check Timer is canceled
                }
            }
            catch (ThreadAbortException thEx)
            {
                Console.WriteLine(thEx.Message, thEx);
            }
            catch (Exception ex1)
            {
                Console.WriteLine(ex1.Message, ex1);
                throw;
            }
        }
        private void TimerTask(object stateObj)
        {
            Thread.CurrentThread.Name = "MemoryCacheHandler TimerTaskThread";
            ThreadState threadState = (ThreadState)stateObj;
            System.Threading.Interlocked.Increment(ref threadState.IncrementValue);

            if (_cacheProperties.CacheExpireDuration == null && _cacheProperties.CacheItemExpireDuration == null)
            {
                return;
            }
            Console.WriteLine("[{0}] MemoryCacheHandler item Count:{1} CacheExpireDuration:{2} minutes CacheItemExpireDuration:{3} minutes", _name, Count, _cacheProperties.CacheExpireDuration, _cacheProperties.CacheItemExpireDuration);
        }
        public ICollection<string> Keys
        {
            get { return _cacheItems.DefaultIfEmpty().Where(a => a.Key != null).Select(x => x.Key).ToList(); }
        }
        public void RemovedCallback(CacheEntryRemovedArguments arguments)
        {
            if (arguments == null) return;

            //for all cache item removed
            if (_cacheProperties.CacheExpireDuration != null && _cacheProperties.CacheExpireDuration > 0)
            {
                if ((DateTime.Now - StartTimeForExpiration).Minutes >= _cacheProperties.CacheExpireDuration)
                {
                    if (_cacheItems.GetCount() == 0)
                    {
                        Initialize(_name, _cacheProperties);
                        Console.WriteLine("Before CacheExpireFunction cache [" + _name + "] MemoryCacheHandler Cache  Count:" + _cacheItems.GetCount() + " expire duration:" + _cacheProperties.CacheExpireDuration + " minutes");
                        if (_cacheProperties.CacheExpireFunction != null) _cacheProperties.CacheExpireFunction.Method.Invoke(null, new object[] { this });
                        Console.WriteLine("After CacheExpireFunction cache [" + _name + "] MemoryCacheHandler Cache  Count:" + _cacheItems.GetCount() + " expire duration:" + _cacheProperties.CacheExpireDuration + " minutes");
                        return;
                    }
                }
            }

            //for one CacheItem removed
            if (_cacheProperties.CacheItemExpireFunction != null)
            {
                Console.WriteLine("Remove expired cache item [{0}].[{1}], Total cache size:{2}", _name, arguments.CacheItem.Key, _cacheItems.Count());
                CacheItem ci = new CacheItem(arguments.CacheItem.Key, arguments.CacheItem.Value);
                _cacheProperties.CacheItemExpireFunction.Method.Invoke(null, new object[] { ci });
            }
        }
        #endregion

        #region disabled
        //public object this[string key]
        //{
        //    get { return GetValue(key); }
        //}
        //public object GetValue(string key)
        //{
        //    Console.WriteLine("MemoryCacheHandler.GetValue({0}.{1}), Total cache size:{2}", _name, key, _cacheItems.Count());
        //    return GeneralUtil.FromJson(_cacheItems.Get(key) as string);
        //}
        #endregion
    }
}