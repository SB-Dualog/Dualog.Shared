using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Dualog.eCatch.Shared.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNotEmpty(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        /// <summary>
        ///     Doubles curly braces so they are not interpreted as parameter placeholders for format strings.
        /// </summary>
        public static string EscapeParameters(this string str)
        {
            return str.Replace("{", "{{").Replace("}", "}}");
        }

        public static string FormatWith(this string str, params object[] args)
        {
            if (args.IsNullOrEmpty())
                return str;
            try {
                return string.Format(str, args);
            } catch (FormatException) {
                return string.Format(str, args.Select(a => EscapeParameters(a.ToString())).Cast<object>().ToArray());
            }
        }

        public static string ToUrlEncoded(this string str)
        {
            // Both WebUtility.UrlEncode and Uri.EscapeDataString behave differently depending on platform. 
            // By first encoding with UrlEncode and then replacing the resulting + with %20 (space), we get a consistent result.
            return WebUtility.UrlEncode(str).Replace("+", "%20");
        }

        public static string RemoveSlashes(this string str)
        {
            return Regex.Replace(str, @"[\\/]", string.Empty);
        }

        public static byte[] ToUtf8ByteArray(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }
       
    }
}