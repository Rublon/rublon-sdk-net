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

        public Rublon(string systemToken, string secretKey)
            : this(systemToken, secretKey, DEFAULT_API_SERVER)
        {
        }

        public Rublon(string systemToken, string secretKey, string apiServer)
            : base(systemToken, secretKey, apiServer)
        {
        }

        /// <summary>
        /// Initializes the Rublon 2-factor authentication transaction.
        /// Returns the URL to which browser should be redirected in order to continue authentication process for the started transaction.
        /// The URL will redirect to the so called Rublon prompt.
        /// If something will fail the method will throw exception which inherits from RublonException.
        /// When authentication process is finished in Rublon prompt the process will redirect to callbackUrl with access token and <see cref="GetCredentials(string)"/> can be called to finish authentication.
        /// Instead of get credentials you can also use <see cref="RublonCallback"/> which contains already some logic related to getting access token parameter.
        /// </summary>
        /// <param name="callbackUrl">Callback URL address.</param>
        /// <param name="userId">User's ID in local system.</param>
        /// <param name="userEmail">User's email address.</param>
        /// <returns>web URI to Rublon prompt for the created transaction</returns>
        public virtual string Auth(string callbackUrl, string userId, string userEmail)
        {
            return Auth(callbackUrl, userId, userEmail, new JObject());
        }

        /// <summary>
        /// Works similar as <see cref="Auth(string, string, string)"/> but allows for adding additional so called consumer params.       
        /// </summary>
        /// <param name="callbackUrl">Callback URL address.</param>
        /// <param name="userId">User's ID in local system.</param>
        /// <param name="userEmail">User's email address.</param>
        /// <param name="consumerParams">Additional transaction parameters. The class <seealso cref="ParamsBuilder"/> is a builder which can be used to build proper consumer params.</param>
        /// <returns>web URI to Rublon prompt for the created transaction</returns>
        public virtual string Auth(string callbackUrl, string userId, string userEmail, JObject consumerParams)
        {
            TestConfiguration();

            if (!string.IsNullOrEmpty(Language))
            {
                consumerParams.Add(FIELD_LANG, Language);
            }

            var beginTransaction = new BeginTransaction(this, callbackUrl, userEmail, userId, consumerParams);
            beginTransaction.Logger = Logger;
            beginTransaction.Perform();
            return beginTransaction.GetWebURI();
        }

        /// <summary>
        /// Finishes authentication for given accessToken and get user's credentials using one-time use access token.
        /// 
        /// One-time use access token is a session identifier which will be deleted after first usage.
        /// This method can be called only once in authentication process.
        /// </summary>
        /// <param name="accessToken">access token</param>
        /// <returns>Credentials</returns>
        public Credentials GetCredentials(string accessToken)
        {
            var credentials = new Credentials(this, accessToken);
            credentials.Logger = Logger;
            credentials.Perform();
            return credentials;
        }
    }
}
