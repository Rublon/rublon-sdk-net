using Newtonsoft.Json.Linq;
using Rublon.Sdk.Core;
using Rublon.Sdk.Core.Logging;

namespace Rublon.Sdk.TwoFactor
{
    /// <summary>
    /// Rublon 2-factor service provider class.
    /// </summary>
    public class Rublon : RublonConsumer, IRublon
    {
        public const string FIELD_LANG = "lang";

        public IRublonLogger Logger { get; set; } = new NullLogger();

        public Rublon(string systemToken, string secretKey, string proxyHost = "", int proxyPort = 0, string proxyUsername = "", string proxyPassword = "")
            : this(systemToken, secretKey, DEFAULT_API_SERVER, proxyHost, proxyPort, proxyUsername, proxyPassword)
        {
        }

        public Rublon(string systemToken, string secretKey, string apiServer, string proxyHost = "", int proxyPort = 0, string proxyUsername = "", string proxyPassword = "")
            : base(systemToken, secretKey, apiServer, proxyHost, proxyPort, proxyUsername, proxyPassword)
        {
        }

        public virtual string Auth(AuthenticationParameters authenticationParameters)
        {
            return Auth(
                    authenticationParameters.CallbackUrl,
                    authenticationParameters.Username,
                    authenticationParameters.UserEmail,
                    authenticationParameters.AdditionalParams
                );
        }
        
        private string Auth(string callbackUrl, string userName, string userEmail = "", JObject consumerParams = null)
        {
            TestConfiguration();
            if (consumerParams == null)
            {
                consumerParams = new JObject();
            }
            if (!string.IsNullOrEmpty(Language))
            {
                consumerParams.Add(FIELD_LANG, Language);
            }

            var beginTransaction = new BeginTransaction(this, callbackUrl, userEmail, userName, consumerParams);
            beginTransaction.Logger = Logger;
            beginTransaction.Perform();
            return beginTransaction.GetWebURI();
        }
        
        public Credentials GetCredentials(string accessToken)
        {
            var credentials = new Credentials(this, accessToken);
            credentials.Logger = Logger;
            credentials.Perform();
            return credentials;
        }

    }
}
