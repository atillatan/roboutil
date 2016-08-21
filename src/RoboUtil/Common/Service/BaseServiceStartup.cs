using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using log4net.Config;
using RoboUtil.managers;

namespace RoboUtil.Common.Service
{
    public class BaseServiceStartup
    {
        public  static void Start(FileInfo configFileInfo)
        {
            //ConfigManager.Current.Configure(configFileInfo);
            //ConfigureLogging(ConfigManager.Current.GetConfig<string>("log4net"));
            //ConfigureCronJobs(ConfigManager.Current.GetConfig<string>("Schedule"));
        }

        private static void ConfigureCronJobs(string filePath)
        {
           // JobManager.Instance.Run();
        }

        public static void ConfigureLogging(string filePath)
        {
            //XmlConfigurator.ConfigureAndWatch(new FileInfo("Configuration/Logging.config"));
        }



    }
}
