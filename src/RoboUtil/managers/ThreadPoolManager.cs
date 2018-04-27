using System;
using System.Collections;
using System.Collections.Concurrent;
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

        /// <summary>
        /// ThreadPoolManager makes easy to use ThreadPools
        /// </summary>
        /// <param name="poolName">define pool for same jobs</param>
        /// <param name="poolSize">define how many Thread will run in pool</param>
        /// <param name="callbackMethod">which method will run from every Thread</param>
        /// <returns></returns>
        public ThreadPoolHandler CreatePool(string poolName, int poolSize, WaitCallback callbackMethod)
        {
            ThreadPoolHandler handler = new ThreadPoolHandler(poolName, poolSize, callbackMethod);
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

        private Thread _mainThread;

        public Thread MainThread
        {
            get { return _mainThread; }
        }

        #endregion Properties

        public ThreadPoolHandler(string poolName, int poolSize, WaitCallback callbackMethod)
        {
            _jobQueue = Queue.Synchronized(new Queue());
            _poolName = poolName;
            _poolSize = poolSize;
            _waitCallback = callbackMethod;
        }

        /// <summary>
        /// it starts all threads in yout ThreadPool
        /// </summary>
        public void Start()
        {
            for (int i = 1; i < _poolSize + 1; i++)
            {
                ThreadPool.QueueUserWorkItem(JobConsumer, new ThreadInfo(i));
            }
            _mainThread = new Thread(RunMainThread);
            _mainThread.Name = PoolName + "-MainThread";
            _mainThread.Start();
        }

        private void RunMainThread()
        {
            while (true)
            {
                if (_jobQueue.Count != 0)
                {
                    Console.WriteLine("JobQueue.Count:{0} Pool:{1} continue MainThread", _jobQueue.Count,PoolName);
                    Thread.Sleep(1000);
                }
                else
                {
                    Console.WriteLine("JobQueue.Count:{0} Pool:{1} stoping MainThread", _jobQueue.Count,PoolName);
                    break;
                }
            }
        }

        private void JobConsumer(object threadInfo)
        {
            int checkQueuCount = 0;

            while (true)
            {
                JobData job = null;
                try
                {
                    job = (_jobQueue.Count > 0 ? _jobQueue.Dequeue() : null) as JobData;
                }
                catch (Exception)
                {
                    Console.WriteLine("Queue is empty!");
                }

                if (job != null)
                {
                    job.ThreadInfo = threadInfo as ThreadInfo;
                    job.PoolName = _poolName;
                    try
                    {
                        _waitCallback(job);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
                    }
                }
                else
                {
                    //wait for main thread
                    Thread.Sleep(1000);

                    if (_jobQueue.Count > 0)
                    {
                        continue;
                    }

                    checkQueuCount++;

                    if (checkQueuCount > 2)
                    {
                        Console.WriteLine("Thread pool:{0} Thread Number:{1} has terminated, Queue is empty!", PoolName, ((ThreadInfo)threadInfo).ThreadNumber);
                        break;
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
    }





}
namespace RoboUtil.managers.thread
{
    [Serializable]
    public class ThreadInfo
    {
        public int ThreadNumber;

        public ThreadInfo(int threadNumber)
        {
            ThreadNumber = threadNumber;
        }
        public ThreadInfo()
        {
        }
    }
}
namespace RoboUtil.managers.thread
{
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