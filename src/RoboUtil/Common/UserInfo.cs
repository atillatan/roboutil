using System.Collections.Concurrent;
using System.Security.Claims;
using System.Collections.Generic;
using System.Security;
namespace RoboUtil.Common
{
    public class UserInfo
    {
        public IEnumerable<Claim> Claims { get; set; }
        public ConcurrentDictionary<object, object> Items { get; set; }


    }
}
