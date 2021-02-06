using System;
using System.Security.Cryptography;
using System.Text;

namespace Rublon.Sdk.Core.Signature
{
    public class RublonMessageSigner
    {     
        public RublonMessageSigner()
        {            
        }
       
        /// <summary>
        /// Sign given data with secret key and get the signature
        /// </summary>
        /// <param name="rawData">Raw string data to sign</param>
        /// <param name="secretKey">Secret key</param>
        /// <returns></returns>
        public string Sign(string rawData, string secretKey)
        {
            return HmacSHA256(rawData, secretKey);
        }

        /// <summary>
        /// Verify data by signature and secret key
        /// </summary>
        /// <param name="data">Data to sign</param>
        /// <param name="secretKey">Secret key used to create the signature</param>
        /// <param name="sign">Computed signature to verify</param>
        /// <returns></returns>
        public bool VerifyData(string data, string secretKey, string sign)
        {
            var dataSign = Sign(data, secretKey);
            return dataSign == sign;
        }

        /// <summary>
        /// Compute SHA256 hash of giver text
        /// </summary>
        /// <param name="value">Text to hash</param>
        /// <returns></returns>
        public static string SHA256(string value)
        {
            var builder = new StringBuilder();

            using (var hash = SHA256Managed.Create())
            {
                var encoding = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(encoding.GetBytes(value));

                foreach (Byte b in result)
                {
                    builder.Append(b.ToString("x2"));
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Compute HMAK hash of giver text
        /// </summary>
        /// <param name="value">Text to hash</param>
        /// <returns></returns>
        public static string HmacSHA256(string value, string key)
        {
            var builder = new StringBuilder();
            var keyBytes = new ASCIIEncoding().GetBytes(key);

            using (var hash = new HMACSHA256(keyBytes))
            {
                var encoding = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(encoding.GetBytes(value));

                foreach (Byte b in result)
                {
                    builder.Append(b.ToString("x2"));
                }
            }

            return builder.ToString();
        }
    }
}
