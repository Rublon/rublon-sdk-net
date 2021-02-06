using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Rublon.Sdk.Core
{
    public static class EncodingUtils
    {
        /// <summary>
        /// Encode string by Base64
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Base64-encoded input</returns>
        public static string Base64Encode(string input)
        {
            try
            {
                var encoding = Encoding.UTF8;
                var bytes = encoding.GetBytes(input);
                return Convert.ToBase64String(bytes);
            }
            catch (System.Exception)
            {
            }

            return string.Empty;
        }

        /// <summary>
        /// Encode string as URL component
        /// </summary>
        /// <param name="input"></param>
        /// <returns>URL-encoded string</returns>
        public static string UrlEncode(string input)
        {
            try
            {
                return HttpUtility.UrlEncode(input, Encoding.UTF8);
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Get HTML-safe string
        /// </summary>
        /// <param name="input"></param>
        /// <returns>HTML-safe string</returns>
        public static string HtmlSpecialChars(string input)
        {
            return HttpUtility.HtmlEncode(input);
        }
    }
}
