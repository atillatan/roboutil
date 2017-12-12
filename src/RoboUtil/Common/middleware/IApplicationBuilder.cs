using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboUtil.Common.middleware
{
    public interface IApplicationBuilder
    {
        InvokeDelegate Build();

        IApplicationBuilder Use(Func<InvokeDelegate, InvokeDelegate> middleware);
    }
}