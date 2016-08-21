using System.Collections.Generic;
 

namespace RoboUtil.Common.Service
{
    public interface IExampleService
    {
        [Authorized(false)]
        ServiceResponse<ExampleDto> Get(int id);

        [Authorized]
        [Transactional(false)]
        ServiceResponse<IEnumerable<ExampleDto>> List();

        [Authorized]
        [Transactional]
        ServiceResponse<int> Insert(ExampleDto exampleDto);

        [Authorized]
        [Transactional]
        ServiceResponse<int> Update(ExampleDto exampleDto);

        [Authorized]
        [Transactional]
        ServiceResponse<int> Delete(int id);

    }
}
