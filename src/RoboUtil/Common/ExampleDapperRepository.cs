using Dapper;
using RoboUtil.Common;
using RoboUtil.Common.Service;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboUtil.Common
{
    public class ExampleDapperRepository : BaseRepository<ExampleDto>
    {
        public ExampleDapperRepository(DatabeseContext databeseContext, ServiceContext serviceContext) : base(databeseContext, serviceContext)
        {
        }

        public override int Insert(ExampleDto obj)
        {
            return DatabeseContext.Connection.Execute(@"UPDATE Example SET StringVar=@StringVAr, LastName=@IntVar WHERE Id = @Id", obj);
        }

        public override ExampleDto Get(int id)
        {
            return DatabeseContext.Connection.Query<ExampleDto>(@"select * from Example Where Id=@Id", new { Id = id }).SingleOrDefault();
        }

        public override int Update(ExampleDto obj)
        {
            return DatabeseContext.Connection.Execute(@"UPDATE Example SET StringVar=@StringVAr, LastName=@IntVar WHERE Id = @Id", obj);
        }

        public override int Delete(int id)
        {
            return DatabeseContext.Connection.Execute(@"Delete from Example WHERE Id=@Id", new { Id = id });
        }

        public IList<ExampleDto> List(BaseDto exampleDto, PagingDto pagingDto)
        {
            return DatabeseContext.Connection.Query<ExampleDto>(@"select * from Example").ToList();
        }
    }
}