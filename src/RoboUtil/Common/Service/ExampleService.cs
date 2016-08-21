//using System.Collections.Generic;
//using System.Data.Common;
//using System.Linq;
//using System.Transactions;
//using RoboUtil.Common;
//using RoboUtil.Common.Service;
////using log4net;

//namespace RoboUtil.Common.Service
//{
//    public class ExampleService : BaseService, IExampleService
//    {
//        //private static readonly ILog _Log = LogManager.GetLogger(typeof(ExampleService));
//        public ExampleService(ServiceContext serviceContext) : base(serviceContext) { }

//        private RepositoryManager Rm
//        {
//            get { return GetRepository<RepositoryManager>(); }
//        }


//        public ServiceResponse<ExampleDto> Get(int id)
//        {

//            ExampleDto exampleDto = Rm.ExampleDapperRepository.Get(id);
//            if (exampleDto == null)
//                return new ServiceResponse<ExampleDto>(false, ResultType.Error, "Kayit Bulunamadi");
//            return new ServiceResponse<ExampleDto>(true, ResultType.Information, "", exampleDto);
//        }
//        public ServiceResponse<IEnumerable<ExampleDto>> List()
//        {
//            IEnumerable<ExampleDto> list = Rm.ExampleDapperRepository.List();
//            if (list == null || !list.Any())
//                return new ServiceResponse<IEnumerable<ExampleDto>>(false, ResultType.Error, "Kayit Bulunamadi");
//            return new ServiceResponse<IEnumerable<ExampleDto>>(true, ResultType.Information, "", list);
//        }
//        public ServiceResponse<int> Insert(ExampleDto exampleDto)
//        {
//            int rowsAffected = Rm.ExampleDapperRepository.Insert(exampleDto);
//            if (rowsAffected == 0)
//                return new ServiceResponse<int>(true, ResultType.Information, "Kayit Basari ile eklendi", rowsAffected);
//            return new ServiceResponse<int>(false, ResultType.Error, "Kayit eklenemedi!");
//        }
//        public ServiceResponse<int> Update(ExampleDto exampleDto)
//        {

//            int rowsAffected = Rm.ExampleDapperRepository.Update(exampleDto);
//            if (rowsAffected == 0)
//                return new ServiceResponse<int>(true, ResultType.Information, "Kayit Basari ile guncellendi", rowsAffected);
//            return new ServiceResponse<int>(false, ResultType.Error, "Kayit guncellenemedi!");
//        }
//        public ServiceResponse<int> Delete(int id)
//        {
//            int resutl = Rm.ExampleDapperRepository.Delete(id);
//            if (resutl > 0)
//                return new ServiceResponse<int>(true, ResultType.Information, "Kayit Basari ile silindi");
//            return new ServiceResponse<int>(false, ResultType.Error, "Kayit silinemedi!");
//        }
//    }
//}

