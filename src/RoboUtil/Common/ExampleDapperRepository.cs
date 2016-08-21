using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoboUtil.Common;
using Dapper;

namespace RoboUtil.Common
{
    public class ExampleDapperRepository : BaseRepository<ExampleDto>
    {
        public ExampleDapperRepository(DatabeseContext databeseContext) : base(databeseContext) { }
        public override ExampleDto Get(int id)
        {
            return _databeseContext.Connection.Query<ExampleDto>(@"select * from Example Where Id=@Id", new {Id=id}).SingleOrDefault();
        }

        public override IList<ExampleDto> List()
        {
            return _databeseContext.Connection.Query<ExampleDto>(@"select * from Example").ToList();
        }

        public override int Insert(ExampleDto obj)
        {
            return _databeseContext.Connection.Execute(@"UPDATE Example SET StringVar=@StringVAr, LastName=@IntVar WHERE Id = @Id", obj);
        }

        public override int Update(ExampleDto obj)
        {
           return _databeseContext.Connection.Execute(@"UPDATE Example SET StringVar=@StringVAr, LastName=@IntVar WHERE Id = @Id", obj);
        }

        public override int Delete(int id)
        {
            return _databeseContext.Connection.Execute(@"Delete from Example WHERE Id=@Id", new { Id=id });
        }
    }
}
