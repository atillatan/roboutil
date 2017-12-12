using RoboUtil;
using RoboUtil.Common;
using RoboUtil.Common.Service;
using RoboUtil.utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RoboUtil.Common
{
    public class ExampleSqlRepository : BaseRepository<ExampleDto>
    {
        public ExampleSqlRepository(DatabeseContext databeseContext, ServiceContext serviceContext) : base(databeseContext, serviceContext)
        {
        }

        public override int Insert(ExampleDto obj)
        {
            return Utils.DynamicDbUtil.Execute((SqlConnection)DatabeseContext.Connection, "insert into Example (StringVar,IntVar,DateTimeVar) Values({0},{1},GETDATE()", obj.StringVar, obj.IntVar);
        }

        public override ExampleDto Get(int id)
        {
            return Utils.DynamicDbUtil.Get((SqlConnection)DatabeseContext.Connection, "select * from Example Where Id={0}", id);
        }

        public override int Update(ExampleDto obj)
        {
            return Utils.DynamicDbUtil.Execute((SqlConnection)DatabeseContext.Connection, "update Example set StringVar={0},IntVar={1},DateTimeVar={2} where id={3}", obj.StringVar, obj.IntVar, DateTime.Now, obj.Id);
        }

        public override int Delete(int id)
        {
            return Utils.DynamicDbUtil.Execute((SqlConnection)DatabeseContext.Connection, "delete from Example Where Id={0}", id);
        }

        public IList<ExampleDto> List(BaseDto exampleDto, PagingDto pagingDto)
        {
            return Utils.DynamicDbUtil.List<ExampleDto>((SqlConnection)DatabeseContext.Connection, "select * from Example");
        }
    }
}