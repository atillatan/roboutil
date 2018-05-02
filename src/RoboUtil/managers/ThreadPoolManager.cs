using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using RoboUtil.managers.thread;


namespace RoboUtil.managers
{
    public class ThreadPoolManager
    {
        #region Singleton Implementation

        private static ThreadPoolManager _threadPoolManager = null;

        private static readonly object SyncRoot = new Object();

        private ThreadPoolManager()
        {
            Initialize();
        }

        public static ThreadPoolManager Instance
        {
            get
            {
                if (_threadPoolManager == null)
                {
                    lock (SyncRoot)
                    {
                        if (_threadPoolManager == null)
                            _threadPoolManager = new ThreadPoolManager();
                    }
                }
                return _threadPoolManager;
            }
        }

        #endregion Singleton Implementation

        private void Initialize()
        {
            _pool = new ConcurrentDictionary<string, ThreadPoolHandler>();
        }

        private static ConcurrentDictionary<string, ThreadPoolHandler> _pool = null;

        public ConcurrentDictionary<string, ThreadPoolHandler> Pool
        {
            get { return _pool; }
        }

        /// <summary>
        /// ThreadPoolManager makes easy to use ThreadPools
        /// </summary>
        /// <param name="poolName">define pool for same jobs</param>
        /// <param name="poolSize">define how many Thread will run in pool</param>
        /// <param name="callbackMethod">which method will run from every Thread</param>
        /// <returns></returns>
        public ThreadPoolHandler CreatePool(ThreadPoolOptions threadPoolOptions)
        {
            ThreadPoolHandler handler = new ThreadPoolHandler(threadPoolOptions);
            ThreadPoolHandler result = null;

            if (!_pool.ContainsKey(handler.PoolName))
            {
                if (_pool.TryAdd(handler.PoolName, handler))
                {
                    result = handler;
                }
            }

            return result;
        }

        // public ThreadPoolHandler StartPool(List<object> jobs, int poolSize, Action<object> jobAction)
        // {
        //     ThreadPoolOptions threadPoolOptions = new ThreadPoolOptions() { TargetMethod = new WaitCallback(jobAction), Jobs = jobs, PoolSize = poolSize };
        //     return this.StartPool(threadPoolOptions);
        // // }
        // public ThreadPoolHandler StartPool(List<object> jobs, int poolSize, WaitCallback waitCallback)
        // {
        //     ThreadPoolOptions threadPoolOptions = new ThreadPoolOptions() { TargetMethod = waitCallback, Jobs = jobs, PoolSize = poolSize };
        //     return this.StartPool(threadPoolOptions);
        // }

        public ThreadPoolHandler StartPool(ThreadPoolOptions threadPoolOptions)
        {
            ThreadPoolHandler threadPoolHandler = this.CreatePool(threadPoolOptions);

            foreach (var job in threadPoolOptions.Jobs)
            {
                threadPoolHandler.addJob(job);
            }

            threadPoolHandler.Start();
            return threadPoolHandler;
        }

    }

    public class ThreadPoolHandler
    {
        #region Properties

        private Queue _jobQueue;

        public Queue JobQueue
        {
            get { return _jobQueue; }
        }

        private string _poolName;

        public string PoolName
        {
            get { return _poolName; }
        }

        private int _poolSize;

        public int PoolSize
        {
            get { return _poolSize; }
        }

        private WaitCallback _waitCallback;

        public WaitCallback WaitCallback
        {
            get { return _waitCallback; }
        }

        private bool _exitOnFinish;

        public bool ExitOnFinish
        {
            get { return _exitOnFinish; }
        }
        public ManualResetEvent manualEvent { get; set; }

        public static int poolNumber = 0;

        public static int busyThreadCount = 0;

        #endregion Properties
        public ThreadPoolHandler(ThreadPoolOptions threadPoolOptions)
        {
            if (threadPoolOptions == null)
            {
                threadPoolOptions = new ThreadPoolOptions() { };
            }
            Interlocked.Increment(ref poolNumber);
            string poolName = "ThreadPool-" + poolNumber;

            _waitCallback = threadPoolOptions.TargetMethod;
            _jobQueue = Queue.Synchronized(new Queue());
            _poolName = threadPoolOptions.PoolName ?? poolName;
            _poolSize = threadPoolOptions.PoolSize ?? throw new ArgumentException("PoolSize is required parameter");
            _exitOnFinish = threadPoolOptions.ExitOnFinish ?? true;
        }

        /// <summary>
        /// it starts all threads in your ThreadPool
        /// </summary>
        public void Start()
        {
            manualEvent = new ManualResetEvent(false);

            for (int i = 0; i < _poolSize; i++)
            {
                ThreadInfo threadInfo = new ThreadInfo(_exitOnFinish);
                ThreadPool.QueueUserWorkItem(JobConsumer, threadInfo);
            }
        }

