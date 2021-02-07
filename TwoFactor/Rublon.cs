using Newtonsoft.Json.Linq;
using Rublon.Sdk.Core;
using Rublon.Sdk.Core.Logging;

namespace Rublon.Sdk.TwoFactor
{
    /// <summary>
    /// Rublon 2-factor service provider class.
    /// </summary>
    public class Rublon : RublonConsumer
    {

        public const string FIELD_CONFIRM_MESSAGE = "confirmMessage";

        public const string FIELD_LANG = "lang";

        public const string FIELD_CONFIRM_TIME_BUFFER = "confirmTimeBuffer";

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
        public string Authorize(string callbackUrl, string userId, string userEmail)
        {
            return Authorize(callbackUrl, userId, userEmail, new JObject());
        }

        /// <summary>
        /// Works similar as <see cref="Authorize(string, string, string)"/> but allows for adding additional so called consumer params.       
        /// </summary>
        /// <param name="callbackUrl">Callback URL address.</param>
        /// <param name="userId">User's ID in local system.</param>
        /// <param name="userEmail">User's email address.</param>
        /// <param name="consumerParams">Additional transaction parameters. The class <seealso cref="ConsumerParamsBuilder"/> is a builder which can be used to build proper consumer params.</param>
        /// <returns>web URI to Rublon prompt for the created transaction</returns>
        public string Authorize(string callbackUrl, string userId, string userEmail, JObject consumerParams)
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
        /// Authenticate user and perform an additional confirmation of the transaction.
        /// 
        /// This method requires user to use the Rublon mobile app
        /// (even if the Trusted Device is available)
        /// and confirm transaction to maintain higher security level.
        /// 
        /// For users which are using the email-2-factor method, the question
        /// will be displayed in the web browser after clicking the confirmation
        /// link sent to the user's email address.
        /// 
        /// The message passed in the $customMessage argument will be displayed
        /// in the confirmation dialog.
        /// </summary>
        /// <param name="callbackUrl">Callback URL address.</param>
        /// <param name="userId">User's ID in local system.</param>
        /// <param name="userEmail">User's email address.</param>
        /// <param name="confirmMessage">Message to display (max. 255 bytes).</param>
        /// <returns>URL to redirect or NULL if user is not protected.</returns>
        public string Confirm(string callbackUrl, string userId, string userEmail, string confirmMessage)
        {
            return Confirm(callbackUrl, userId, userEmail, confirmMessage, new JObject());
        }

        /// <summary>
        /// Authenticate user and perform an additional confirmation of the transaction.
        /// 
        /// This method requires user to use the Rublon mobile app
        /// (even if the Trusted Device is available)
        /// and confirm transaction to maintain higher security level.
        /// 
        /// For users which are using the email-2-factor method, the question
        /// will be displayed in the web browser after clicking the confirmation
        /// link sent to the user's email address.
        /// 
        /// The message passed in the $customMessage argument will be displayed
        /// in the confirmation dialog.
        /// </summary>
        /// <param name="callbackUrl">Callback URL address.</param>
        /// <param name="userId">User's ID in local system.</param>
        /// <param name="userEmail">User's email address.</param>
        /// <param name="confirmMessage">Message to display (max. 255 bytes).</param>
        /// <param name="consumerParams">Additional transaction parameters.</param>
        /// <returns>URL to redirect or NULL if user is not protected.</returns>
        public string Confirm(string callbackUrl, string userId, string userEmail, string confirmMessage, JObject consumerParams)
        {
            consumerParams.Add(FIELD_CONFIRM_MESSAGE, confirmMessage);
            if (!string.IsNullOrEmpty(Language))
            {
                consumerParams.Add(FIELD_LANG, Language);
            }

            return Authorize(callbackUrl, userId, userEmail, consumerParams);
        }

        /// <summary>
        /// Perform a confirmation of the transaction without user's action needed
        /// if the time buffer after previous confirmation has not been reached.
        /// 
        /// If the amount of seconds after the previous transaction is less than
        /// given time buffer, Rublon will confirm the transaction without user's action.
        /// In other cases, this method will behave the same as the Rublon2Factor.confirm() method.
        /// </summary>
        /// <param name="callbackUrl">Callback URL address.</param>
        /// <param name="userId">User's local ID.</param>
        /// <param name="userEmail">User's email address.</param>
        /// <param name="confirmMessage">Message to display (max. 255 bytes).</param>
        /// <param name="timeBuffer">Amount of seconds from last confirmation.</param>
        /// <param name="consumerParams">Additional transaction parameters.</param>
        /// <returns>URL to redirect or NULL if user is not protected.</returns>
        public string ConfirmWithBuffer(string callbackUrl, string userId, string userEmail, string confirmMessage, int timeBuffer)
        {
            return ConfirmWithBuffer(callbackUrl, userId, userEmail, confirmMessage, timeBuffer, new JObject());
        }

        /// <summary>
        /// Perform a confirmation of the transaction without user's action needed
        /// if the time buffer after previous confirmation has not been reached.
        /// 
        /// If the amount of seconds after the previous transaction is less than
        /// given time buffer, Rublon will confirm the transaction without user's action.
        /// In other cases, this method will behave the same as the Rublon2Factor.confirm() method.
        /// </summary>
        /// <param name="callbackUrl">Callback URL address.</param>
        /// <param name="userId">User's local ID.</param>
        /// <param name="userEmail">User's email address.</param>
        /// <param name="confirmMessage">Message to display (max. 255 bytes).</param>
        /// <param name="timeBuffer">Amount of seconds from last confirmation.</param>
        /// <param name="consumerParams">Additional transaction parameters.</param>
        /// <returns>URL to redirect or NULL if user is not protected.</returns>
        public string ConfirmWithBuffer(string callbackUrl, string userId, string userEmail, string confirmMessage, int timeBuffer, JObject consumerParams)
        {
            consumerParams.Add(FIELD_CONFIRM_TIME_BUFFER, timeBuffer);
            return Confirm(callbackUrl, userId, userEmail, confirmMessage, consumerParams);
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
