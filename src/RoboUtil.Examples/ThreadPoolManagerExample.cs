using System;
using System.Collections.Generic;
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

        public static void Example1()
        {

            //1- Create Pool
            ThreadPoolHandler tpHandler= ThreadPoolManager.Instance.CreatePool("testpool1", 10, targetMethod);

            //2- Add tasks
            for (int i = 0; i < 10000; i++)
                tpHandler.addJob("http://page=" + i);

            //3-Start all thrads, belirtilen kadar Thread canlandirilir hepsi ayni methodu calistirir ve is kuyrugu tuketilir.
            //Not: WaitCallBack olarak belirlenen method isterse kuyruga is te ekleyebilir.
            tpHandler.Start();
           
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

        private static  void targetMethod(object obj)
        {
            JobData jobData = (JobData)obj;//we receive JobData from each thread
            Console.WriteLine("Poolname:{0}, Thread Number:{1}, job:{2}", jobData.PoolName, jobData.ThreadInfo.ThreadNumber, jobData.Job.ToString());
            Thread.Sleep(100);//for tracing console, what happens
        }
    }
}
