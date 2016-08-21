//using log4net.Repository.Hierarchy;
//using Quartz;
//using Quartz.Impl;
//using Quartz.Impl.Calendar;

namespace RoboUtil.managers
{
    public class JobManager
    {
        private bool isMasterScheduleJobManager = false;

        #region Singleton Implementation

        private JobManager()
        {
            //#if DEBUG
            //isMasterScheduleJobManager = true;//Atilla, production da değişecek false olacak;
            //#endif

            string hostName = ConfigManager.Current.GetConfig<string>("master.schedule.job.servername");

            if (System.Net.Dns.GetHostName().Equals(hostName, System.StringComparison.CurrentCultureIgnoreCase))
            {
                isMasterScheduleJobManager = true;
            }
        }

        public static JobManager Instance
        {
            get
            {
                return NestedSingleton.scheduleJobManager;
            }
        }

        private class NestedSingleton
        {
            internal static readonly JobManager scheduleJobManager = new JobManager();
            static NestedSingleton() { }
        }

        #endregion Singleton Implementation

        public void Run()
        {
            //Logger.Info("Quartz Scheduler initializing");
            if (!isMasterScheduleJobManager)
            {
                //Logger.Info("Quartz Scheduler Canceled, because isMasterScheduleJobManager=false");
                return;
            }

            //NameValueCollection properties = new NameValueCollection();
            //properties["quartz.scheduler.instanceName"] = "UsisScheduler";
            //properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            //properties["quartz.threadPool.threadCount"] = "10";
            //properties["quartz.threadPool.threadPriority"] = "Normal";
            //properties["quartz.jobStore.type"] = "Quartz.Simpl.RAMJobStore, Quartz";
            //properties["quartz.plugin.xml.type"] = "Quartz.Plugin.Xml.XMLSchedulingDataProcessorPlugin, Quartz";
            //properties["quartz.plugin.xml.fileNames"] = System.Web.HttpContext.Current != null ?
            //     Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scheduler.xml") :
            //     Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Scheduler.xml");
            //ISchedulerFactory sf = new StdSchedulerFactory(properties);
            //IScheduler sched = sf.GetScheduler();
            //var dailyCalendar = new DailyCalendar("00:01", "23:59");
            //dailyCalendar.InvertTimeRange = true;
            //sched.AddCalendar("cal1", dailyCalendar, false, false);
            ////Logger.Info("Quartz Scheduler Initialization Complete");
            //sched.Start();
            //Logger.Info("Quartz Scheduler Started");
            //Logger.Info("Quartz Scheduler Shutdown");
            //sched.Shutdown(true);
            //Logger.Info("Quartz Scheduler Shutdown Complete");
        }
    }
}
