using System;

//using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using RoboUtil.Common;
using System.Collections.Generic;
using System.Security.Claims;

namespace RoboUtil.Common.Service.API
{
    public class BaseAPIController<T> : Controller
    {
        //protected static readonly ILog Log = LogManager.GetLogger(typeof(T));

        protected ServiceManager Sm;

        protected IConfiguration _config;
        protected IServiceProvider _aspNetServiceProvider { get; set; }
        // protected IApplicationEnvironment _ApplicationEnvironment { get; set; }net46
        protected IHostingEnvironment _HostingEnvironment { get; set; }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            BuildServiceRequest(actionContext);
            base.OnActionExecuting(actionContext);
        }

        private void BuildServiceRequest(ActionExecutingContext actionContext)
        {
            _aspNetServiceProvider = actionContext.HttpContext.RequestServices;
            _config = (IConfiguration)_aspNetServiceProvider.GetService(typeof(IConfiguration));
            // _ApplicationEnvironment =(IApplicationEnvironment)_aspNetServiceProvider.GetService(typeof(IApplicationEnvironment));net46
            _HostingEnvironment = (IHostingEnvironment)_aspNetServiceProvider.GetService(typeof(IHostingEnvironment));


            ServiceContext ctx = new ServiceContext();
            Sm = new ServiceManager(ctx);

            if (actionContext.HttpContext.Request != null && actionContext.HttpContext.Request.Path != null)
            {
                ctx.URL = actionContext.HttpContext.Request.GetDisplayUrl();
                ctx.ServerIP = actionContext.HttpContext.Request.Host.Value;      

                ctx.RequestID = Guid.NewGuid().ToString();
                //ctx.UserInfo = new UserInfo() { Claims = User.Claims };
                Dictionary<object, object> _items = new Dictionary<object, object>();
                _items.Add("Application", "RoboUtil");
                ctx.Items = _items;
            }
        }

        #region Dispose

        bool disposed;

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //dispose managed resources
                    if (Sm != null)
                    {
                        //Log.Debug("BaseServiceManager is being disposed");net46
                        Sm.Dispose();
                    }
                }
                //dispose unmanaged resources
                disposed = true;
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
