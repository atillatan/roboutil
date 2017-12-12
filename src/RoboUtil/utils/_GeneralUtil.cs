//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;

////using System.Runtime.Serialization.Formatters.Binary;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Xml;
//using System.Xml.Serialization;

//namespace RoboUtil.utils
//{
//    public class GeneralUtil
//    {
//        #region Base Util

//        //public static bool IsNull(object obj)
//        //{
//        //    return obj == null;
//        //}

//        //public static bool IsEmpty(object obj)
//        //{
//        //    if (obj is string)
//        //    {
//        //        return obj.ToString().Equals("");
//        //    }
//        //    else
//        //    {
//        //        //try to convert strong
//        //        string a = "";
//        //        try
//        //        {
//        //            a = Convert<string>(obj);
//        //        }
//        //        catch (Exception)
//        //        {
//        //            throw new Exception("object is not a string!");
//        //        }
//        //        return a.Trim().Equals("");
//        //    }
//        //}

//        //public static bool IsNullOrEmpty(object obj)
//        //{
//        //    if (IsNull(obj) || IsEmpty(obj))
//        //    {
//        //        return true;
//        //    }
//        //    return false;
//        //}

//        public static void ThrowIfArgumentIsNull<T>(T obj, string parameterName) where T : class
//        {
//            if (obj == null) throw new ArgumentNullException(parameterName + " not allowed to be null");
//        }

//        #endregion Base Util

//        #region Converting util

//        //public static T Convert<T>(object obj)
//        //{
//        //    return (T)System.Convert.ChangeType(obj, typeof(T));
//        //}

//        //public static string ToStringValue(object obj)
//        //{
//        //    if (!GeneralUtil.IsNull(obj))
//        //    {
//        //        return obj.ToString();
//        //    }
//        //    else
//        //    {
//        //        return "";
//        //    }
//        //}

//        //public static int? ToIntValue(object obj)
//        //{
//        //    if (!GeneralUtil.IsNull(obj))
//        //    {
//        //        return int.Parse(ToStringValue(obj));
//        //    }
//        //    else
//        //    {
//        //        return null;
//        //    }
//        //}

//        //public static long ToInt16(string value)
//        //{
//        //    Int16 result = 0;

//        //    if (!string.IsNullOrEmpty(value))
//        //        Int16.TryParse(value, out result);

//        //    return result;
//        //}

//        //public static long ToInt32(string value)
//        //{
//        //    Int32 result = 0;

//        //    if (!string.IsNullOrEmpty(value))
//        //        Int32.TryParse(value, out result);

//        //    return result;
//        //}

//        //public static long ToInt64(string value)
//        //{
//        //    Int64 result = 0;

//        //    if (!string.IsNullOrEmpty(value))
//        //        Int64.TryParse(value, out result);

//        //    return result;
//        //}

//        #endregion Converting util

//        #region String Util

//        public static string ToTitleCase(string mText)
//        {
//            if (mText == null) return mText;

//            System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
//            System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;

//            // TextInfo.ToTitleCase only operates on the string if is all lower case, otherwise it returns the string unchanged.
//            return textInfo.ToTitleCase(mText.ToLower());
//        }

//        /// <summary>
//        /// seperate to words from compsed string
//        /// </summary>
//        /// <param name="camelCaseWord"></param>
//        /// <returns></returns>
//        public static string Wordify(string camelCaseWord)
//        {
//            // if the word is all upper, just return it
//            if (!Regex.IsMatch(camelCaseWord, "[a-z]"))
//                return camelCaseWord;

//            return string.Join(" ", Regex.Split(camelCaseWord, @"(?<!^)(?=[A-Z])"));
//            // "TableColumnName".Capitalize().Wordify() => Table Clumn Name
//        }

//        /// <summary>
//        /// Convert to upper first letter
//        /// </summary>
//        /// <param name="word"></param>
//        /// <returns></returns>
//        public static string Capitalize(string word)
//        {
//            return word[0].ToString().ToUpper() + word.Substring(1);
//        }

//        #endregion String Util

//        public static bool IsCollection(object obj)
//        {
//            return obj.GetType().GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICollection<>));
//        }

