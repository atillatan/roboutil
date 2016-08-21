using System;
using System.Data;
using System.Data.Common;

namespace RoboUtil.Common
{
    public class DatabeseContext : IDisposable
    {
        private string _transactionStatus = "";

        private DbConnection _connection;
        public DbConnection Connection{get { return _connection; }}

        private DbTransaction _tran;
        public DbTransaction Transaction{get { return _tran; }}

        public DatabeseContext(bool useTransaction, DbConnection connection)
        {
            _connection = connection;
            if (useTransaction)
            {
                //_log.Debug("Database connection is being opened");
                _connection.Open();
                _tran = _connection.BeginTransaction(IsolationLevel.ReadCommitted);
            }
        }

        public void Commit()
        {
            if (_tran != null)
            {
                //_log.Debug("Database transaction is being commited");
                _tran.Commit();
                _transactionStatus = "COMMITED";
            }
        }

        public void Rollback()
        {
            if (_tran != null)
            {
                //_log.Debug("Database transaction is being rollbacked");
                _tran.Rollback();
                _transactionStatus = "ROLLBACKED";
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
                    if (_tran != null && _transactionStatus.Length == 0)
                    {
                        //_log.Debug("Database transaction is being rollbacked");
                        _tran.Rollback();
                        _tran.Dispose();
                    }

                    if (_connection != null && _connection.State == ConnectionState.Open)
                    {
                        //_log.Debug("Database connection is being closed");
                        _connection.Close();
                        _connection.Dispose();
                    }
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