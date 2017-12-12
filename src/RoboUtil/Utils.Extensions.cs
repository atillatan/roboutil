using Newtonsoft.Json;
using System;

using System.Collections;
using System.Collections.Generic;

using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;

using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;

using System.Text.RegularExpressions;

using System.Threading;
using System.Xml;

using System.Xml.Linq;
using System.Xml.Serialization;

namespace RoboUtil
{
    partial class Utils
    {
        #region Base Util

        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        public static bool IsEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool HasValue(this string s)
        {
            return !string.IsNullOrEmpty(s);
        }

        public static bool IsEmptyString(this object obj)
        {
            string strVal = obj as string;
            if (strVal != null) return string.IsNullOrEmpty(strVal);

            string str = "";
            try
            {
                str = System.Convert.ToString(obj);
            }
            catch (Exception)
            {
                throw new Exception("object is not a string!");
            }
            return string.IsNullOrEmpty(str);
        }

        public static string FormatString(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        public static bool IsEqualTo<T>(this T actual, T expected)
        {
            return actual.Equals(expected);
        }

        public static bool IsNumeric(this object o)
        {
            return IsNumeric(o.GetType());
        }

        public static bool IsNumeric(this Type type)
        {
            if (type == typeof(Int16) ||
                type == typeof(Int32) ||
                type == typeof(Int64) ||
                type == typeof(Decimal) ||
                type == typeof(Double) ||
                type == typeof(Byte) ||
                type == typeof(SByte) ||
                type == typeof(UInt16) ||
                type == typeof(UInt32) ||
                type == typeof(UInt64) ||
                type == typeof(Single) ||
                type == typeof(float)
                )
            {
                return true;
            }
            return false;
        }

        #endregion Base Util

        #region Converting util

        public static T Convert<T>(this IConvertible obj)
        {
            return (T)System.Convert.ChangeType(obj, typeof(T));
        }

        public static T To<T>(this IConvertible obj)
        {
            return Convert<T>(obj);
        }

        public static string ToStringValue(this object obj)
        {
            if (!IsNull(obj))
                return System.Convert.ToString(obj);
            return string.Empty;
        }

        public static int? ToIntValue(this object obj)
        {
            if (!IsNull(obj))
                return int.Parse(ToStringValue(obj));
            return null;
        }

        public static Int16 ToInt16(this string value)
        {
            Int16 result = 0;

            if (!string.IsNullOrEmpty(value))
                Int16.TryParse(value, out result);

            return result;
        }

        public static Int32 ToInt32(this string value)
        {
            Int32 result = 0;

            if (!string.IsNullOrEmpty(value))
                Int32.TryParse(value, out result);

            return result;
        }

        public static Int64 ToInt64(this string value)
        {
            Int64 result = 0;

            if (!string.IsNullOrEmpty(value))
                Int64.TryParse(value, out result);

            return result;
        }

        #endregion Converting util

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

        #endregion Enum util

        #region RegExUtil

        public static bool Match(this string value, string pattern)
        {
            Regex regex = new Regex(pattern);
            return regex.IsMatch(value);
        }

        public static string RxReplace(this string str, string pattern, string value)
        {
            return Regex.Replace(str, pattern, value);
        }

        public static string RxRemove(this string str, string pattern)
        {
            return Regex.Replace(str, pattern, "");
        }

        #endregion RegExUtil

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

        #endregion For Collection

        #region Logging

        public static void Log(this Exception obj)
        {
            //your logging logic here
            //usage = ex.Log(); //Default LogLevel is "Debug"
        }

        public static void Log(this Exception obj, string level)
        {
            //your logging logic here
            //usage = ex.Log(Loglevel.Error);
        }

        #endregion Logging

        #region Reflection

        /// <summary>
        /// DateTime now = DateTime.Now;
        /// int min = GetPropValue<int>(now, "TimeOfDay.Minutes");
        /// int hrs = now.GetPropValue<int>("TimeOfDay.Hours");
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetPropValue(this object obj, String name)
        {
            foreach (String part in name.Split('.'))
            {
                if (obj == null) { return null; }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null) { return null; }

                obj = info.GetValue(obj, null);
            }
            return obj;
        }

        public static T GetPropValue<T>(this object obj, String name)
        {
            object retval = GetPropValue(obj, name);
            if (retval == null) { return default(T); }

            // throws InvalidCastException if types are incompatible
            return (T)retval;
        }

        public static bool SetProperty(object obj, string propertyName, object properyValue)
        {
            if (obj.GetType().GetProperty(propertyName) != null)
            {
                obj.GetType().GetProperty(propertyName).SetValue(obj, properyValue);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsInterface(this Type type)
        {
#if COREFX
                    return type.GetTypeInfo().IsInterface;
#else
            return type.IsInterface;
#endif
        }

#if !COREFX

        public static bool IsCollection(this object obj)
        {
            if (obj.GetType().IsPrimitive) return false;
            return obj.GetType().GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }

        public static bool IsEnumerable(this object obj)
        {
            if (obj.GetType().IsPrimitive) return false;

            return obj.GetType().GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }

#endif

        public static bool IsEnumerable(this PropertyInfo property)
        {
            var type = property.PropertyType;
            return typeof(string) != type && typeof(IEnumerable).IsAssignableFrom(type);
        }

        #endregion Reflection

        #region Exception

        /// <summary>
        ///  Provides full stack trace for the exception that occurred.
        /// </summary>
        /// <param name="exception">Exception object.</param>
        /// <param name="environmentStackTrace">Environment stack trace, for pulling additional stack frames.</param>
        public static string ToLogString(this Exception exception, string environmentStackTrace)
        {
            List<string> environmentStackTraceLines = GetUserStackTraceLines(environmentStackTrace);
            environmentStackTraceLines.RemoveAt(0);

            List<string> stackTraceLines = GetStackTraceLines(exception.StackTrace);
            stackTraceLines.AddRange(environmentStackTraceLines);

            string fullStackTrace = String.Join(Environment.NewLine, stackTraceLines);

            string logMessage = exception.Message + Environment.NewLine + fullStackTrace;
            return logMessage;
        }

        /// <summary>
        ///  Gets a list of stack frame lines, as strings.
        /// </summary>
        /// <param name="stackTrace">Stack trace string.</param>
        private static List<string> GetStackTraceLines(string stackTrace)
        {
            return stackTrace.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
        }

        /// <summary>
        ///  Gets a list of stack frame lines, as strings, only including those for which line number is known.
        /// </summary>
        /// <param name="fullStackTrace">Full stack trace, including external code.</param>
        private static List<string> GetUserStackTraceLines(string fullStackTrace)
        {
            List<string> outputList = new List<string>();
            Regex regex = new Regex(@"([^\)]*\)) in (.*):line (\d)*$");

            List<string> stackTraceLines = GetStackTraceLines(fullStackTrace);
            foreach (string stackTraceLine in stackTraceLines)
            {
                if (!regex.IsMatch(stackTraceLine))
                {
                    continue;
                }

                outputList.Add(stackTraceLine);
            }

            return outputList;
        }

        #endregion Exception
    }
}