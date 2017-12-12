using RoboUtil.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboUtil.Common.Service
{
    public class ServiceContext : IDisposable
    {
        public UserInfo UserInfo { get; set; }
        public string RequestID { get; set; }
        public string ServerIP { get; set; }
        public string URL { get; set; }
        public Dictionary<object, object> Items { get; set; }

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
                }
                disposed = true;
            }
        }

        ~ServiceContext()
        {
            Dispose(false);
        }

        #endregion Disposing
    }
}