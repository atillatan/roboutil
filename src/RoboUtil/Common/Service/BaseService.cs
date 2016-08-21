using RoboUtil.Common;

namespace RoboUtil.Common.Service
{
    public class BaseService : IService
    {
        private BaseRepositoryManager _baseRepositoryManager;
        public BaseRepositoryManager baseRepositoryManager
        {
            get { return _baseRepositoryManager; }
            set { _baseRepositoryManager = value; }
        }

        public T GetRepository<T>() where T : class
        {
            return _baseRepositoryManager as T;
        }

        private ServiceContext _serviceContext;
        public ServiceContext ServiceContext
        {
            get { return _serviceContext; }
        }
        public BaseService(ServiceContext serviceContext)
        {
            this._serviceContext = serviceContext;
        }
    }
}
