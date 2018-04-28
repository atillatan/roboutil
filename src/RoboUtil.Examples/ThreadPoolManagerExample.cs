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

        public static void ExampleThreadPool()
        {

            //1- Create Pool       
            ThreadPoolHandler tpHandler = ThreadPoolManager.Instance.CreatePool("testpool1", 20, PingLocalNetwork);

            //2- Add tasks
            for (int i = 0; i < 255; i++)
                tpHandler.addJob("192.168.0." + i);

            //3-Start all thrads, belirtilen kadar Thread canlandirilir hepsi ayni methodu calistirir ve is kuyrugu tuketilir.
            //Not: WaitCallBack olarak belirlenen method isterse kuyruga is te ekleyebilir.
            var stopwatch = new Stopwatch();
            
            stopwatch.Start();

            tpHandler.Start();

            tpHandler.WaitAll();

            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
            //...


        }
        public static void Example2()
        {

            //1- Create Pool
            ThreadPoolManager.Instance.CreatePool("testpool1", 10, new WaitCallback(targetMethod));
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
                     System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
                     System.Net.NetworkInformation.PingReply rep = p.Send(job);

                     if (rep.Status == System.Net.NetworkInformation.IPStatus.Success)
                     {
                         Console.WriteLine($"{job}: Success");
                     }
                     else
                     {
                         Console.WriteLine($"{job}: Fail");
                     }
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
                    System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
                    System.Net.NetworkInformation.PingReply rep = p.Send(dest);

                    if (rep.Status == System.Net.NetworkInformation.IPStatus.Success)
                    {
                        Console.WriteLine($"{dest}: Success");
                    }
                    else
                    {
                        Console.WriteLine($"{dest}: Fail");
                    }
                });
            }

            Task.WaitAll(tasks);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
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

            Console.WriteLine($"{jobData.PoolName}-{jobData.ThreadInfo.ThreadName}, job:{jobData.Job.ToString()}: Started");

            System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
            System.Net.NetworkInformation.PingReply rep = p.Send((string)jobData.Job);

            if (rep.Status == System.Net.NetworkInformation.IPStatus.Success)
            {
                Console.WriteLine($"{jobData.PoolName}-{jobData.ThreadInfo.ThreadName}, job:{jobData.Job.ToString()}: Success");
            }
            else
            {
                Console.WriteLine($"{jobData.PoolName}-{jobData.ThreadInfo.ThreadName}, job:{jobData.Job.ToString()}: Fail");
            }

            //Console.WriteLine("{0}-{1}, job:{2}", jobData.PoolName, jobData.ThreadInfo.ThreadName, jobData.Job.ToString());
            //Thread.Sleep(10);//for tracing console, what happens
        }
    }
}
