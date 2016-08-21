using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoboUtil.Common.Service;

namespace RoboUtil.Common.Service
{
    public class ServiceManager: BaseServiceManager<ServiceManager>  
    {
        public ServiceManager(ServiceContext serviceContext) : base(serviceContext){}

        #region ExampleService

        //private ExampleService _exampleService;

 

        //public ExampleService ExampleService
        //{
        //    get
        //    {
        //        if (this._exampleService == null)
        //            this._exampleService = new ExampleService(_serviceContext);
        //        return _exampleService;
        //    }
        //}

        #endregion

     
    }
}
