using RoboUtil.Common;
using RoboUtil.Common.Service;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboUtil.Common
{
    public class RepositoryManager : BaseRepositoryManager
    {
        public RepositoryManager(bool useTransaction, DbConnection connection, ServiceContext serviceContext) : base(useTransaction, connection, serviceContext)
        {
        }

        #region ExampleRepository

        private ExampleSqlRepository _exampleSqlRepository;

        public ExampleSqlRepository ExampleSqlRepository
        {
            get
            {
                if (this._exampleSqlRepository == null)
                    this._exampleSqlRepository = new ExampleSqlRepository(DatabeseContext, ServiceContext);
                return _exampleSqlRepository;
            }
        }

        private ExampleDapperRepository _exampleDapperRepository;

        public ExampleDapperRepository ExampleDapperRepository
        {
            get
            {
                if (this._exampleDapperRepository == null)
                    this._exampleDapperRepository = new ExampleDapperRepository(DatabeseContext, ServiceContext);
                return _exampleDapperRepository;
            }
        }

        #endregion ExampleRepository
    }
}