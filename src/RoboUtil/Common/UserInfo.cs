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

        public IList<string> Roles { get; set; }
        public string Username { get; set; }
        public string OrganizationUnit { get; set; }


    }
}
