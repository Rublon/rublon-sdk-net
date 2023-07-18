using Newtonsoft.Json.Linq;
using Rublon.Sdk.Core.Logging;
using Rublon.Sdk.Core.Rest;
using Rublon.Sdk.Core.Validation;
using System;

namespace Rublon.Sdk.Core
{
    public abstract class APIMethod
    {       
        public const string FIELD_RESULT = "result";
        public const string FIELD_SYSTEM_TOKEN = "systemToken";

        protected IRublonConsumer rublon;
        protected string rawResponseBody;
        protected string responseStatus;
        protected JObject methodCallResponse;

        public static class RublonCommonParams
        {
            public const string USERNAME_FIELD = "username";
            public const string FIELD_USER_ID = "appUserId";
        }

        public RESTClient RestClient { get; set; }

        public IRublonLogger Logger { get; set; } = new NullLogger();

        public IAPIResponseValidator ResponseValidator { get; set; } = new DefaultAPIResponseValidator();

        /// <summary>
        /// Construct the API method instance.
        /// </summary>
        /// <param name="rublon"></param>
        public APIMethod(IRublonConsumer rublon)
        {
            this.rublon = rublon;
            RestClient = new RESTClient(rublon.SecretKey, rublon.ProxyHost, rublon.ProxyPort, rublon.ProxyUsername, rublon.ProxyPassword);
        }

        /// <summary>
        /// Performs the method HTTP request.
        /// </summary>
        public void Perform()
        {
            var rawPostBody = string.Empty;
            var requestBodyJSON = prepareRequestBody();
            if (requestBodyJSON != null && requestBodyJSON.Count > 0)
            {
                rawPostBody = requestBodyJSON.ToString();
            }

            var url = getUrl();
            Logger.log(
                    string.Format("Starting request to core: url: {0}, {1}", url, rawPostBody)
                );

            this.rawResponseBody = RestClient.PerformRequest(url, rawPostBody);
            Logger.log(
                    string.Format("Got response from core: {0}", this.rawResponseBody)
                );
            validateReponse();
            
            methodCallResponse = JObject.Parse(this.rawResponseBody).Value<JObject>(FIELD_RESULT);
        }

        /// <summary>
        /// Get the API request's URL.
        /// </summary>
        /// <returns></returns>
        protected abstract string getUrl();

        /// <summary>
        /// Get the API request's parameters object.
        /// </summary>
        /// <returns></returns>
        protected virtual JObject prepareRequestBody()
        {
            var parameters = new JObject();
            if (rublon.SystemToken == "")
            {
                parameters.Add(FIELD_SYSTEM_TOKEN, "A49F2206561C4FEBBFEEE178A2ED1FAA");
            }
            else
                parameters.Add(FIELD_SYSTEM_TOKEN, rublon.SystemToken);
            return parameters;
        }

        /// <summary>
        /// Validate the API response.
        /// </summary>
        protected void validateReponse()
        {
            ResponseValidator.RestClient = this.RestClient;
            ResponseValidator.SecretKey = this.rublon.SecretKey;
            ResponseValidator.validateResponse();
        }
       
    }
}
