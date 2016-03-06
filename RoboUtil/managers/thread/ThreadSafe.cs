using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RoboUtil.managers.thread
{
    /// <summary>
    /// bir isi yapan bir thread yap icindeki method ise yonelik listesini alsin 
    /// ve bitirene kadar available olmasin icini bitirince available olsun
    /// ana thread, therad leri tarasin available olana elindeki listeden versin
    /// 
    /// </summary>
    public class ThreadSafe
    {
        //private static int count = 0;
        //private static readonly object locker = new object();

        //private static void Main()
        //{
        //    Thread.CurrentThread.Name = "MainThread";

        //instance method
        //DoWork dw=new DoWork();
        //Thread t = new Thread(dw.Go2);
        //t.Name = "T2";
        //t.Start();

        //statid method
        //Thread t2=new Thread(DoWork.Go3);


        //Join to main thread
        //Thread t=new Thread(DoWork.RunWithStatic);
        //t.Name = "SubThread";
        //t.Start();
        //t.Join();//Bind to mainThread, mainThread waiting subThread
        //Console.WriteLine("MainThred Finish");


        //use lambda expression
        //Thread t =new Thread(()=>Console.WriteLine("test"));
        //t.Start();

        //user dispather
        //DoWork dw = new DoWork();
        //Dispatcher.Invoke(() =>dw.Run(), dw);

        //pass the parameter
        //Thread t =new Thread(()=>DoWork.RunWithStatic("test"));
        //t.Start();

        //pass the parameter
        //new Thread(() =>
        //{
        //    Console.WriteLine("test1");
        //    Console.WriteLine("test2"); 
        //    DoWork.RunWithStatic("total");
        //}).Start();

        //another usage
        //for (int i = 0; i < 10; i++)
        //{
        //    int temp = i; //icerde tanimlanmali
        //    new Thread(() => Console.Write(temp)).Start();
        //}          

        //ThreadPool Example
        //bir alt class yapilmasi ve icine WaitCallback icin kullanilacak methodun yazilmasi lazim
        //    DoWork dw=new DoWork();
        //    ThreadPool.QueueUserWorkItem(new WaitCallback(dw.ThreadPoolCallback),1);
        //    ThreadPool.QueueUserWorkItem(new WaitCallback(dw.ThreadPoolCallback),2);



        //    Console.ReadKey();
        //}


    }

    public class DoWork
    {
        public static void RunWithStatic(string message)
        {
            for (int cycles = 0; cycles < 5; cycles++)
            {
                Console.Write(message);
                Console.WriteLine(Thread.CurrentThread.Name);
                Thread.Sleep(500);
            }

        }

        public string Run()
        {
            string result = "";

            for (int cycles = 0; cycles < 5; cycles++)
            {
                Console.Write("? ");
                Console.WriteLine(Thread.CurrentThread.Name);
                Thread.Sleep(500);
            }

            return result;

        }

        /// <summary>
        /// Bu methodun ThreadPool tarafindan cagrilabilmesi icin object parametresi almasi ve void donmesi lazim
        /// </summary>
        /// <param name="obj"></param>
        public void ThreadPoolCallback(Object obj)
        {
            int threadIndex = (int)obj;
            Console.WriteLine("thread {0} started..", threadIndex);
            for (int cycles = 0; cycles < 5; cycles++)
            {
                Console.Write("pool");
                Console.WriteLine(Thread.CurrentThread.Name);
                Thread.Sleep(500);
            }
            Console.WriteLine("thread {0} finished..", threadIndex);
        }
    }


}