//        public static bool SetProperty(object obj, string propertyName, object properyValue)
//        {
//            if (obj.GetType().GetProperty(propertyName) != null)
//            {
//                obj.GetType().GetProperty(propertyName).SetValue(obj, properyValue);
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        public static object GetProperty(object obj, string propertyName)
//        {
//            if (obj.GetType().GetProperty(propertyName) != null)
//            {
//                return obj.GetType().GetProperty(propertyName).GetValue(obj);
//            }
//            else
//            {
//                return null;
//            }
//        }

//        public static T OneOf<T>(Random rng, params T[] things)
//        {
//            //string babyName = rand.OneOf("John", "George", "Radio XBR74 ROCKS!");
//            return things[rng.Next(things.Length)];
//        }

//        /// <summary>Serializes an object of type T in to an xml string</summary>
//        /// <typeparam name="T">Any class type</typeparam>
//        /// <param name="obj">Object to serialize</param>
//        /// <returns>A string that represents Xml, empty otherwise</returns>
//        public static string XmlSerialize<T>(T obj) where T : class, new()
//        {
//            if (obj == null) throw new ArgumentNullException("obj");

//            var serializer = new XmlSerializer(typeof(T));
//            using (var writer = new StringWriter())
//            {
//                serializer.Serialize(writer, obj);
//                return writer.ToString();
//            }
//        }

//        /// <summary>Deserializes an xml string in to an object of Type T</summary>
//        /// <typeparam name="T">Any class type</typeparam>
//        /// <param name="xml">Xml as string to deserialize from</param>
//        /// <returns>A new object of type T is successful, null if failed</returns>
//        public static T XmlDeserialize<T>(string xml) where T : class, new()
//        {
//            if (xml == null) throw new ArgumentNullException("xml");

//            var serializer = new XmlSerializer(typeof(T));
//            using (var reader = new StringReader(xml))
//            {
//                try { return (T)serializer.Deserialize(reader); }
//                catch { return null; } // Could not be deserialized to this type.
//            }
//        }

//        public static string ToJson(object value)
//        {
//            string returnValue = null;
//            if (value != null)
//            {
//                returnValue = JsonConvert.SerializeObject(value);
//                returnValue = returnValue == "[]" ? null : returnValue;
//            }
//            return returnValue;
//        }

//        public static object FromJson(string value)
//        {
//            return JsonConvert.DeserializeObject<object>(value);
//        }

//        public static T FromJson<T>(string value)
//        {
//            return JsonConvert.DeserializeObject<T>(value);
//        }

//        public static string JsonString(XmlDocument xmlDoc)
//        {
//            StringWriter sw = new StringWriter();
//            XmlTextWriter tx = new XmlTextWriter(sw);
//            xmlDoc.WriteTo(tx);
//            string strXmlText = sw.ToString();
//            return strXmlText;
//        }

//        //public static string ExceptionToString(Exception exception)
//        //{
//        //    StringBuilder exceptionBuilder = new StringBuilder();
//        //    while (exception != null)
//        //    {
//        //        exceptionBuilder.AppendLine(exception.Message);
//        //        exception = exception.InnerException;
//        //    }
//        //    return exceptionBuilder.ToString();
//        //}

//        //public static T Clone<T>(T obj) where T : ICloneable
//        //{
//        //    return (T)obj.Clone();
//        //}
//        //public static IList<T> Clone<T>(IList<T> listToClone) where T : ICloneable
//        //{
//        //    return listToClone.Select(item => (T)item.Clone()).ToList();
//        //}

//        //netstatndart library
//        //public static T DeepCopy<T>(object objectToCopy) where T : class
//        //{
//        //    if (!typeof(T).IsSerializable)
//        //    {
//        //        throw new ArgumentException("The type must be serializable.", "source");
//        //    }

//        //    // Don't serialize a null object, simply return the default for that object
//        //    if (Object.ReferenceEquals(objectToCopy, null))
//        //    {
//        //        return default(T);
//        //    }

//        //    using (MemoryStream memoryStream = new MemoryStream())
//        //    {
//        //        BinaryFormatter binaryFormatter = new BinaryFormatter();
//        //        binaryFormatter.Serialize(memoryStream, objectToCopy);
//        //        memoryStream.Seek(0, SeekOrigin.Begin);
//        //        return (T)binaryFormatter.Deserialize(memoryStream);
//        //    }
//        //}

//        private static void Loop<T>(T item, Action<T> action, int iterations = 1000000)
//        {
//            for (int i = 0; i < iterations; i++)
//            {
//                action(item);
//            }
//        }
//    }
//}