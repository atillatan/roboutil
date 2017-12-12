using Dapper;
using Dapper.Contrib;
using Dapper.Contrib.Extensions;
using RoboUtil.Common;
using RoboUtil.Common.Service;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace RoboUtil.Common
{
    public class BaseRepository<TModel> : IRepository<TModel>
        where TModel : class
    {
        protected DatabeseContext DatabeseContext;

        protected ServiceContext ServiceContext;

        public BaseRepository(DatabeseContext databeseContext, ServiceContext serviceContext)
        {
            DatabeseContext = databeseContext;
            ServiceContext = serviceContext;
        }

        #region CRUD operations

        public virtual int Insert(TModel obj)
        {
            throw new NotImplementedException();
        }

        public virtual TModel Get(int id)
        {
            //var yontem1 = DatabeseContext.Connection.Query<TModel>($@"SELECT *  FROM   {GetTableName()}  WHERE  ID = :ID ", new { Id = id }).SingleOrDefault();
            //Type type = typeof(TModel);
            //var yontem2 = DatabeseContext.Connection.Get<TModel>(id);
            //return yontem2;
            throw new NotImplementedException();
        }

        public virtual int Update(TModel obj)
        {
            throw new NotImplementedException();
        }

        public virtual int Delete(int id)
        {
            //string tableName = (new TModel()).GetType().Name;
            //return DatabeseContext.Connection.Execute($@"UPDATE {tableName} SET IsActive=0 WHERE Id=@Id", new { Id = id });
            throw new NotImplementedException();
        }

        #endregion CRUD operations

        public int DeleteHard(TModel model)
        {
            //string tableName = (new TModel()).GetType().Name;
            //return DatabeseContext.Connection.Execute($@"DELETE FROM {tableName} WHERE Id=@Id", new { Id = model.Id });
            throw new NotImplementedException();
        }

        //public virtual IList<T> List(BaseDto dto, PagingDto pagingDto)
        //{
        //    throw new NotImplementedException();
        //}

        //private static readonly ConcurrentDictionary<RuntimeTypeHandle, string> TypeTableName = new ConcurrentDictionary<RuntimeTypeHandle, string>();
        //        private string GetTableName()
        //        {
        //            string name;
        //            Type type = typeof(TModel);

        //            //if (TypeTableName.TryGetValue(type.TypeHandle, out name)) return name;

        //            //if (TableNameMapper != null)
        //            //{
        //            //    name = TableNameMapper(type);
        //            //}
        //            //else
        //            //{
        //            //NOTE: This as dynamic trick should be able to handle both our own Table-attribute as well as the one in EntityFramework
        //            var tableAttr = type
        //#if COREFX
        //                    .GetTypeInfo()
        //#endif
        //                    .GetCustomAttributes(false).SingleOrDefault(attr => attr.GetType().Name == "TableAttribute") as dynamic;
        //            if (tableAttr != null)
        //                name = tableAttr.Name;
        //            else
        //            {
        //                name = type.Name;
        //                if (IsInterface(type) && name.StartsWith("I"))
        //                    name = name.Substring(1);
        //            }
        //            //}

        //            //TypeTableName[type.TypeHandle] = name;
        //            return name;
        //        }

        public bool IsInterface(Type type)
        {
#if COREFX
                    return type.GetTypeInfo().IsInterface;
#else
            return type.IsInterface;
#endif
        }
    }
}