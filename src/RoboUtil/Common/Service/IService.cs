using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboUtil.Common.Service
{
    public interface IService<TBaseRepositoryManager>
    {
        BaseRepositoryManager BaseRepositoryManager { set; }
        ServiceContext ServiceContext { get; }

        TBaseRepositoryManager RepositoryManager { get; }

        TBaseRepositoryManager Rm { get; }
    }
}