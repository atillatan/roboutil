using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
//using System.Runtime.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace RoboUtil.managers.cache
{
    public interface ICache : IDisposable
    {
        string Name { get; }
        CacheProperties CacheProperties { get; }
        DateTime StartTimeForExpiration { get; set; }
        long Count { get; }
        long Size { get; }
        ConcurrentDictionary<string, CacheItem> DictionaryCache { get; }
        MemoryCache MemoryCache { get; }

        //Add
        void Add(string key, object value);
        void Add<T>(string key, T value) where T : class;
        void AddAll(IDictionary<string, object> dictionary);


        //Get       
        T GetValue<T>(string key) where T : class;
        IList<T> GetValues<T>() where T : class;
        IList<T> GetValues<T>(IList<string> keys);
        
        //Remove
        bool Remove(string key);
        void RemoveKeys(List<string> list);
        bool ContainsKey(string key);        
        ICollection<string> Keys { get; }        
        T CacheItems<T>() where T : class;
        void Clear();
        void StopTimer();
        void RunTimer();

    }
}