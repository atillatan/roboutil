using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboUtil
{
    partial class Utils
    {
        public static string ToCSV<T>(IEnumerable<T> collection, string delim)
        {
            if (collection == null)
            {
                return "";
            }

            StringBuilder result = new StringBuilder();
            foreach (T value in collection)
            {
                result.Append(value);
                result.Append(delim);
            }
            if (result.Length > 0)
            {
                result.Length -= delim.Length;
            }
            return result.ToString();
        }
    }
}