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
        public void LoadConfiguration(FileInfo configFileInfo)
        {
            if (configFileInfo.IsNullOrEmpty()) throw new FileNotFoundException("configFileInfo does not exist!");
            if (configFileInfo.Exists) throw new FileNotFoundException("configFileInfo path does not exist!");

            NameValueCollection nvc = XmlUtil.ReadNameValueXml("configuration", configFileInfo.FullName);

            foreach (string key in nvc.Keys)
            {
                _configCache.TryAdd(key, nvc[key]);
            }
        }
        public void LoadConfiguration(NameValueCollection nameValueCollection)
        {
            NameValueCollection nv = nameValueCollection;
            foreach (string key in nv)
            {
                _configCache.TryAdd(key, nv[key]);
            }

        }
        public void LoadConfiguration(string sectionNameInAppConfigurationFile)
        {
            NameValueCollection nv = (NameValueCollection)ConfigurationManager.GetSection(sectionNameInAppConfigurationFile);

            foreach (string key in nv)
            {
                _configCache.TryAdd(key, nv[key]);
            }

        }
        #endregion

        #region Get generic config
        public T GetConfig<T>(string key, T defaultVal)
        {
            return _configCache.ContainsKey(key) ? _configCache[key].Convert<T>() : (T)defaultVal;
        }

        public object GetConfig(string key)
        {
            return _configCache.ContainsKey(key) ? _configCache[key] : null;
        }
        #endregion

        #region Get String Config
        public string GetStringConfig(string key)
        {
            return GetStringConfig(key, "");
        }
        public string GetStringConfig(string key, string defaultValue)
        {
            string result = defaultValue;
            if (_configCache.ContainsKey(key))
            {
                result = _configCache[key];
                if (result.Equals(string.Empty))
                    result = defaultValue;
            }
            return result;
        }
        #endregion

        #region Get Integer Config
        public int? GetIntConfig(string key)
        {
            return GetIntConfig(key, null);
        }

        private int? GetIntConfig(string key, int? defaultValue)
        {
            int? result = defaultValue;
            try
            {
                result = int.Parse(_configCache[key]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Parse Exception", ex);
            }
            return result;
        }

        #endregion

        #region Get Boolen Config
        public bool GetBooleanConfig(string masterJobmanager, bool b)
        {
            bool result = b;
            try
            {
                result = bool.Parse(_configCache[masterJobmanager]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Parse Exception", ex);
            }
            return result;
        }
        #endregion

        #region All Configurations
        public string APP_NAME { get { return GetStringConfig("app.name", "ROBOUTIL"); } }
        public string APP_DOMAIN { get { return GetStringConfig("app.domain", "RoboUtil"); } }
        public string MASTER_JOBMANAGER { get { return GetStringConfig("master.jobmanager","N/A"); } }


        #endregion
    }
}
