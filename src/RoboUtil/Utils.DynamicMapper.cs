using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Linq;

using System.Reflection;
using System.Threading.Tasks;

namespace RoboUtil
{
    static partial class Utils
    {
        public static class DynamicMapper
        {
            private static void DynamicMap(KeyValuePair<string, object> prop, dynamic instance, Type t)
            {
                //PropertyInfo fi = t.GetProperty(prop.Key);

                PropertyInfo fi = Reflections.TypePropertiesCache(t).Where(p => p.Name == prop.Key).SingleOrDefault();

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

            internal static List<T> ToMap<T>(List<dynamic> list)
            {
                throw new NotImplementedException();
            }
        }

        public static T Map<T>(this object obj)
        {
            return Utils.DynamicMapper.Map<T>(obj);
        }

        public static List<T> Map<T>(this List<dynamic> list)
        {
            return DynamicMapper.Map<T>(list);
        }

        /// <summary>
        /// Turns the object into an ExpandoObject
        /// </summary>
        public static dynamic ToExpando(this object o)
        {
            var result = new ExpandoObject();
            var d = result as IDictionary<string, object>; //work with the Expando as a Dictionary
            if (o.GetType() == typeof(ExpandoObject)) return o; //shouldn't have to... but just in case
            if (o.GetType() == typeof(NameValueCollection) || o.GetType().IsSubclassOf(typeof(NameValueCollection)))
            {
                var nv = (NameValueCollection)o;
                nv.Cast<string>().Select(key => new KeyValuePair<string, object>(key, nv[key])).ToList().ForEach(i => d.Add(i));
            }
            else
            {
                var props = o.GetType().GetProperties();
                foreach (var item in props)
                {
                    d.Add(item.Name, item.GetValue(o, null));
                }
            }
            return result;
        }
    }
}