        private void JobConsumer(object threadInfo)
        {
            ThreadInfo _threadInfo = threadInfo as ThreadInfo;

            while (true)
            {
                JobData job = null;

                try
                {
                    job = (_jobQueue.Count > 0 ? _jobQueue.Dequeue() : null) as JobData;
                }
                catch (Exception e)
                { Console.WriteLine("Queue is empty!" + e.Message); }

                if (job != null)
                {
                    job.ThreadInfo = _threadInfo;
                    job.PoolName = _poolName;
                    try
                    {
                        _threadInfo.IsBusy = true;
                        Interlocked.Increment(ref busyThreadCount);
                        _waitCallback(job);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
                        Console.WriteLine("Continue to next job, Thread Number:{1}");
                    }
                    finally
                    {
                        Interlocked.Decrement(ref busyThreadCount);
                        _threadInfo.IsBusy = false;
                    }
                }
                else
                {
                    if (_jobQueue.Count == 0)
                    {
                        if (_threadInfo.ExitOnFinish == true)
                        {
                            Console.WriteLine("Thread pool:{0} Thread Number:{1} Queue is empty!, ExitOnFinish:true, waiting busy threads, busy threads:{2}", PoolName, ((ThreadInfo)threadInfo).ThreadNumber, busyThreadCount);

                            if (busyThreadCount == 0)
                            {
                                Console.WriteLine("all threads terminating, Queue is empty!");
                                manualEvent.Set();
                            }

                            //Wait busy threads
                            Thread.Sleep(1000);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Thread pool:{0} Thread Number:{1} ExitOnFinish:false, thread sleep 1 second, Waiting new jobs! busy thread count:{2}", PoolName, ((ThreadInfo)threadInfo).ThreadNumber, busyThreadCount);
                            Thread.Sleep(1000);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Thread pool:{0} Thread Number:{1} jobs in queue:{2} busy thread count:{3}", PoolName, ((ThreadInfo)threadInfo).ThreadNumber, _jobQueue.Count, busyThreadCount);
                        Thread.Sleep(1000);
                    }
                }
            }
        }

        /// <summary>
        /// add any type of objects, its will come to your  callback method input
        /// </summary>
        /// <param name="job">you can use this object, in your callback method</param>
        public void addJob(object job)
        {
            this.JobQueue.Enqueue(new JobData() { Job = job, PoolName = _poolName });
        }
        public ThreadPoolHandler WaitOne()
        {
            manualEvent.WaitOne();
            Console.WriteLine("Job queue is empty! - exiting...");
            return this;
        }
        public ThreadPoolHandler WaitOne(int timeout)
        {
            manualEvent.WaitOne(timeout);
            Console.WriteLine("Job queue is empty! - exiting...");
            return this;
        }
        public ThreadPoolHandler WaitOne(int timeout, bool exitContext)
        {
            manualEvent.WaitOne(timeout, exitContext);
            Console.WriteLine("Job queue is empty! - exiting...");
            return this;
        }
        public ThreadPoolHandler WaitOne(TimeSpan timeout)
        {
            manualEvent.WaitOne(timeout);
            Console.WriteLine("Job queue is empty! - exiting...");
            return this;
        }
        public ThreadPoolHandler WaitOne(TimeSpan timeout, bool exitContext)
        {
            manualEvent.WaitOne(timeout, exitContext);
            Console.WriteLine("Job queue is empty! - exiting...");
            return this;
        }
    }

}

namespace RoboUtil.managers.thread
{
    [Serializable]
    public class ThreadInfo
    {
        public int ThreadNumber;

        public string ThreadName { get; set; }

        private static int threadCount = -1;

        public bool ExitOnFinish { get; set; }

        public bool IsBusy { get; set; }

        public ThreadInfo(string threadName)
        {
            Initialize(threadName, true);
        }

        public ThreadInfo(bool exitOnFinish)
        {
            Initialize(null, exitOnFinish);
        }

        private void Initialize(string threadName, bool exitOnFinish)
        {
            Interlocked.Increment(ref threadCount);
            ThreadNumber = threadCount;
            ThreadName = string.IsNullOrEmpty(threadName) ? "Thread-" + threadCount : threadName;
            ExitOnFinish = exitOnFinish;
            IsBusy = false;

        }

        public override string ToString()
        {
            return ThreadName;
        }
    }

    [Serializable]
    public class JobData
    {
        public object Job { get; set; }
        public ThreadInfo ThreadInfo { get; set; }
        public string PoolName { get; set; }
        public JobData()
        {
        }
    }

    public class ThreadPoolOptions
    {
        public WaitCallback TargetMethod { get; set; } = null;
        public int? PoolSize { get; set; } = null;
        public string PoolName { get; set; } = null;

        public bool? ExitOnFinish { get; set; } = null;
        public List<object> Jobs { get; set; } = new List<object>();
    }
}
