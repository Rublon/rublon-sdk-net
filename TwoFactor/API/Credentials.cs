using Newtonsoft.Json.Linq;
using Rublon.Sdk.Core;

namespace Rublon.Sdk.TwoFactor
{
    public class Credentials : APIMethod
    {
        public const string REQUEST_URI_PATH = "/api/transaction/credentials";
        
        public const string FIELD_ACCESS_TOKEN = "accessToken";

        protected string accessToken;

        /// <summary>
        /// Construct the API method instance.
        /// </summary>
        /// <param name="rublon">Rublon instance.</param>
        /// <param name="accessToken">Access token.</param>
        public Credentials(IRublon rublon, string accessToken)
            : base(rublon)
        {
            this.accessToken = accessToken;
        }

        /// <summary>
        /// Get the user's local ID from the response.
        /// </summary>
        /// <returns></returns>
        public virtual string GetUsername()
        {
            return methodCallResponse.Value<string>(RublonCommonParams.USERNAME_FIELD);
        }

        protected override string getUrl()
        {
            return rublon.APIServer + REQUEST_URI_PATH;
        }

        protected override JObject prepareRequestBody()
        {
            var baseParameters = base.prepareRequestBody();
            baseParameters.Add(FIELD_ACCESS_TOKEN, accessToken);

            return baseParameters;
        }
    }
}
