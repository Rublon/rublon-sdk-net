using Newtonsoft.Json.Linq;

namespace Rublon.Sdk.Core
{
    /// <summary>
    /// Parameters wrapper of the Rublon authentication process.
    /// 
    /// This class is used to prepare the parameters for the authentication
    /// process. This includes both the parameters used for the authentication
    /// itself as well as any additional parameters that would be used by the
    /// integrated website in the callback. An object of this class can also
    /// be used to embed the authentication parameters in a Rublon button.
    /// </summary>
    public class RublonAuthParams
    {
        /// <summary>
        /// Field name for access token parameter.
        /// </summary>
        public const string FIELD_ACCESS_TOKEN = "accessToken";

        /// <summary>
        /// Field name for required Rublon user's profile ID.
        /// </summary>
        public const string FIELD_REQUIRE_PROFILE_ID = "requireProfileId";

        /// <summary>
        /// Name of the field with profile ID.
        /// </summary>
        public const string FIELD_PROFILE_ID = "profileId";

        /// <summary>
        /// Field name for "service" parameter
        /// </summary>
        public const string FIELD_SERVICE = "service";

        /// <summary>
        /// Field name for "systemToken" parameter
        /// </summary>
        public const string FIELD_SYSTEM_TOKEN = "systemToken";

        /// <summary>
        /// Field name for language parameter
        /// </summary>
        public const string FIELD_LANG = "lang";

        /// <summary>
        /// Field name for origin URL.
        /// </summary>
        public const string FIELD_ORIGIN_URL = "originUrl";

        /// <summary>
        /// Field name for return URL.
        /// </summary>
        public const string FIELD_RETURN_URL = "returnUrl";

        /// <summary>
        /// Field name for consumer parameters
        /// </summary>
        public const string FIELD_CONSUMER_PARAMS = "consumerParams";

        /// <summary>
        /// Field name for action parameter.
        /// </summary>
        public const string FIELD_ACTION = "action";

        /// <summary>
        /// Field name for action flag.
        /// </summary>
        public const string FIELD_ACTION_FLAG = "actionFlag";

        /// <summary>
        /// Field name for callback URL.
        /// </summary>
        public const string FIELD_CALLBACK_URL = "callbackUrl";

        /// <summary>
        /// Field name for local user ID.
        /// </summary>
        public const string FIELD_USER_ID = "appUserId";

        /// <summary>
        /// Field name for local user email hash address.
        /// </summary>
        public const string FIELD_USER_EMAIL_HASH = "userEmailHash";

        /// <summary>
        /// Field name for local user email address.
        /// </summary>
        public const string FIELD_USER_EMAIL = "userEmail";

        /// <summary>
        /// Field name for logout listener boolean flag.
        /// </summary>
        public const string FIELD_LOGOUT_LISTENER = "logoutListener";

        /// <summary>
        /// Field name for version parameter.
        /// </summary>
        public const string FIELD_VERSION = "version";

        /// <summary>
        /// Field name for version date parameter.
        /// </summary>
        public const string FIELD_VERSION_DATE = "versionDate";

        /// <summary>
        /// Field name to require Rublon to authenticate
        /// by mobile app only, not using Email 2-factor.
        /// </summary>
        public const string FIELD_FORCE_MOBILE_APP = "forceMobileApp";

        /// <summary>
        /// Field name to force ignoring the existing Trusted Device
        /// during the authentication.
        /// </summary>
        public const string FIELD_IGNORE_TRUSTED_DEVICE = "ignoreTrustedDevice";

        /// <summary>
        /// Field name to add a custom URI query parameter to the callback URL.
        /// </summary>
        public const string FIELD_CUSTOM_URI_PARAM = "customURIParam";

        /// <summary>
        /// Field name to define a message for a transaction.
        /// </summary>
        public const string FIELD_CONFIRM_MESSAGE = "confirmMessage";

        /// <summary>
        /// Field name to set the time buffer in seconds from previous confirmation
        /// which allow Rublon to confirm the custom transaction
        /// without user's action.
        /// </summary>
        public const string FIELD_CONFIRM_TIME_BUFFER = "confirmTimeBuffer";

        /// <summary>
        /// URL path to authentication code
        /// </summary>
        public const string URL_PATH_CODE = "/code/native/";

        protected RublonConsumer rublon;
        protected JObject consumerParams = new JObject();
        protected JObject outerParams = new JObject();
        protected string originUrl = "http://rublon.com/";
        protected string actionFlag;

        /// <summary>
        /// Initialize object with RublonService instance.
        /// A RublonService class instance is required for
        /// the object to work.
        /// </summary>
        /// <param name="rublon">An instance of the RublonService class</param>
        public RublonAuthParams(RublonConsumer rublon)
        {
            this.rublon = rublon;
        }
    }
}
