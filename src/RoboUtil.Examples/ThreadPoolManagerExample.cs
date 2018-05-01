using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RoboUtil.managers;
using RoboUtil.managers.thread;

namespace RoboUtil.Examples
{
    public class ThreadPoolManagerExample
    {

        public static void ExampleSimple1()
        {
            List<object> jobs = new List<object>();
            for (int i = 0; i < 255; i++)
                jobs.Add("192.168.0." + i);

            ThreadPoolManager.Instance.StartPool(new ThreadPoolOptions
            {
                Jobs = jobs,
                PoolSize = 20,
                TargetMethod = PingLocalNetwork
            }).WaitOne();

            Console.WriteLine("completed");

        }

        public static void ExampleSimple2()
        {
            List<object> jobs = new List<object>();
            for (int i = 0; i < 255; i++)
                jobs.Add("192.168.0." + i);

            ThreadPoolHandler tpHandler = ThreadPoolManager.Instance.StartPool(new ThreadPoolOptions
            {
                Jobs = jobs,
                PoolSize = 20,
                TargetMethod = (jobData) =>
                {
                    JobData jd = (JobData)jobData;
                    Console.WriteLine($"PoolName:{jd.PoolName}, ThreadName:{jd.ThreadInfo.ThreadName}, Job:{jd.Job.ToString()}");
                    Thread.Sleep(1000);
                }
            })
            .WaitOne();

            Console.WriteLine("completed");
        }

        public static void ExampleSimple3()
        {
            List<object> jobs = new List<object>();
            for (int i = 0; i < 255; i++)
                jobs.Add("192.168.0." + i);

            ThreadPoolHandler tpHandler = ThreadPoolManager.Instance.StartPool(new ThreadPoolOptions
            {
                Jobs = jobs,
                PoolSize = 20,
                TargetMethod = (jobData) =>
                {
                    JobData _jobData = (JobData)jobData;
                    System.Net.NetworkInformation.PingReply rep = new System.Net.NetworkInformation.Ping().Send((string)_jobData.Job);
                    if (rep.Status == System.Net.NetworkInformation.IPStatus.Success) Console.WriteLine($"{_jobData.PoolName}-{_jobData.ThreadInfo.ThreadName}, job:{_jobData.Job.ToString()}: Success");
                    else Console.WriteLine($"{_jobData.PoolName}-{_jobData.ThreadInfo.ThreadName}, job:{_jobData.Job.ToString()}: Fail");

                }
            })
            .WaitOne();

            Console.WriteLine("completed");
        }

        public static void ExampleThreadPool()
        {

            //1- Create Pool       
            ThreadPoolHandler tpHandler = ThreadPoolManager.Instance.CreatePool(new ThreadPoolOptions
            {
                TargetMethod = PingLocalNetwork,
                PoolName = "testpool1",
                PoolSize = 20,
                ExitOnFinish = false
            });

            //2- Add tasks
            for (int i = 0; i < 255; i++)
                tpHandler.addJob("192.168.0." + i);

            //3-Start all thrads, belirtilen kadar Thread canlandirilir hepsi ayni methodu calistirir ve is kuyrugu tuketilir.
            //Not: WaitCallBack olarak belirlenen method isterse kuyruga is te ekleyebilir.
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            tpHandler.Start();
            tpHandler.WaitOne();

            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);

            // while (true)
            // {
            //     Console.WriteLine("sleeping 1 sn");
            //     Thread.Sleep(1000);
            // }
            //...

        }
        public static void Example2()
        {

            //1- Create Pool
            ThreadPoolManager.Instance.CreatePool(new ThreadPoolOptions
            {
                TargetMethod = targetMethod,
                PoolName = "testpool1",
                PoolSize = 10,
                ExitOnFinish = true
            });
            //2- Add tasks
            for (int i = 0; i < 10000; i++)
                ThreadPoolManager.Instance.Pool["testpool1"].JobQueue.Enqueue(new JobData() { Job = "http://page=" + i, PoolName = "testpool1" });
            //3-Start all thrads, belirtilen kadar Thread canlandirilir hepsi ayni methodu calistirir ve is kuyrugu tuketilir.
            //Not: WaitCallBack olarak belirlenen method isterse kuyruga is te ekleyebilir.
            ThreadPoolManager.Instance.Pool["testpool1"].Start();
        }

