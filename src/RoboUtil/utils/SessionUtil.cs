using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace RoboUtil.utils
{
    public static class SessionUtil
    {
        private static IHttpContextAccessor _contextAccessor = new HttpContextAccessor();
        static SessionUtil()
        {
            
        }

        //public static int? SeciliPersonelKullaniciNo
        //{
        //    get { return _contextAccessor.HttpContext.Session["SeciliPersonel"] as int?; }
        //    set { HttpContext.Current.Session["SeciliPersonel"] = value; }
        //}
    }
}
