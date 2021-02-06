﻿using Newtonsoft.Json.Linq;
using Rublon.Sdk.Core.Logging;
using Rublon.Sdk.Core.Rest;
using Rublon.Sdk.Core.Validation;

namespace Rublon.Sdk.Core
{
    public abstract class APIMethod
    {       
        public const string FIELD_RESULT = "result";

        protected RublonConsumer rublon;
        protected string rawResponseBody;
        protected string responseStatus;
        protected JObject responseResult;

        public RESTClient RestClient { get; set; }

        public IRublonLogger Logger { get; set; } = new NullLogger();

        public IResponseValidator ResponseValidator { get; set; } = new DefaultResponseValidator();

        /// <summary>
        /// Construct the API method instance.
        /// </summary>
        /// <param name="rublon"></param>
        public APIMethod(RublonConsumer rublon)
        {
            this.rublon = rublon;
            RestClient = new RESTClient(rublon.SecretKey);
        }

        /// <summary>
        /// Perform HTTP request.
        /// </summary>
        public void Perform()
        {
            var rawPostBody = string.Empty;
            var parameters = getParams();
            if (parameters != null && parameters.Count > 0)
            {
                rawPostBody = parameters.ToString();
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
            
            responseResult = JObject.Parse(this.rawResponseBody).Value<JObject>(FIELD_RESULT);
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
        protected virtual JObject getParams()
        {
            var parameters = new JObject();
            parameters.Add(RublonAuthParams.FIELD_SYSTEM_TOKEN, rublon.SystemToken);
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
