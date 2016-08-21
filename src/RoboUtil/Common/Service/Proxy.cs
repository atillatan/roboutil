//using System;
//using System.Collections.Generic;
//using System.Data.Common;
//using System.Diagnostics;
//using System.Linq;
//using System.Linq.Expressions;
//using RoboUtil.Common;
//using RoboUtil.Common.Service;
//using RoboUtil;
//using RoboUtil.utils;

//namespace RoboUtil.Common.Service
//{

//    public class Proxy
//    {
//        public static T Invoke<T>(Expression<Func<T>> instanceFunction, BaseService service) where T : class
//        {
//            var timer = Stopwatch.StartNew();

//            var fbody = instanceFunction.Body as MethodCallExpression;

//            if (fbody?.Method == null)
//            {
//                throw new InvalidOperationException("Expression must be a method call.");
//            }

//            try
//            {
//                string methodName = fbody.Method.Name;
//                IList<object> inputs = new List<object>();
//                IList<MemberExpression> arguments = fbody.Arguments.OfType<MemberExpression>().ToList();

//                for (int argIndex = 0; argIndex < arguments.Count; argIndex++)
//                {
//                    inputs.Add(Enumerable.ToList<object>(ExpressionUtil.ExtractConstants(arguments[argIndex])));
//                }

//                object[] dynamicArgs = new object[inputs.Count];

//                if (inputs.Count > 0)
//                {
//                    dynamicArgs = new object[inputs.Count];

//                    for (int index = 0; index < dynamicArgs.Length; index++)
//                    {
//                        dynamicArgs[index] = ((List<object>)inputs[index])[0];
//                    }
//                }

//                var svc = fbody.Method.DeclaringType;
//                //BaseService a = (BaseService) svc;
//                //string _providerName = ConfigurationSettings.AppSettings["Database.ProviderName"];
//                //string _readConnectionString = ConfigurationSettings.AppSettings["Database.ReadConnectionString"];
//                //string _writeConnectionString = ConfigurationSettings.AppSettings["Database.WriteConnectionString"];
                

//                Authorized authorized = service.GetType().GetInterfaces()[0].GetMethod(methodName).GetCustomAttributes(true).FirstOrDefault(t => t.GetType() == typeof(Authorized)) as Authorized;
//                if (authorized != null && authorized.IsEnabled)
//                    CheckAuthorized();


//                Transactional transactional = service.GetType().GetInterfaces()[0].GetMethod(methodName).GetCustomAttributes(true).FirstOrDefault(t => t.GetType() == typeof(Transactional)) as Transactional;
//                var connection = DbProviderFactories.GetFactory(ApplicationContext.Current.DATABASE_PROVIDER_NAME).CreateConnection();
//                if (connection.IsNull()) throw new Exception("Cannot create DbConnection with providerName ={0}".FormatString(ApplicationContext.Current.DATABASE_PROVIDER_NAME));

//                bool isTransactionEnabled = transactional != null && transactional.IsEnabled;
                
//                object result = null;

//                if (isTransactionEnabled)
//                {
//                    connection.ConnectionString = ApplicationContext.Current.DATABASE_WRITE_CONNECTIONSTRING;
//                    using (var scope = new TransactionScope())
//                    {
//                        using (service.baseRepositoryManager = new RepositoryManager(true, connection))
//                        {
//                            result = fbody.Method.Invoke(service, dynamicArgs);
//                        }
//                        scope.Complete();
//                    }
//                }
//                else
//                {
//                    connection.ConnectionString = ApplicationContext.Current.DATABASE_READ_CONNECTIONSTRING;
//                    using (service.baseRepositoryManager = new RepositoryManager(false, connection))
//                    {
//                        result = fbody.Method.Invoke(service, dynamicArgs);
//                    }
//                }
//                timer.Stop();
//                if (timer.ElapsedMilliseconds > 5000)
//                {
//                    //_log.Debug("Long query in "+methodName+" took {0} ms",timer.ElapsedMillseconds);
//                }
//                return result as T;
//            }
//            finally
//            {
//                LogAudit();
//            }
//        }
//        private static void LogAudit()
//        {

//        }
//        private static void CheckAuthorized()
//        {

//        }

//    }
//}
