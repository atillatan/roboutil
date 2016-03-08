using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using RoboUtil.utils;


namespace RoboUtil.managers
{
    /// <summary>
    /// bu class configPath deki dosyayi monitor edecek ve degisiklik olursa direk reload edecek
    /// </summary>
    public class ConfigManager
    {
        #region Singleton Implementation

        private static ConfigManager _configManager = null;

        private static readonly object SyncRoot = new Object();

        private ConfigManager()
        {
            Initialize();
        }

        public static ConfigManager Instance
        {
            get
            {
                if (_configManager == null)
                {
                    lock (SyncRoot)
                    {
                        if (_configManager == null)
                            _configManager = new ConfigManager();
                    }
                }
                return _configManager;
            }
        }

        #endregion Singleton Implementation
        public static string ConfigurationPath { get; set; }
        private ConcurrentDictionary<string, string> _configCache;
        public ConcurrentDictionary<string, string> Configurations
        {
            get
            {
                return _configCache;
            }
        }
        private void Initialize()
        {
            _configCache = new ConcurrentDictionary<string, string>();
        }

        #region Loading Configurations 
        public void LoadConfiguration(NameValueCollection nameValueCollection)
        {
            NameValueCollection nv = nameValueCollection;

            foreach (string key in nv)
            {
                _configCache.TryAdd(key, nv[key]);
            }
        }
        public void LoadConfiguration(FileInfo configFileInfo)
        {
            if (configFileInfo.IsNullOrEmpty()) throw new FileNotFoundException("configFileInfo does not exist!");
            if (configFileInfo.Exists) throw new FileNotFoundException("configFileInfo path does not exist!");

            NameValueCollection nvc = XmlUtil.ReadNameValueXml("configuration", configFileInfo.FullName);
            LoadConfiguration(nvc);
        }
        public void LoadConfiguration(string sectionNameInAppConfigurationFile)
        {
            NameValueCollection nv = (NameValueCollection)ConfigurationManager.GetSection(sectionNameInAppConfigurationFile);
            LoadConfiguration(nv);

        }
        #endregion

        #region Get generic config
        public T GetConfig<T>(string key, T defaultVal)
        {
            return _configCache.ContainsKey(key) ? _configCache[key].Convert<T>() : defaultVal;
        }

        public T GetConfig<T>(string key) where T : class
        {
            if (_configCache.ContainsKey(key))
            {
                return _configCache[key].Convert<T>();
            }
            return null;
        }
        #endregion

        #region All Configurations
        public string APP_NAME { get { return GetConfig<string>("app.name", "ROBOUTIL"); } }
        public string APP_DOMAIN { get { return GetConfig<string>("app.domain", "RoboUtil"); } }
        public string MASTER_JOBMANAGER { get { return GetConfig<string>("master.jobmanager", "N/A"); } }
        #endregion

        #region File reload monitor
        //TODO: atilla when configuration loading, it gets file modified date
        //every one minute one thread compare filemodification date, and decide reloading
        #endregion
    }
}
