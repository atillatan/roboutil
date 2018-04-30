using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
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
        public static int poolCount = 0;

        /// <summary>
        /// ThreadPoolManager makes easy to use ThreadPools
        /// </summary> 
        /// <param name="callbackMethod">which method will run from every Thread</param>
        /// <returns></returns>
        public ThreadPoolHandler CreatePool(WaitCallback callbackMethod)
        {
            Interlocked.Increment(ref poolCount);
            string poolName = "ThreadPool-" + poolCount;

            return CreatePool(poolName, int.MaxValue, callbackMethod, true);
        }

        public ThreadPoolHandler CreatePool(WaitCallback callbackMethod, bool exitOnFinish)
        {
            Interlocked.Increment(ref poolCount);
            string poolName = "ThreadPool-" + poolCount;

            return CreatePool(poolName, int.MaxValue, callbackMethod, exitOnFinish);
        }

        /// <summary>
        /// ThreadPoolManager makes easy to use ThreadPools
        /// </summary>
        /// <param name="poolSize">define how many Thread will run in pool</param>
        /// <param name="callbackMethod">which method will run from every Thread</param>
        /// <returns></returns>
        public ThreadPoolHandler CreatePool(int poolSize, WaitCallback callbackMethod)
        {
            Interlocked.Increment(ref poolCount);
            string poolName = "ThreadPool-" + poolCount;
            return CreatePool(poolName, poolSize, callbackMethod, true);
        }

        /// <summary>
        /// ThreadPoolManager makes easy to use ThreadPools
        /// </summary>
        /// <param name="poolSize">define how many Thread will run in pool</param>
        /// <param name="callbackMethod">which method will run from every Thread</param>
        /// <returns></returns>
        public ThreadPoolHandler CreatePool(int poolSize, WaitCallback callbackMethod, bool exitOnFinish)
        {
            Interlocked.Increment(ref poolCount);
            string poolName = "ThreadPool-" + poolCount;
            return CreatePool(poolName, poolSize, callbackMethod, exitOnFinish);
        }

        public ThreadPoolHandler CreatePool(string poolName, int poolSize, WaitCallback callbackMethod)
        {
            return CreatePool(poolName, poolSize, callbackMethod, true);
        }
        /// <summary>
        /// ThreadPoolManager makes easy to use ThreadPools
        /// </summary>
        /// <param name="poolName">define pool for same jobs</param>
        /// <param name="poolSize">define how many Thread will run in pool</param>
        /// <param name="callbackMethod">which method will run from every Thread</param>
        /// <returns></returns>
        public ThreadPoolHandler CreatePool(string poolName, int poolSize, WaitCallback callbackMethod, bool exitOnFinish)
        {
            ThreadPoolHandler handler = new ThreadPoolHandler(poolName, poolSize, callbackMethod, exitOnFinish);
            ThreadPoolHandler result = null;

            if (!_pool.ContainsKey(poolName))
            {
                if (_pool.TryAdd(poolName, handler))
                {
                    result = handler;
                }
            }

            return result;
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

        // private Thread _mainThread;

        // public Thread MainThread
        // {
        //     get { return _mainThread; }
        // }

        private ManualResetEvent[] manualEvents { get; set; }

        #endregion Properties

        public ThreadPoolHandler(string poolName, int poolSize, WaitCallback callbackMethod, bool exitOnFinish)
        {
            _jobQueue = Queue.Synchronized(new Queue());
            _poolName = poolName;
            _poolSize = poolSize;
            _waitCallback = callbackMethod;
            _exitOnFinish = exitOnFinish;
        }

        /// <summary>
        /// it starts all threads in yout ThreadPool
        /// </summary>
        public void Start()
        {
            manualEvents = new ManualResetEvent[_poolSize];

            for (int i = 0; i < _poolSize; i++)
            {
                manualEvents[i] = new ManualResetEvent(false);
                ThreadInfo threadInfo = new ThreadInfo(manualEvents[i], _exitOnFinish);
                ThreadPool.QueueUserWorkItem(JobConsumer, threadInfo);
            }
            // _mainThread = new Thread(RunMainThread);
            // _mainThread.Name = PoolName + "-MainThread";
            // _mainThread.Start();
        }

        // private void RunMainThread()
        // {
        //     while (true)
        //     {
        //         if (_jobQueue.Count != 0)
        //         {
        //             Console.WriteLine("MainThread: JobQueue.Count:{0} Pool:{1}", _jobQueue.Count, PoolName);
        //             Thread.Sleep(1000);
        //         }
        //         else
        //         {
        //             Console.WriteLine("MainThread: JobQueue.Count:{0} Pool:{1}", _jobQueue.Count, PoolName);
        //             Console.WriteLine("MainThread: Aborting", _jobQueue.Count, PoolName);
        //             return;
        //         }
        //     }
        // }
        private void JobConsumer(object threadInfo)
        {
            int checkQueuCount = 0;
            ThreadInfo _threadInfo = threadInfo as ThreadInfo;

            while (true)
            {
                JobData job = null;
                try
                {
                    job = (_jobQueue.Count > 0 ? _jobQueue.Dequeue() : null) as JobData;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Queue is empty!" + e.Message);
                }

                if (job != null)
                {
                    job.ThreadInfo = _threadInfo;
                    job.PoolName = _poolName;
                    try
                    {
                        _waitCallback(job);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
                        Console.WriteLine("Continuoue to next job, Thread Number:{1}");
                    }
                }
                else
                {
                    Thread.Sleep(1000);

                    if (_jobQueue.Count > 0) { continue; }

                    checkQueuCount++;

                    if (checkQueuCount > 2)
                    {

                        _threadInfo.manualEvent.Set();
                        if (_threadInfo.ExitOnFinish == true)
                        {
                            Console.WriteLine("Thread pool:{0} Thread Number:{1} has terminated, Queue is empty!", PoolName, ((ThreadInfo)threadInfo).ThreadNumber);
                            break;
                        }else{
                            Console.WriteLine("Thread pool:{0} Thread Number:{1} Waiting job Queue!", PoolName, ((ThreadInfo)threadInfo).ThreadNumber);
                        }
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
        public void WaitAll()
        {
            WaitHandle.WaitAll(this.manualEvents);
            Console.WriteLine("Job queue is empty! - exiting...");
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

        public ManualResetEvent manualEvent;

        private static int threadCount = -1;

        public bool ExitOnFinish { get; set; }


        public ThreadInfo(ManualResetEvent resetEvent)
        {
            Initialize(resetEvent, null, true);
        }

        public ThreadInfo(ManualResetEvent resetEvent, string threadName)
        {
            Initialize(resetEvent, threadName, true);
        }

        public ThreadInfo(ManualResetEvent resetEvent, bool exitOnFinish)
        {
            Initialize(resetEvent, null, exitOnFinish);
        }

        private void Initialize(ManualResetEvent resetEvent, string threadName, bool exitOnFinish)
        {
            manualEvent = resetEvent;
            Interlocked.Increment(ref threadCount);
            ThreadNumber = threadCount;
            ThreadName = string.IsNullOrEmpty(threadName) ? "Thread-" + threadCount : threadName;
            ExitOnFinish = exitOnFinish;

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
}
