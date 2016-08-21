using System;

namespace RoboUtil.managers.cache
{
    public class CacheProperties
    {
        /// <summary>
        /// Expire duration for every CacheItem
        /// </summary>
        public int? CacheItemExpireDuration { get; set; }
        /// <summary>
        /// Period for checking item expiration
        /// ControlPeriod type:milisecond
        /// </summary>
        public int? ControlPeriod { get; set; }
        /// <summary>
        /// MemoryCache Capacity(MB) parameter: 100MB
        /// DictionaryCache Capacity(maxitem) parameter: maxitem in cache
        /// </summary>
        public int? Capacity { get; set; }
        /// <summary>
        /// default value is true
        /// used only CacheItemExpiration
        /// cacheitem expiration reset, every lastusedtime
        /// </summary>
        public bool? IsSlidingExpiration { get; set; }
        public Func<CacheItem, bool> CacheItemExpireFunction { get; set; }
        public CacheCollectionType? CacheCollectionType { get; set; }
        public int? CacheExpireDuration { get; set; }
        public Func<ICache, bool> CacheExpireFunction { get; set; }

        public CacheProperties(CacheCollectionType cacheCollectionType)
        {
            CacheCollectionType = cacheCollectionType;
        }

        public CacheProperties(bool? isSlidingExpiration = null, int? cacheItemExpireDuration = null)
        {
            IsSlidingExpiration = isSlidingExpiration;
            CacheItemExpireDuration = cacheItemExpireDuration;
        }
    }
}
namespace RoboUtil.managers.cache
{
    public enum CacheCollectionType
    {
        DictionaryCache = 2,
        MemoryCache = 1
    };
}