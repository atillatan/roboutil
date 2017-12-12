using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RoboUtil
{
    partial class Utils
    {
        public static class Regexp
        {
            internal static Dictionary<RegexType, string> extraktors = new Dictionary<RegexType, string>
            {
                [RegexType.UrlExtractor] = @"(?:href\s*=)(?:[\s""']*)(?!#|mailto|location.|javascript|.*css|.*this\.)(?<url>.*?)(?:[\s>""'])",
                [RegexType.SrcExtractor] = @"(?:src\s*=)(?:[\s""']*)(?<url>.*?)(?:[\s>""'])"
            };

            private static readonly ConcurrentDictionary<RegexType, Regex> RegexTypeRegex = new ConcurrentDictionary<RegexType, Regex>();

            public static Regex GetRegEx(RegexType type)
            {
                Regex rx;
                if (RegexTypeRegex.TryGetValue(type, out rx)) return rx;

                rx = new Regex(extraktors[type], RegexOptions.IgnoreCase);
                RegexTypeRegex[type] = rx;
                return rx;
            }
        }

        /// <summary>
        /// Get a match object based on a predefined regular expression
        /// </summary>
        /// <param name="regularExpressionId">Id of the regular expression to return</param>
        /// <param name="text">Text to match on</param>
        /// <returns>Match</returns>
        public static Match GetMatchRegEx(this string text, RegexType regexType)
        {
            return Regexp.GetRegEx(regexType).Match(text);
        }

        public enum RegexType
        {
            UrlExtractor = 1,
            SrcExtractor = 2
        }
    }
}