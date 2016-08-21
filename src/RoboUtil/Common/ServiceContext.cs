//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Dynamic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


//namespace Core.Common.DTO
//{
//    public class ServiceContext:IDisposable
//    {
//        public UserSession UserSession { get; set; }
//        public string RequestID { get; set; }
//        public string ServerIP { get; set; }
//        public string URL { get; set; }
//        public ConcurrentDictionary<object,object> Params { get; set; }
//        public string ReadConnectionString { get; set; }
//        public string WriteConnectionString { get; set; }
//        public string ProviderName { get; set; }
//        public string Profile { get; set; }


//        #region Disposing
//        private bool disposed = false;
//        public void Dispose()
//        {
//            Dispose(true);
//            GC.SuppressFinalize(this);
//        }
//        protected virtual void Dispose(bool disposing)
//        {
//            if (!disposed)
//            {
//                if (disposing)
//                {
                  
//                }
//                disposed = true;
//            }
//        }
//        ~ServiceContext()
//        {
//            Dispose(false);
//        }
//        #endregion
//    }
//}
