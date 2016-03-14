using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace RoboUtil.utils
{
    public static class ExpandoObjectMapper
    {
        public static T Map<T>(dynamic obj)
        {
            T result = default(T);
            IDictionary<string, dynamic> objectProperties = obj as IDictionary<string, dynamic>;
            if (default(T) is ValueType || (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>)))
            {
                result = (T)obj;
            }
            else
            {
                Type t = typeof(T);
                T instance = (T)t.GetConstructor(System.Type.EmptyTypes).Invoke(null);
                foreach (var item in objectProperties)
                {
                    DynamicMap(item, instance, t);
                }
                result = instance;
            }
            return result;
        }
        public static List<T> Map<T>(List<dynamic> list)
        {
            List<T> result = new List<T>();

            if (default(T) is ValueType || typeof(T) == typeof(String) || (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>)))
            {
                foreach (var item in list)
                {
                    result.Add(item);
                }
            }
            else
            {
                Type t = typeof(T);

                foreach (var item in list)
                {
                    T instance = (T)t.GetConstructor(System.Type.EmptyTypes).Invoke(null);
                    IDictionary<string, dynamic> objectProperties = item as IDictionary<string, dynamic>;
                    foreach (var prop in objectProperties)
                    {
                        DynamicMap(prop, instance, t);
                    }
                    result.Add(instance);
                }
            }
            return result;
        }
        private static void DynamicMap(KeyValuePair<string, object> prop, dynamic instance, Type t)
        {
            PropertyInfo fi = t.GetProperty(prop.Key);
            if (fi != null)
            {
                if (fi.PropertyType.UnderlyingSystemType.Namespace == "System" || prop.Value == null)
                {
                    fi.SetValue(instance, prop.Value);
                }
                else
                {
                    object ins = Activator.CreateInstance(fi.PropertyType);
                    fi.SetValue(instance, ins);
                    foreach (var p in (prop.Value as IDictionary<string, dynamic>))
                    {
                        DynamicMap(p, ins, ins.GetType());
                    }
                }
            }

        }
        public static List<T> ToMap<T>(this List<dynamic> list)
        {
            return Map<T>(list);
        }
    }
}
