using Newtonsoft.Json.Linq;
using Rublon;
using Rublon.Sdk.Core;
using System.Diagnostics;

namespace Rublon.Sdk.TwoFactor
{
    public class BeginTransaction : APIMethod
    {
        /// <summary>
        /// Field name for the web URI in the API response.
        /// </summary>
        public const string FIELD_WEB_URI = "webURI";

        /// <summary>
        /// API request URI path.
        /// </summary>
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
        /// Get the web URI from the API response and redirect the user's web browser.
        /// </summary>
        /// <returns></returns>
        public string GetWebURI()
        {
            return responseResult.Value<string>(FIELD_WEB_URI);
        }

        /// <summary>
        /// Get the API request URL.
        /// </summary>
        /// <returns></returns>
        protected override string getUrl()
        {
            return rublon.APIServer + REQUEST_URI_PATH;
        }

        /// <summary>
        /// Get the API request parameters.
        /// </summary>
        /// <returns></returns>
        protected override JObject getParams()
        {
            var baseParameters = base.getParams();
            var parameters = new JObject(consumerParams);
            parameters.Merge(baseParameters);
            parameters.Add(RublonAuthParams.FIELD_USER_ID, userId);
            parameters.Add(RublonAuthParams.FIELD_CALLBACK_URL, callbackUrl);
            parameters.Add(RublonAuthParams.FIELD_USER_EMAIL, userEmail.ToLower());

            return parameters;
        }
    }
}
