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
        /// Sign given data with secret key and returns a signature
        /// </summary>
        /// <param name="data">Raw string data to sign</param>
        /// <param name="secretKey">Secret key</param>
        /// <returns>signature of the data</returns>
        public string SignData(string data, string secretKey)
        {
            return HmacSHA256(data, secretKey);
        }

        /// <summary>
        /// Verifies whether <paramref name="signature"/> for <paramref name="data"/> is valid.
        /// </summary>
        /// <param name="data">data</param>
        /// <param name="secretKey">Secret key used to create the signature</param>
        /// <param name="signature">Signature to verify</param>
        /// <returns>true if signature is valid false otherwise</returns>
        public bool VerifySignatureForData(string data, string secretKey, string signature)
        {
            var dataSign = SignData(data, secretKey);
            return dataSign == signature;
        }
        
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
