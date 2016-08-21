using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RoboUtil.Common;



namespace RoboUtil.Common
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected DatabeseContext _databeseContext;

        public BaseRepository(DatabeseContext databeseContext)
        {
            _databeseContext = databeseContext;
        }
        public virtual T Get(int id)
        {
            throw new NotImplementedException();
        }

        public virtual IList<T> List()
        {
            throw new NotImplementedException();
        }
        public virtual IList<T> Fetch(int pageNumber, int rowsPage)
        {
            throw new NotImplementedException();
        }
        public virtual int Insert(T obj)
        {
            throw new NotImplementedException();
        }

        public virtual int Update(T obj)
        {
            throw new NotImplementedException();
        }

        public virtual int Delete(int id)
        {
            throw new NotImplementedException();
        }
     
    }
}
