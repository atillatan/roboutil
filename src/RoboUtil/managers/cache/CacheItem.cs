using System;

namespace RoboUtil.managers.cache
{
    [Serializable()]
    public class CacheItem
    {
        #region properties

        private DateTime _lastUsedTime;
        public DateTime LastUsedTime
        {
            get { return _lastUsedTime; }
            set { _lastUsedTime = value; }
        }

        private DateTime _created;
        public DateTime Created
        {
            get { return _created; }
            set { _created = value; }
        }

        private DateTime _updated;
        public DateTime Updated
        {
            get { return _updated; }
            set { _updated = value; }
        }

        private string _key;
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        private object _val = null;
        public object Value
        {
            get { return _val; }
            set { _val = value; }
        }

        #endregion

        public CacheItem(string key, object obj)
        {
            _key = key;
            _val = obj;
            _lastUsedTime = DateTime.Now;
            _created = DateTime.Now;
            _updated = DateTime.Now;
        }
    }
}
