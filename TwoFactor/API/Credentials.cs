﻿using Newtonsoft.Json.Linq;
using Rublon.Sdk.Core;

namespace Rublon.Sdk.TwoFactor
{
    public class Credentials : APIMethod
    {
        public const string REQUEST_URI_PATH = "/api/transaction/credentials";

        /// <summary>
        /// Field name for the user's device ID.
        /// </summary>
        public const string FIELD_DEVICE_ID = "deviceId";

        public const string FIELD_ACCESS_TOKEN = "accessToken";

        protected string accessToken;

        /// <summary>
        /// Construct the API method instance.
        /// </summary>
        /// <param name="rublon">Rublon instance.</param>
        /// <param name="accessToken">Access token.</param>
        public Credentials(Rublon rublon, string accessToken)
            : base(rublon)
        {
            this.accessToken = accessToken;
        }

        /// <summary>
        /// Get the user's local ID from the response.
        /// </summary>
        /// <returns></returns>
        public string GetUserId()
        {
            return methodCallResponse.Value<string>(RublonCommonParams.FIELD_USER_ID);
        }
        
        /// <summary>
        /// Get the user's device ID from the response.
        /// </summary>
        /// <returns></returns>
        public string GetDeviceId()
        {
            return methodCallResponse.Value<string>(FIELD_DEVICE_ID);
        }        

        protected override string getUrl()
        {
            return rublon.APIServer + REQUEST_URI_PATH;
        }

        protected override JObject getParams()
        {
            var baseParameters = base.getParams();
            baseParameters.Add(FIELD_ACCESS_TOKEN, accessToken);

            return baseParameters;
        }
    }
}
