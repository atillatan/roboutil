using RoboUtil.Common.Service;
using RoboUtil.managers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboUtil.Common.Service
{
    public class ApplicationContext
    {
        #region Singleton Implementation

        private static ApplicationContext _appContext = null;
        private static readonly object SyncRoot = new Object();

        private ApplicationContext()
        {
            Initialize();
        }

        public static ApplicationContext Current
        {
            get
            {
                if (_appContext == null)
                {
                    lock (SyncRoot)
                    {
                        if (_appContext == null)
                            _appContext = new ApplicationContext();
                    }
                }
                return _appContext;
            }
        }

        #endregion Singleton Implementation

        private ConcurrentDictionary<string, string> _items;
        public ConcurrentDictionary<string, string> Items { get { return _items; } }

        public static readonly ConcurrentDictionary<string, ServiceContext> ServiceContextCache = new ConcurrentDictionary<string, ServiceContext>();

        private void Initialize()
        {
            _items = new ConcurrentDictionary<string, string>();
        }

        #region Hardcoded Items

        public string DATABASE_WRITE_CONNECTIONSTRING { get; set; }
        public string DATABASE_READ_CONNECTIONSTRING { get; set; }
        public string DATABASE_PROVIDER_NAME { get; set; }
        public string ENVIRONMENT_NAME { get; set; }

        #endregion Hardcoded Items
    }
}