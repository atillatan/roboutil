using RoboUtil.utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;

//using System.Configuration;//converting netstandart1.6
using System.IO;
using System.Linq;
using System.Text;

namespace RoboUtil.managers
{
    /// <summary>
    /// this class will be monitor configFile info
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

        public static ConfigManager Current
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

        public static FileInfo ConfigFileInfo { get; }

        private ConcurrentDictionary<string, string> _configurations;
        public ConcurrentDictionary<string, string> Configurations { get { return _configurations; } }

        private void Initialize()
        {
            _configurations = new ConcurrentDictionary<string, string>();
        }

        #region Loading Configurations

        public void LoadConfiguration(NameValueCollection nameValueCollection)
        {
            NameValueCollection nv = nameValueCollection;

            foreach (string key in nv)
            {
                _configurations.TryAdd(key, nv[key]);
            }
        }

        public void Configure(FileInfo configFileInfo)
        {
            if (configFileInfo == null)
            {
                Console.WriteLine("configFileInfo does not exist!");
                System.Diagnostics.Debug.WriteLine("configFileInfo does not exist!");
                //return;
                throw new Exception("configFileInfo does not exist!");
            }
            if (!configFileInfo.Exists)
            {
                Console.WriteLine("configFileInfo path does not exist!");
                System.Diagnostics.Debug.WriteLine("configFileInfo path does not exist!");
                //return;
                throw new Exception("configFileInfo path does not exist!");
            }

            NameValueCollection nvc = Utils.XmlUtil.ReadNameValueXml("configuration", configFileInfo.FullName);
            LoadConfiguration(nvc);
        }

        //converting netstandart1.6
        //public void LoadConfiguration(string sectionNameInAppConfigurationFile)
        //{
        //    NameValueCollection nv = (NameValueCollection)ConfigurationManager.GetSection(sectionNameInAppConfigurationFile);
        //    LoadConfiguration(nv);
        //}

        #endregion Loading Configurations

        #region Get generic config

        public T GetConfig<T>(string key, T defaultVal)
        {
            if (_configurations.ContainsKey(key))
            {
                string result = _configurations[key];
                result = ReplaceVars(result);
                return result.ConvertTo<T>();
            }
            return defaultVal;
        }

        public T GetConfig<T>(string key) where T : class
        {
            if (_configurations.ContainsKey(key))
            {
                string result = _configurations[key];
                result = ReplaceVars(result);
                return result.ConvertTo<T>();
            }
            return null;
        }

        private string ReplaceVars(string str)
        {
            foreach (string key in _configurations.Keys)
            {
                string val = _configurations[key];
                if (str.Contains("${" + key + "}"))
                {
                    str = str.Replace("${" + key + "}", val);
                }
            }

            return str;
        }

        #endregion Get generic config

        #region All Configurations

        //public string APP_NAME { get { return GetConfig<string>("app.name", "ROBOUTIL"); } }
        //public string APP_DOMAIN { get { return GetConfig<string>("app.domain", "RoboUtil"); } }
        //public string MASTER_JOBMANAGER { get { return GetConfig<string>("master.jobmanager", "N/A"); } }

        #endregion All Configurations

        #region File reload monitor

        //TODO: atilla when configuration loading, it gets file modified date
        //every one minute one thread compare filemodification date, and decide reloading

        #endregion File reload monitor
    }
}