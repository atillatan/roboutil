using RoboUtil.Common;
using RoboUtil.Common.Service;

namespace RoboUtil.Common.Service
{
    public class BaseService<TBaseRepositoryManager> : IService<TBaseRepositoryManager>
        where TBaseRepositoryManager : class
    {
        private ServiceContext serviceContext;

        public ServiceContext ServiceContext
        {
            get { return serviceContext; }
        }

        public BaseService(ServiceContext serviceContext)
        {
            this.serviceContext = serviceContext;
        }

        private BaseRepositoryManager _baseRepositoryManager;

        public BaseRepositoryManager BaseRepositoryManager
        {
            set { _baseRepositoryManager = value; }
        }

        public TBaseRepositoryManager Rm
        {
            get { return _baseRepositoryManager as TBaseRepositoryManager; }
        }

        public TBaseRepositoryManager RepositoryManager
        {
            get { return _baseRepositoryManager as TBaseRepositoryManager; }
        }

        //public T GetRepository<T>()
        //{
        //    return _baseRepositoryManager as T;
        //}

        //public TBaseRepositoryManager GetRepository<TBaseRepositoryManager>() where TBaseRepositoryManager : class
        //{
        //    return _baseRepositoryManager as TBaseRepositoryManager;
        //}
    }
}