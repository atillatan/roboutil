//Atilla: I'll redesign WCF service to be compatible with .net core 2.0
//using log4net;
/*
using RoboUtil;
using RoboUtil.Common;
using RoboUtil.managers;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace RoboUtil.Common.Service.WCF
{
    public class BaseWcfService<T>
    {
        //protected static readonly ILog Log = LogManager.GetLogger(typeof(T));

        //protected BaseServiceManager<T> _baseServiceManager;

        //public BaseWcfService()
        //{
        //    ServiceContext ctx = new ServiceContext();

        //    MessageProperties mp = OperationContext.Current.RequestContext.RequestMessage.Properties;

        //    if (!mp.ContainsKey("BaseServiceManager"))
        //    {
        //        //TODO:Atilla daha sonra bu iptal edilecek, ss=new T();
        //        var sm = Activator.CreateInstance(typeof(T), ctx);
        //        mp.Add("ServiceManager", sm);
        //    }
        //    _baseServiceManager = mp["ServiceManager"] as BaseServiceManager<T>;
        //}

        //public Ti GetServiceManager<Ti>() where Ti : class
        //{
        //    return _baseServiceManager as Ti;
        //}
    }
}
*/