using RoboUtil.Common.Service;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboUtil.Common
{
    public class BaseRepositoryManager : IDisposable
    {
        protected DatabeseContext DatabeseContext = null;

        protected ServiceContext ServiceContext = null;

        public BaseRepositoryManager(bool useTransaction, DbConnection connection, ServiceContext serviceContext)
        {
            DatabeseContext = new DatabeseContext(useTransaction, connection);
            ServiceContext = serviceContext;
        }

        public void Commit()
        {
            try
            {
                DatabeseContext.Commit();
            }
            catch (Exception e)
            {
                //Log.Exception(e);
                throw e;
            }
        }

        public void RollBack()
        {
            try
            {
                DatabeseContext.Rollback();
            }
            catch (Exception e)
            {
                //Log.Exception(e);
                throw e;
            }
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
                    if (DatabeseContext != null)
                    {
                        //_log.Debug("DataBaseContext is being disposed");
                        DatabeseContext.Dispose();
                    }
                }
                disposed = true;
            }
        }

        ~BaseRepositoryManager()
        {
            Dispose(false);
        }

        #endregion Disposing
    }
}