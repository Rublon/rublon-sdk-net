using Rublon;
using Rublon.Sdk.Core.Exception;
using Rublon.Sdk.Core.Signature;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace Rublon.Sdk.Core.Rest
{
    public class RESTClient
    {
        
        /// <summary>
        /// Connection timeout in seconds.
        /// </summary>
        public const int TIMEOUT = 30;

        /// <summary>
        /// User agent string.
        /// </summary>
        public const string USER_AGENT = "rublon-net-sdk";

        /// <summary>
        /// Value of the "Content-Type" HTTP header
        /// </summary>
        public const string HEADER_VALUE_CONTENT_TYPE = "Content-Type: application/json";

        /// <summary>
        /// Value of the "Accept" HTTP header
        /// </summary>
        public const string HEADER_VALUE_ACCEPT = "Accept: application/json, text/javascript, */*; q=0.01";

        /// <summary>
        /// Name of the custom HTTP header to send the library's version
        /// </summary>
        public const string HEADER_NAME_VERSION = "X-Rublon-API-Version";

        /// <summary>
        /// Name of the custom HTTP header to send the library's version date
        /// </summary>
        public const string HEADER_NAME_VERSION_DATE = "X-Rublon-API-Version-Date";

        /// <summary>
        /// Name of the custom HTTP heaader to send the library's platform
        /// </summary>
        public const string HEADER_NAME_PLATFORM = "X-Rublon-Technology";

        /// <summary>
        /// Name of the custom HTTP header to send the library's technology
        /// </summary>
        public const string HEADER_NAME_SIGNATURE = "X-Rublon-Signature";                    


        private HttpWebRequest httpRequest;        
        protected string rawResponse;
        protected HttpWebResponse response;
        private string secretKey;

        public RublonMessageSigner RublonMessageSigner
        {
            get;
            set;
        } = new RublonMessageSigner();

        /// <summary>
        /// Construct REST client instance
        /// </summary>
        /// <param name="rublon">RublonConsumer instance</param>
        public RESTClient(string secretKey)
        {
            this.secretKey = secretKey;
        }

        public RESTClient() : this("") { }

        /// <summary>
        /// Perform the request
        /// </summary>
        /// <param name="url">Connection URL address</param>
        /// <param name="rawPostBody">Raw POST body</param>
        /// <returns>Raw HTTP response body</returns>
        public virtual string PerformRequest(string url, string rawPostBody)
        {
            setupHTTPRequest(url,rawPostBody);
            rawResponse = string.Empty;
            try
            {
                using (var requestStream = httpRequest.GetRequestStream())
                {
                    var bodyBytes = Encoding.UTF8.GetBytes(rawPostBody);
                    requestStream.Write(bodyBytes, 0, bodyBytes.Length);
                }
                response = (HttpWebResponse)httpRequest.GetResponse();
                rawResponse = readWholeRawResponse(response);                
                response.Close();

            }catch (System.Net.WebException ex)
            {
                if (ex.Response != null)
                {
                    response = (HttpWebResponse)ex.Response;
                    rawResponse = readWholeRawResponse(response);
                    response.Close();
                }
                else
                {
                    throw new ConnectionException("Web error occurred while connecting to the Core", ex);
                }
            }catch(System.Exception ex)
            {
                throw new ConnectionException("Error occurred while connecting to the Core", ex);
            }
            return rawResponse;
        }

        protected void setupHTTPRequest(string url, string rawPostBody)
        {
            httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.ContentType = HEADER_VALUE_CONTENT_TYPE;
            httpRequest.Accept = HEADER_VALUE_ACCEPT;
            httpRequest.UserAgent = USER_AGENT;
            httpRequest.Headers.Add(HEADER_NAME_VERSION, RublonConsumer.VERSION);
            httpRequest.Headers.Add(HEADER_NAME_VERSION, RublonConsumer.VERSION);
            httpRequest.Headers.Add(HEADER_NAME_VERSION_DATE, RublonConsumer.VERSION_DATE);
            httpRequest.Headers.Add(HEADER_NAME_PLATFORM, RublonConsumer.PLATFORM);
            var signature = RublonMessageSigner.Sign(rawPostBody, this.secretKey);
            httpRequest.Headers.Add(HEADER_NAME_SIGNATURE, signature);
            httpRequest.Timeout = TIMEOUT * 1000;
            httpRequest.Method = "POST";
        }

        private string readWholeRawResponse(HttpWebResponse response)
        {
            var stream = response.GetResponseStream();
            var reader = new StreamReader(stream);
            var rawResponseContent = reader.ReadToEnd();

            reader.Close();
            stream.Close();
            return rawResponseContent;
        }
        
        /// <summary>
        /// Get the HTTP response status code.
        /// </summary>
        /// <returns></returns>
        public virtual HttpStatusCode GetHTTPStatusCode()
        {            
            return response.StatusCode;
        }

        /// <summary>
        /// Get the Rublon signature header from response.
        /// </summary>
        /// <returns></returns>
        public string GetSignature()
        {
            var headers = response.Headers.GetValues(HEADER_NAME_SIGNATURE);
            if (headers.Length > 0)
            {
                return headers[0];
            }

            return null;
        }

        /// <summary>
        /// Get the raw response body string.
        /// </summary>
        virtual public string RawResponse
        {
            get { return rawResponse; }
        }
    }
}
