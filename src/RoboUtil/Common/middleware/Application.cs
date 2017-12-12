using RoboUtil.Common.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RoboUtil.Common.middleware
{
    public class Application
    {
        #region Singleton Implementation

        private static Application _application = null;
        private static readonly object SyncRoot = new Object();

        private Application()
        {
        }

        public static Application Current
        {
            get
            {
                if (_application == null)
                {
                    lock (SyncRoot)
                    {
                        if (_application == null)
                            _application = new Application();
                    }
                }
                return _application;
            }
        }

        #endregion Singleton Implementation

        public readonly IApplicationBuilder appBuilder = new ApplicationBuilder();

        public InvokeDelegate app;

        public void Build()
        {
            app = appBuilder.Build();
        }

        public static async void Invoke<TResult>(Expression<Func<object>> function, object instance) where TResult : class
        {
            InvokeContext context = new InvokeContext(function, instance, typeof(TResult));
            await Application.Current.app.Invoke(context);
        }

        //public static TResult Invoke2<TResult>(Expression<Func<object>> function, object instance) where TResult : class
        //{
        //    InvokeContext context = new InvokeContext(function, instance, typeof(TResult));
        //    Application.Current.app.Invoke(context).Wait();
        //    if (context?.Result?.Value.GetType() != typeof(TResult))
        //    {
        //        throw new ArgumentException("Given type mismatch with result type!");
        //    }
        //    return context.Result.Value as TResult;
        //}

        //public static Task<TResult> InvokeAsync<TResult>(Expression<Func<object>> function, object instance) where TResult : class
        //{
        //    return Task.Run(() => Application.Invoke2<TResult>(function, instance));
        //}
    }

    //public static class ServiceExtension
    //{
    //    public static Task InvokeAsync<TResult>(this BaseService<BaseRepositoryManager> instance, Expression<Func<object>> function) where TResult : class
    //    {
    //        return Task.Run(() => Application.Invoke<TResult>(function, instance));
    //    }

    //    public static TResult Invoke<TResult>(this BaseService<BaseRepositoryManager> instance, Expression<Func<object>> function) where TResult : class
    //    {
    //        return Application.Invoke<TResult>(function, instance);
    //    }
    //}
}