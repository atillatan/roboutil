using System.Collections.Generic;
 

namespace RoboUtil.Common.Service
{
    public interface IExampleService
    {
        [Authorized("")]
        [Transactional(false)]
        ServiceResponse<ExampleDto> Get(int id);

        [Authorized("")]        
        [Transactional(false)]
        ServiceResponse<IEnumerable<ExampleDto>> List();

        [Authorized("")]
        [Transactional(true)]
        ServiceResponse<int> Insert(ExampleDto exampleDto);

        [Authorized("")]
        [Transactional(true)]
        ServiceResponse<int> Update(ExampleDto exampleDto);

        [Authorized("")]
        [Transactional(true)]
        ServiceResponse<int> Delete(int id);

    }
}
