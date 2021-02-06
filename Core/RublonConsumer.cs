using Rublon.Sdk.Core.Exception;

namespace Rublon.Sdk.Core
{
    /// <summary>
    /// Rublon Consumer abstract class.
    /// The main class provides common methods for all Rublon services.
    /// In order for the class to work properly, it must be initiated with two parameters: System Token and the Secret Key.
    /// Both of parameters can be obtained from developer dashboard at developers.rublon.com.
    /// </summary>
    public class RublonConsumer
    {
        /// <summary>
        /// API version number.
        /// </summary>
        public const string VERSION = "3.8.0";

        /// <summary>
        /// API version date.
        /// </summary>
        public const string VERSION_DATE = "2015-09-10";

        /// <summary>
        /// Default API server.
        /// </summary>
        //public const string DEFAULT_API_SERVER = "https://lo-frmcode.rublon.com";
        public const string DEFAULT_API_SERVER = "https://core.rublon.net";

        /// <summary>
        /// Library target platform.
        /// </summary>
        public const string PLATFORM = ".NET";

        /// <summary>
        /// Default language code.
        /// </summary>
        public const string DEFAULT_LANG = "en";

        protected string systemToken;
        protected string secretKey;
        protected string serviceName;
        protected string apiServer;
        protected string languge;
        protected string currentUrl;
        protected bool userCanConfigure;

        /// <summary>
        /// Initialize RublonConsumer.
        /// </summary>
        /// <param name="systemToken">Consumer's system token string.</param>
        /// <param name="secretKey">Consumer's secret key string.</param>
        public RublonConsumer(string systemToken, string secretKey)
            : this(systemToken, secretKey, DEFAULT_API_SERVER)
        {
        }

        /// <summary>
        /// Initialize RublonConsumer.
        /// </summary>
        /// <param name="systemToken">Consumer's system token string.</param>
        /// <param name="secretKey">Consumer's secret key string.</param>
        /// <param name="apiServer">API server's URI</param>
        public RublonConsumer(string systemToken, string secretKey, string apiServer)
        {
            this.systemToken = systemToken;
            this.secretKey = secretKey;
            this.apiServer = apiServer;
        }

        /// <summary>
        /// Test whether the service is configured and throw an exception if not.
        /// </summary>
        /// <exception cref="RublonSDK.Core.Exception.RublonException.ConfigurationException">Throws if service is not configured</exception>
        public void TestConfiguration()
        {
            if (!IsConfigured())
            {
                throw new RublonException.ConfigurationException();
            }
        }

        /// <summary>
        /// Check whether service is configured.
        /// </summary>
        /// <returns></returns>
        public bool IsConfigured()
        {
            return !string.IsNullOrEmpty(this.systemToken) && !string.IsNullOrEmpty(this.secretKey);
        }

        /// <summary>
        /// Get secret key.
        /// </summary>
        public string SystemToken
        {
            get { return systemToken; }
        }

        /// <summary>
        /// Get system token.
        /// </summary>
        public string SecretKey
        {
            get { return secretKey; }
        }

        /// <summary>
        /// Get the service name.
        /// </summary>
        public string ServiceName
        {
            get { return serviceName; }
        }

        /// <summary>
        /// Get Rublon API server.
        /// </summary>
        public string APIServer
        {
            get { return apiServer; }
        }

        /// <summary>
        /// Get or set language code.
        /// Set 2-letter language code compliant with <a href="https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes">ISO 639-1</a>.
        /// </summary>
        public string Language
        {
            get { return languge; }
            set { languge = value; }
        }

        /// <summary>
        /// Get or set current URL address.
        /// </summary>
        public string CurrentUrl
        {
            get { return currentUrl; }
            set { currentUrl = value; }
        }

        /// <summary>
        /// Get or set flag whether the current user can configure the Rublon system token and secret key.
        /// </summary>
        public bool UserCanConfigure
        {
            get { return userCanConfigure; }
            set { userCanConfigure = value; }
        }
    }
}