        public static void ExampleParallelForeach()
        {
            var jobs = new List<string>();

            for (int i = 0; i < 255; i++)
                jobs.Add("192.168.0." + i);


            var stopwatch = new Stopwatch();
            stopwatch.Start();

            ParallelOptions po = new ParallelOptions();
            po.MaxDegreeOfParallelism = 20;

            Parallel.ForEach(jobs, po, job =>
                 {
                     System.Net.NetworkInformation.PingReply rep = new System.Net.NetworkInformation.Ping().Send(job);
                     if (rep.Status == System.Net.NetworkInformation.IPStatus.Success) Console.WriteLine($"{job}: Success");
                     else Console.WriteLine($"{job}: Fail");
                 }
            );

            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
        }



        public static void ExampleTPL()
        {
            var jobs = new List<string>();

            for (int i = 0; i < 255; i++)
                jobs.Add("192.168.0." + i);


            var stopwatch = new Stopwatch();
            stopwatch.Start();


            var tasks = new Task[jobs.Count];
            for (var i = 0; i < jobs.Count; i++)
            {
                var dest = jobs[i]; /* work-around modified closures */
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    System.Net.NetworkInformation.PingReply rep = new System.Net.NetworkInformation.Ping().Send(dest);
                    if (rep.Status == System.Net.NetworkInformation.IPStatus.Success) Console.WriteLine($"{dest}: Success");
                    else Console.WriteLine($"{dest}: Fail");
                });
            }

            Task.WaitAll(tasks);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
        }

        public static void coreExample()
        {
            int toProcess = 10;
            using (ManualResetEvent resetEvent = new ManualResetEvent(false))
            {
                var list = new List<string>();
                for (int i = 0; i < 10; i++) list.Add("192.168.0."+i);

                for (int i = 0; i < 10; i++)
                {
                    ThreadPool.QueueUserWorkItem(
                       new WaitCallback(x =>
                       {
                           System.Net.NetworkInformation.PingReply rep = new System.Net.NetworkInformation.Ping().Send(x);
                           if (rep.Status == System.Net.NetworkInformation.IPStatus.Success) Console.WriteLine($"{x}: Success");
                           else Console.WriteLine($"{x}: Fail");
                           // Safely decrement the counter
                           if (Interlocked.Decrement(ref toProcess) == 0)
                               resetEvent.Set();

                       }), list);
                }

                resetEvent.WaitOne();
            }
            // When the code reaches here, the 10 threads will be done
            Console.WriteLine("Done");


        }
        private static void targetMethod(object obj)
        {
            JobData jobData = (JobData)obj;//we receive JobData from each thread
            Console.WriteLine("Started: {0}-{1}, job:{2}", jobData.PoolName, jobData.ThreadInfo.ThreadName, jobData.Job.ToString());
            //Thread.Sleep(1000*60*3);//for tracing console, what happens
        }



        private static void PingLocalNetwork(object obj)
        {
            JobData jobData = (JobData)obj;//we receive JobData from each thread
            string ip = (string)jobData.Job;

            Console.WriteLine($"{jobData.PoolName}-{jobData.ThreadInfo.ThreadName}, job:{jobData.Job.ToString()}: Started");

            System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
            System.Net.NetworkInformation.PingReply rep = p.Send(ip);

            if (rep.Status == System.Net.NetworkInformation.IPStatus.Success)
                Console.WriteLine($"{jobData.PoolName}-{jobData.ThreadInfo.ThreadName}, job:{jobData.Job.ToString()}: Success");
            else
                Console.WriteLine($"{jobData.PoolName}-{jobData.ThreadInfo.ThreadName}, job:{jobData.Job.ToString()}: Fail");
        }
    }
}
