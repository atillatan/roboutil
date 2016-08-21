using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RoboUtil.Common;


using RoboUtil.utils;


namespace RoboUtil.Common
{
    public class ExampleSqlRepository : BaseRepository<ExampleDto>
    {
        public ExampleSqlRepository(DatabeseContext databeseContext) : base(databeseContext) { }

        public override ExampleDto Get(int id)
        {
            return DynamicDbUtil.Get<ExampleDto>((SqlConnection)_databeseContext.Connection, "select * from Example Where Id={0}", id);
        }

        public override IList<ExampleDto> List()
        {
            return DynamicDbUtil.List<ExampleDto>((SqlConnection)_databeseContext.Connection, "select * from Example");
        }

        public override int Insert(ExampleDto obj)
        {
            return DynamicDbUtil.Execute((SqlConnection)_databeseContext.Connection, "insert into Example (StringVar,IntVar,DateTimeVar) Values({0},{1},GETDATE()", obj.StringVar, obj.IntVar);
        }

        public override int Update(ExampleDto obj)
        {
            return DynamicDbUtil.Execute((SqlConnection)_databeseContext.Connection, "update Example set StringVar={0},IntVar={1},DateTimeVar={2} where id={3}", obj.StringVar, obj.IntVar, DateTime.Now, obj.Id);
        }

        public override int Delete(int id)
        {
            return DynamicDbUtil.Execute((SqlConnection)_databeseContext.Connection, "delete from Example Where Id={0}", id);
        }

    }
}
