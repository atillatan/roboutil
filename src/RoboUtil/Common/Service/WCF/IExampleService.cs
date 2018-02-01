//Atilla: I'll redesign WCF service to be compatible with .net core 2.0
/*
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using RoboUtil.Common;

namespace RoboUtil.Common.Service.WCF
{
    [ServiceContract(Namespace = "urn:Core-IExampleService", Name = "IExampleService")]
    public interface IExampleService
    {
        [OperationContract]
        ServiceResponse<ExampleDto> Get(int id);

        [OperationContract]
        ServiceResponse<IEnumerable<ExampleDto>> List();

        [OperationContract]
        ServiceResponse<int> Insert(ExampleDto exampleDto);
        [OperationContract]
        ServiceResponse<int> Update(int id, ExampleDto exampleDto);
        [OperationContract]
        ServiceResponse<int> Delete(int id);

    }
}
*/