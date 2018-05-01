using System;

using RoboUtil.managers;
using RoboUtil.managers.thread;
using System.Threading;
using Xunit;


namespace RoboUtil.Tests
{
    public class ThreadPoolManagerTest
    {

        [Fact]
        public void Example1Test()
        {

            //1- Create Pool
            ThreadPoolHandler tpHandler = ThreadPoolManager.Instance.CreatePool(new ThreadPoolOptions
            {
                TargetMethod = targetMethod,
                PoolName = "testpool1",
                PoolSize = 10,
                ExitOnFinish = true
            });

            //2- Add tasks
            for (int i = 0; i < 10000; i++)
                tpHandler.addJob("http://page=" + i);

            //3-Start all thrads, belirtilen kadar Thread canlandirilir hepsi ayni methodu calistirir ve is kuyrugu tuketilir.
            //Not: WaitCallBack olarak belirlenen method isterse kuyruga is te ekleyebilir.
            System.Diagnostics.Debug.WriteLine("sdfsdfsef");
            tpHandler.Start();
            System.Diagnostics.Debug.WriteLine("sdfsdfsef");

        }
        [Fact]
        public void Example2Test()
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

        private void targetMethod(object obj)
        {
            JobData jobData = (JobData)obj;//we receive JobData from each thread
            Console.WriteLine("Poolname:{0}, Thread Number:{1}, job:{2}", jobData.PoolName, jobData.ThreadInfo.ThreadNumber, jobData.Job.ToString());
            Thread.Sleep(100);//for tracing console, what happens
        }
    }
}
