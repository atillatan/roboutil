using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{

    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        Test1();
    //        Console.ReadKey();
    //    }

    //    private static void Test1()
    //    {
    //        Console.WriteLine("Current Context is {0}", ContextManager.Current != null ? ContextManager.Current.InnerContext.ToString() : "null");
    //        using (new ContextManager(new DatabeseContext("Connection1")))
    //        {
    //            Console.WriteLine("Current Context is {0}", ContextManager.Current != null ? ContextManager.Current.InnerContext.ToString() : "null");
    //            using (new ContextManager(new DatabeseContext("Connection2")))
    //            {
    //                Console.WriteLine("Current Context is {0}", ContextManager.Current != null ? ContextManager.Current.InnerContext.ToString() : "null");
    //            }
    //            Console.WriteLine("Current Context is {0}", ContextManager.Current != null ? ContextManager.Current.InnerContext.ToString() : "null");
    //        }
    //        Console.WriteLine("Current Context is {0}", ContextManager.Current != null ? ContextManager.Current.InnerContext.ToString() : "null");
    //    }
    //}

    public class ContextManager : IDisposable
    {
        private static Stack<ContextManager> scopeStack = new Stack<ContextManager>();
        public IDisposable InnerContext { get; set; }
        public ContextManager(IDisposable ctx)
        {
            InnerContext = ctx;
            scopeStack.Push(this);
        }
        public static ContextManager Current
        {
            get
            {
                if (scopeStack.Count == 0)
                {
                    return null;
                }
                return scopeStack.Peek();
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (ShouldKillScope())
            {
                KillScope();
            }
            scopeStack.Pop();
        }

        #endregion

        private void KillScope()
        {
            //kill scope
            Console.WriteLine("Disposing:" + InnerContext.ToString());
            InnerContext.Dispose();
        }

        private bool ShouldKillScope()
        {
            bool result = true;
            //check if should kill..
            return result;
        }
    }



    public class DatabeseContext : IDisposable
    {
        public string ConnectionString { get; set; }


        public DatabeseContext(string name)
        {
            ConnectionString = name;
        }

        public override string ToString()
        {
            return ConnectionString;
        }

        #region Disposing
        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //if (_tran != null && _transactionStatus.Length == 0)
                    //{
                    //    //_log.Debug("Database transaction is being rollbacked");
                    //    _tran.Rollback();
                    //    _tran.Dispose();
                    //}

                    //if (_connection != null && _connection.State == ConnectionState.Open)
                    //{
                    //    //_log.Debug("Database connection is being closed");
                    //    _connection.Close();
                    //    _connection.Dispose();
                    //}
                }
                disposed = true;
            }
        }
        ~DatabeseContext()
        {
            Dispose(false);
        }
        #endregion
    }


}
