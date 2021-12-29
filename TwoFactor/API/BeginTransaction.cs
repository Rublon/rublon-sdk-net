using Newtonsoft.Json.Linq;
using Rublon.Sdk.Core;
using System;

namespace Rublon.Sdk.TwoFactor
{
    public class BeginTransaction : APIMethod
    {
        
        public const string FIELD_WEB_URI = "webURI";

        public const string FIELD_CALLBACK_URL = "callbackUrl";

        public const string FIELD_USER_EMAIL = "userEmail";
        
        public const string REQUEST_URI_PATH = "/api/transaction/init";

        protected string callbackUrl;
        protected string userEmail;
        protected string userId;
        protected JObject additionalParams;

        /// <summary>
        /// Construct the API method instance.
        /// </summary>
        /// <param name="rublon">Rublon instance.</param>
        /// <param name="callbackUrl">URL of the callback method.</param>
        /// <param name="userEmail">User's email address.</param>
        /// <param name="userName">User's local ID.</param>
        public BeginTransaction(Rublon rublon, string callbackUrl, string userEmail, string userName)
            : this(rublon, callbackUrl, userEmail, userName, new JObject())
        {

        }

        /// <summary>
        /// Construct the API method instance.
        /// </summary>
        /// <param name="rublon">Rublon instance.</param>
        /// <param name="callbackUrl">URL of the callback method.</param>
        /// <param name="userEmail">User's email address.</param>
        /// <param name="userName">User's local ID.</param>
        /// <param name="additionalParams">Additional transaction parameters.</param>
        public BeginTransaction(IRublon rublon, string callbackUrl, string userEmail, string userName, JObject additionalParams)
            : base(rublon)
        {
            this.callbackUrl = callbackUrl;
            this.userEmail = userEmail;
            this.userId = userName;
            this.additionalParams = additionalParams;
        }

        /// <summary>
        /// Gets the web URI to the Rublon prompt from the API response. 
        /// </summary>
        /// <returns></returns>
        public string GetWebURI()
        {
            return methodCallResponse.Value<string>(FIELD_WEB_URI);
        }

        protected override string getUrl()
        {
            return rublon.APIServer + REQUEST_URI_PATH;
        }

        protected override JObject prepareRequestBody()
        {
            var baseParameters = base.prepareRequestBody();
            var parameters = new JObject(additionalParams);
            parameters.Merge(baseParameters);
            parameters.Add(RublonCommonParams.USERNAME_FIELD, userId);
            parameters.Add(FIELD_CALLBACK_URL, callbackUrl);
            addUserEmailIfNotEmpty(parameters);            
            return parameters;
        }

        private void addUserEmailIfNotEmpty(JObject parameters)
        {
            if (!String.IsNullOrWhiteSpace(userEmail))
            {
                parameters.Add(FIELD_USER_EMAIL, userEmail.ToLower());
            }
        }
    }
}
