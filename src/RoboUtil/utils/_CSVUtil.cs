//using System.Collections.Generic;
//using System.Text;

//namespace RoboUtil.utils
//{
//    public class CsvUtil
//    {
//        public static string ToCSV<T>(IEnumerable<T> collection, string delim)
//        {
//            if (collection == null)
//            {
//                return "";
//            }

//            StringBuilder result = new StringBuilder();
//            foreach (T value in collection)
//            {
//                result.Append(value);
//                result.Append(delim);
//            }
//            if (result.Length > 0)
//            {
//                result.Length -= delim.Length;
//            }
//            return result.ToString();
//        }
//    }
//}