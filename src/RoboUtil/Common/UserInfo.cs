using System.Collections.Concurrent;
using System.Security.Claims;
namespace RoboUtil.Common
{
    public class UserInfo
    {
        public string Claims { get; set; }
        public ConcurrentDictionary<object, object> Items { get; set; }


    }
}
