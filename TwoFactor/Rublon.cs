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
        /// <summary>
        /// Service name.
        /// </summary>
        public const string SERVICE_NAME = "2factor";

        public IRublonLogger Logger { get; set; } = new NullLogger();
        /// <summary>
        /// Construct an instance.
        /// </summary>
        /// <param name="systemToken">Consumer's system token string.</param>
        /// <param name="secretKey">Consumer's secret key string.</param>
        public Rublon(string systemToken, string secretKey)
            : this(systemToken, secretKey, DEFAULT_API_SERVER)
        {
        }

        /// <summary>
        /// Construct an instance.
        /// </summary>
        /// <param name="systemToken">Consumer's system token string.</param>
        /// <param name="secretKey">Consumer's secret key string.</param>
        /// <param name="apiServer">API server's URI</param>
        public Rublon(string systemToken, string secretKey, string apiServer)
            : base(systemToken, secretKey, apiServer)
        {
            this.serviceName = SERVICE_NAME;
        }

        /// <summary>
        /// Initializes the Rublon 2-factor authentication transaction
        /// and returns the URL address to redirect user's browser
        /// or NULL if user's account is not protected.
        ///
        /// First, method checks the account's protection status in the Rublon server for current user.
        /// If user has protected this account, method returns the URL address.
        /// Redirect user's browser to this URL to start the Rublon authentication process.
        ///
        /// If Rublon user has deleted his Rublon account or Rublon API is not available at this time,
        /// method returns null. If so, just bypass Rublon and sign in the user.
        ///
        /// Notice: to use this method the configurations values (system token and secret key)
        /// must be provided to the constructor. If not, function will throw an exception.
        /// </summary>
        /// <param name="callbackUrl">Callback URL address.</param>
        /// <param name="userId">User's ID in local system.</param>
        /// <param name="userEmail">User's email address.</param>
        /// <returns></returns>
        public string Authorize(string callbackUrl, string userId, string userEmail)
        {
            return Authorize(callbackUrl, userId, userEmail, new JObject());
        }

        /// <summary>
        /// Initializes the Rublon 2-factor authentication transaction
        /// and returns the URL address to redirect user's browser
        /// or NULL if user's account is not protected.
        ///
        /// First, method checks the account's protection status in the Rublon server for current user.
        /// If user has protected this account, method returns the URL address.
        /// Redirect user's browser to this URL to start the Rublon authentication process.
        ///
        /// If Rublon user has deleted his Rublon account or Rublon API is not available at this time,
        /// method returns null. If so, just bypass Rublon and sign in the user.
        ///
        /// Notice: to use this method the configurations values (system token and secret key)
        /// must be provided to the constructor. If not, function will throw an exception.
        /// </summary>
        /// <param name="callbackUrl">Callback URL address.</param>
        /// <param name="userId">User's ID in local system.</param>
        /// <param name="userEmail">User's email address.</param>
        /// <param name="consumerParams">Additional transaction parameters.</param>
        /// <returns></returns>
        public string Authorize(string callbackUrl, string userId, string userEmail, JObject consumerParams)
        {
            TestConfiguration();

            if (!string.IsNullOrEmpty(this.languge))
            {
                consumerParams.Add(RublonAuthParams.FIELD_LANG, languge);
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
            consumerParams.Add(RublonAuthParams.FIELD_CONFIRM_MESSAGE, confirmMessage);
            if (!string.IsNullOrEmpty(languge))
            {
                consumerParams.Add(RublonAuthParams.FIELD_LANG, languge);
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
            consumerParams.Add(RublonAuthParams.FIELD_CONFIRM_TIME_BUFFER, timeBuffer);
            return Confirm(callbackUrl, userId, userEmail, confirmMessage, consumerParams);
        }

        /// <summary>
        /// Authenticate user and get user's credentials using one-time use access token.
        /// 
        /// One-time use access token is a session identifier which will be deleted after first usage.
        /// This method can be called only once in authentication process.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public Credentials GetCredentials(string accessToken)
        {
            var credentials = new Credentials(this, accessToken);
            credentials.Logger = Logger;
            credentials.Perform();
            return credentials;
        }
    }
}
