using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboUtil.Common
{
    public class BaseRepositoryManager:IDisposable
    {
        protected DatabeseContext _databeseContext = null;
        public BaseRepositoryManager(bool useTransaction, DbConnection connection)
        {
            _databeseContext = new DatabeseContext(useTransaction, connection);
        }
        public void Commit()
        {
            try
            {
                _databeseContext.Commit();
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
                _databeseContext.Rollback();
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
                    if (_databeseContext != null)
                    {
                        //_log.Debug("DataBaseContext is being disposed");
                        _databeseContext.Dispose();
                    }
                }
                disposed = true;
            }
        }
        ~BaseRepositoryManager()
        {
            Dispose(false);
        }
        #endregion
    }
}
