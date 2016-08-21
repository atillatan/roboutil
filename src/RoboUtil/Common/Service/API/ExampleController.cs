//using System.Collections.Generic;
//using RoboUtil.Common;
//using Microsoft.AspNetCore.Mvc;

//namespace RoboUtil.Common.Service.API
//{
//    [Route("api/[controller]")]
//    public class ExampleController : BaseAPIController<ExampleController>
//    {
//        // GET api/example/5
//        [HttpGet("{id}", Name = "Get")]
//        public IActionResult Get(int id)
//        {
//            ServiceResponse<ExampleDto> result = Proxy.Invoke(() => Sm.ExampleService.Get(id), Sm.ExampleService);
//            return new ObjectResult(result);
//        }

//        // GET api/example
//        [HttpGet]
//        public ServiceResponse<IEnumerable<ExampleDto>> Get()
//        {
//            return Proxy.Invoke(() => Sm.ExampleService.List(), Sm.ExampleService);
//        }

//        // POST api/example
//        [HttpPost]
//        public ServiceResponse<int> Post([FromBody] ExampleDto exampleDto)
//        {
//            return Proxy.Invoke(() => Sm.ExampleService.Insert(exampleDto), Sm.ExampleService);
//        }

//        // PUT api/example/5
//        [HttpPut("{id}")]
//        public ServiceResponse<int> Put(int id, [FromBody] ExampleDto exampleDto)
//        {
//            return Proxy.Invoke(() => Sm.ExampleService.Update(exampleDto), Sm.ExampleService);
//        }

//        // DELETE api/example/5
//        [HttpDelete("{id}")]
//        public ServiceResponse<int> Delete(int id)
//        {
//            return Proxy.Invoke(() => Sm.ExampleService.Delete(id), Sm.ExampleService);
//        }

//    }

   
//}
