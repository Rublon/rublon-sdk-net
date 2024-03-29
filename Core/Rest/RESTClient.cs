﻿using Rublon.Sdk.Core.Exception;
using Rublon.Sdk.Core.Signature;
using Rublon.Sdk.TwoFactor;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Rublon.Sdk.Core.Rest
{
    public class RESTClient
    {
        private const string EVENT_LOG = "Rublon";
        private const string EVENT_SOURCE = "Rublon";
        private static EventLog _eventLog;

        public const int TIMEOUT = 30;

        public const string USER_AGENT = "rublon-net-sdk";

        public const string HEADER_VALUE_CONTENT_TYPE = "Content-Type: application/json";

        public const string HEADER_VALUE_ACCEPT = "Accept: application/json, text/javascript, */*; q=0.01";

        public const string HEADER_NAME_VERSION = "X-Rublon-API-Version";

        public const string HEADER_NAME_VERSION_DATE = "X-Rublon-API-Version-Date";

        public const string HEADER_NAME_PLATFORM = "X-Rublon-Technology";

        public const string HEADER_NAME_SIGNATURE = "X-Rublon-Signature";

        /// <summary>
        /// API version number.
        /// </summary>
        public const string VERSION = "3.8.0";

        /// <summary>
        /// API version date.
        /// </summary>
        public const string VERSION_DATE = "2015-09-10";

        /// <summary>
        /// Library target platform.
        /// </summary>
        public const string PLATFORM = ".NET";

        public HttpWebRequest httpRequest;        
        private string rawResponse;
        private HttpWebResponse response;
        private string secretKey;
        private string proxyHost;
        private int proxyPort;
        private string proxyUser;
        private string proxyPassword;
        private int proxyMode;

        public RublonMessageSigner RublonMessageSigner
        {
            get;
            set;
        } = new RublonMessageSigner();

        /// <summary>
        /// Constructs REST client instance with <paramref name="secretKey"/> which will be used to sign the message.
        /// </summary>
        /// <param name="secretKey">secret key</param>        
        public RESTClient(string secretKey, string proxyHost, int proxyPort, string proxyUser, string proxyPassword, int proxyMode)
        {
            this.secretKey = secretKey;
            this.proxyHost = proxyHost;
            this.proxyPort = proxyPort;
            this.proxyUser = proxyUser;
            this.proxyPassword = proxyPassword;
            this.proxyMode = proxyMode;
        }

        public RESTClient() : this("", "", 0, "", "", 0) {  }

      



        /// <summary>
        /// Performs the request
        /// </summary>
        /// <param name="url">Connection URL</param>
        /// <param name="rawPostBody">Raw POST body (json)</param>
        /// <returns>Raw HTTP response body (json)</returns>
        /// <exception cref="ConnectionException">When the problem with connecting to the Rublon server occurred</exception>
        public virtual string PerformRequest(string url, string rawPostBody)
        {
            setupHTTPRequest(url,rawPostBody);
            ApplyProxy();
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

        private void ApplyProxy()
        {
            //ProxySettings proxySettings = new ProxySettingsProvider().LoadSettings();
            if (proxyMode == 1)
            {
                if (!string.IsNullOrEmpty(proxyHost) && proxyPort != 0)
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    WebProxy proxy = new WebProxy(string.Format("{0}:{1}", proxyHost, proxyPort));

                    if (!string.IsNullOrEmpty(proxyUser))
                    {

                        ICredentials credentials = new NetworkCredential(proxyUser, proxyPassword);
                        proxy.Credentials = credentials;
                    }
                    httpRequest.Proxy = proxy;
                }
            }
        }

        protected void setupHTTPRequest(string url, string rawPostBody)
        {
            httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.ContentType = HEADER_VALUE_CONTENT_TYPE;
            httpRequest.Accept = HEADER_VALUE_ACCEPT;
            httpRequest.UserAgent = USER_AGENT;
            httpRequest.Headers.Add(HEADER_NAME_VERSION, VERSION);
            httpRequest.Headers.Add(HEADER_NAME_VERSION_DATE, VERSION_DATE);
            httpRequest.Headers.Add(HEADER_NAME_PLATFORM, PLATFORM);
            var signature = RublonMessageSigner.SignData(rawPostBody, this.secretKey);
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
        /// HTTP response status code. Usually 200 (OK) or 400 (bad request - which means API Exception)
        /// </summary>
        /// <returns>performed request status code</returns>
        public virtual HttpStatusCode GetHTTPStatusCode()
        {            
            return response.StatusCode;
        }

        /// <summary>
        /// Get the Rublon signature header from response.
        /// </summary>
        /// <returns>Rublon Signature header</returns>
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
        /// Get the raw response body string. Which can be parsed and used to get some action specific parameters
        /// </summary>
        virtual public string RawResponse
        {
            get { return rawResponse; }
        }
    }
}
