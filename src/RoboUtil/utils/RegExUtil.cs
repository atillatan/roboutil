using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RoboUtil.utils
{
    public class RegExUtil
    {
        public static bool Match(string value, string pattern)
        {
            Regex regex = new Regex(pattern);
            return regex.IsMatch(value);
        }

        public static string RxReplace(string str, string pattern, string value)
        {
            return Regex.Replace(str, pattern, value);
        }
        public static string RxRemove(string str, string pattern)
        {
            return Regex.Replace(str, pattern, "");
        }

        #region UrlRewrting   
        private static Hashtable _RegExp;

        public const int URL_EXTRACTOR = 1;
        public const int SRC_EXTRACTOR = 2;

        private const string STR_URL_EXTRACTOR = @"(?:href\s*=)(?:[\s""']*)(?!#|mailto|location.|javascript|.*css|.*this\.)(?<url>.*?)(?:[\s>""'])";
        private const string STR_SRC_EXTRACTOR = @"(?:src\s*=)(?:[\s""']*)(?<url>.*?)(?:[\s>""'])";

    

        /// <summary>
        /// Get a predefined regular expression
        /// </summary>
        /// <param name="regularExpressionId">Id of the regular expression to return
        /// </param>
        /// <returns>RegEx</returns>
        public static Regex GetRegEx(int regularExpressionId)
        {
            if (_RegExp == null)
            {
                _RegExp = new Hashtable();
            }

            if (_RegExp[regularExpressionId] == null)
            {
                _RegExp[regularExpressionId] = StandardRegularExpression(regularExpressionId);
            }

            return (Regex)_RegExp[regularExpressionId];
        }

        /// <summary>
        /// Get a match object based on a predefined regular expression
        /// </summary>
        /// <param name="regularExpressionId">Id of the regular expression to return</param>
        /// <param name="text">Text to match on</param>
        /// <returns>Match</returns>
        public static Match GetMatchRegEx(int regularExpressionId, string text)
        {
            return GetRegEx(regularExpressionId).Match(text);
        }

        private  static Regex StandardRegularExpression(int regularExpressionId)
        {
            switch (regularExpressionId)
            {
                case URL_EXTRACTOR:
                    {  
                        return new Regex(STR_URL_EXTRACTOR, RegexOptions.IgnoreCase);
                    }
                case SRC_EXTRACTOR:
                    {
                        return new Regex(STR_SRC_EXTRACTOR, RegexOptions.IgnoreCase);
                    }
            }

            return null;
        }
        #endregion



    }
}
