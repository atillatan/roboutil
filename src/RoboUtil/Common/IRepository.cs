using RoboUtil.Common.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboUtil.Common
{
    public interface IRepository<T>
    {
        int Insert(T obj);

        T Get(int id);

        int Update(T obj);

        int Delete(int id);

        //IList<T> List(BaseDto dto, PagingDto pagingDto);
    }
}