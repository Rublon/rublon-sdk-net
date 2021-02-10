using Newtonsoft.Json.Linq;
using Rublon.Sdk.Core;

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
        protected JObject consumerParams;

        /// <summary>
        /// Construct the API method instance.
        /// </summary>
        /// <param name="rublon">Rublon instance.</param>
        /// <param name="callbackUrl">URL of the callback method.</param>
        /// <param name="userEmail">User's email address.</param>
        /// <param name="userId">User's local ID.</param>
        public BeginTransaction(Rublon rublon, string callbackUrl, string userEmail, string userId)
            : this(rublon, callbackUrl, userEmail, userId, new JObject())
        {

        }

        /// <summary>
        /// Construct the API method instance.
        /// </summary>
        /// <param name="rublon">Rublon instance.</param>
        /// <param name="callbackUrl">URL of the callback method.</param>
        /// <param name="userEmail">User's email address.</param>
        /// <param name="userId">User's local ID.</param>
        /// <param name="consumerParams">Additional transaction parameters.</param>
        public BeginTransaction(Rublon rublon, string callbackUrl, string userEmail, string userId, JObject consumerParams)
            : base(rublon)
        {
            this.callbackUrl = callbackUrl;
            this.userEmail = userEmail;
            this.userId = userId;
            this.consumerParams = consumerParams;
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

        protected override JObject getParams()
        {
            var baseParameters = base.getParams();
            var parameters = new JObject(consumerParams);
            parameters.Merge(baseParameters);
            parameters.Add(RublonCommonParams.FIELD_USER_ID, userId);
            parameters.Add(FIELD_CALLBACK_URL, callbackUrl);
            parameters.Add(FIELD_USER_EMAIL, userEmail.ToLower());

            return parameters;
        }
    }
}
