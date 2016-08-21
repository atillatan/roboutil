using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace RoboUtil.managers.thread
{
 
    public class Dispatcher
    {
        public static object Invoke<TResponse>(Expression<Func<TResponse>> instanceFunction, object instance)
        {
            var fbody = instanceFunction.Body as MethodCallExpression;

            if (fbody == null || fbody.Method == null)
            {
                throw new InvalidOperationException("Expression must be a method call.");
            }

            try
            {
                string methodName = fbody.Method.Name;
                IList<object> managerInputs = new List<object>();
                IList<MemberExpression> arguments = fbody.Arguments.OfType<MemberExpression>().ToList();

                for (int argIndex = 0; argIndex < arguments.Count; argIndex++)
                {
                    managerInputs.Add(Dispatcher.ExtractConstants(arguments[argIndex]).ToList());
                }

                object[] dynamicArgs = new object[managerInputs.Count]; ;

                if (managerInputs.Count > 0)
                {
                    dynamicArgs = new object[managerInputs.Count];

                    for (int index = 0; index < dynamicArgs.Length; index++)
                    {
                        dynamicArgs[index] = ((List<object>)managerInputs[index])[0];
                    }
                }

                return fbody.Method.Invoke(instance, dynamicArgs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public static IEnumerable<object> ExtractConstants<T>(Expression<Action<T>> expression)
        {
            var lambdaExpression = expression as LambdaExpression;
            if (lambdaExpression == null)
            {
                throw new InvalidOperationException("Please provide a lambda expression.");
            }
            var methodCallExpression = lambdaExpression.Body as MethodCallExpression;
            if (methodCallExpression == null)
            {
                throw new InvalidOperationException("Please provide a *method call* lambda expression.");
            }
            return ExtractConstants(methodCallExpression);
        }

        public static IEnumerable<object> ExtractConstants(Expression expression)
        {
            if (expression == null || expression is ParameterExpression) return new object[0];

            var memberExpression = expression as MemberExpression;
            if (memberExpression != null) return ExtractConstants(memberExpression);

            var constantExpression = expression as ConstantExpression;
            if (constantExpression != null) return ExtractConstants(constantExpression);

            var newArrayExpression = expression as NewArrayExpression;
            if (newArrayExpression != null) return ExtractConstants(newArrayExpression);

            var newExpression = expression as NewExpression;
            if (newExpression != null) return ExtractConstants(newExpression);

            var unaryExpression = expression as UnaryExpression;
            if (unaryExpression != null) return ExtractConstants(unaryExpression);

            return new object[0];
        }

        #region Example Dispatcher Usage
        //public static void Main()
        //{
        //    DoWork dw = new DoWork();
        //    Dispatcher.Invoke(() => dw.Run("test"), dw);
        //}

        //public class DoWork
        //{
        //    public string Run(string param)
        //    {
        //        string result = "";

        //        for (int cycles = 0; cycles < 5; cycles++)
        //        {
        //            Console.Write("? ");
        //        }
        //        return result;
        //    }
        //} 
        #endregion

     
    }
}
