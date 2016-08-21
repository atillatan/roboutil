using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboUtil.Common
{
    public interface IRepository<T>
    {
        T Get(int id);
        IList<T> List();
        int Insert(T obj);
        int Update(T obj);
        int Delete(int id);
    }
}
