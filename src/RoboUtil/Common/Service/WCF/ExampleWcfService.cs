using RoboUtil.Common;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace RoboUtil.Common.Service.WCF
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ExampleWcfService : BaseWcfService<ServiceManager>, IExampleService
    {
        public ServiceResponse<ExampleDto> Get(int id)
        {
            //return Proxy.Invoke(() => Sm.ExampleService.Get(id), Sm.ExampleService);
            return null;
        }

        public ServiceResponse<IEnumerable<ExampleDto>> List()
        {
            //return Proxy.Invoke(() => Sm.ExampleService.List(), Sm.ExampleService);
            return null;
        }

        public ServiceResponse<int> Insert(ExampleDto exampleDto)
        {
            //return Proxy.Invoke(() => Sm.ExampleService.Insert(exampleDto), Sm.ExampleService);
            return null;
        }

        public ServiceResponse<int> Update(int id, ExampleDto exampleDto)
        {
            //return Proxy.Invoke(() => Sm.ExampleService.Update(exampleDto), Sm.ExampleService);
            return null;
        }

        public ServiceResponse<int> Delete(int id)
        {
            //return Proxy.Invoke(() => Sm.ExampleService.Delete(id), Sm.ExampleService);
            return null;
        }
    }
}