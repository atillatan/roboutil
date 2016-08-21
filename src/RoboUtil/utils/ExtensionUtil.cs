using System;
using System.Collections.Generic;
using System.Linq;
using RoboUtil.utils;

namespace RoboUtil
{
    /// <summary>
    /// usefull extension methods are composed in this class
    /// </summary>
    public static class ExtensionUtil
    {
        #region Base Util
        public static bool IsNull(this object obj)
        {
            return GeneralUtil.IsNull(obj);
        }
        public static bool IsEmpty(this object obj)
        {
            return GeneralUtil.IsEmpty(obj);
        }
        public static bool IsNullOrEmpty(this object obj)
        {
            return GeneralUtil.IsNullOrEmpty(obj);
        }
        public static string FormatString(this string format, params object[] args)
        {
            return string.Format(format, args);
        }
        public static bool HasValue(this string s)
        {
            return !string.IsNullOrEmpty(s);
        }
        #endregion

        #region Converting util
        public static T Convert<T>(this IConvertible obj)
        {
            return GeneralUtil.Convert<T>(obj);
        }
        public static T To<T>(this IConvertible obj)
        {
            return Convert<T>(obj);
        }
        public static string ToStringValue(this object obj)
        {
            return GeneralUtil.ToStringValue(obj);
        }
        public static int? ToIntValue(this object obj)
        {
            return GeneralUtil.ToIntValue(obj);
        }
        public static long ToInt16(this string value)
        {
            Int16 result = 0;

            if (!string.IsNullOrEmpty(value))
                Int16.TryParse(value, out result);

            return result;
        }
        public static long ToInt32(this string value)
        {
            Int32 result = 0;

            if (!string.IsNullOrEmpty(value))
                Int32.TryParse(value, out result);

            return result;
        }
        public static long ToInt64(this string value)
        {
            Int64 result = 0;

            if (!string.IsNullOrEmpty(value))
                Int64.TryParse(value, out result);

            return result;
        }
        #endregion

        #region Enum util
        public static bool Has<T>(this System.Enum type, T value)
        {
            try
            {
                return (((int)(object)type & (int)(object)value) == (int)(object)value);
            }
            catch
            {
                return false;
            }
        }
        public static bool Is<T>(this System.Enum type, T value)
        {
            try
            {
                return (int)(object)type == (int)(object)value;
            }
            catch
            {
                return false;
            }
        }
        public static T Add<T>(this System.Enum type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type | (int)(object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    string.Format(
                        "Could not append value from enumerated type '{0}'.",
                        typeof(T).Name
                        ), ex);
            }
        }
        public static T Remove<T>(this System.Enum type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type & ~(int)(object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    string.Format(
                        "Could not remove value from enumerated type '{0}'.",
                        typeof(T).Name
                        ), ex);
            }
        }
        #endregion

        #region RegExUtil
        public static bool Match(this string value, string pattern)
        {
            return RegExUtil.Match(value, pattern);
        }
        public static string RxReplace(this string str, string pattern, string value)
        {
            return RegExUtil.RxReplace(str, pattern, value);
        }
        public static string RxRemove(this string str, string pattern)
        {
            return RegExUtil.RxReplace(str, pattern, "");
        }

        #endregion

        #region For Collection
        public static void ForEach<T>(this IEnumerable<T> @enum, Action<T> mapFunction)
        {
            foreach (var item in @enum) mapFunction(item);
        }
        public static bool ExistIn<T>(this T source, params T[] list)
        {
            if (null == source) throw new ArgumentNullException("source");
            return list.Contains(source);
        }
        public static bool Between<T>(this T actual, T lower, T upper) where T : IComparable<T>
        {
            //myNumber.Between(3,7)
            return actual.CompareTo(lower) >= 0 && actual.CompareTo(upper) < 0;
        }
        #endregion

        #region Logging
        public static void Log(this Exception obj)
        {
            //your logging logic here
            //usage = ex.Log(); //Default LogLevel is "Debug"
        }
        public static void Log(this Exception obj,string level)
        {
            //your logging logic here
            //usage = ex.Log(Loglevel.Error);
        }
        #endregion

        #region Mapping   
       
        #endregion
    }
}
