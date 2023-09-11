using Rublon.Sdk.Core.Exception;

namespace Rublon.Sdk.Core
{
    /// <summary>   
    /// The main class which provides common methods for all Rublon services.
    /// In order for the class to work properly, it must be initiated with two parameters: System Token and the Secret Key.
    /// Both of parameters can be obtained from developer dashboard at developers.rublon.com.
    /// </summary>
    public class RublonConsumer : IRublonConsumer
    {

        /// <summary>
        /// Default API server.
        /// </summary>
        public const string DEFAULT_API_SERVER = "https://staging-core.rublon.net";

        /// <summary>
        /// Default language code.
        /// </summary>
        public const string DEFAULT_LANG = "en";

        protected string systemToken;
        protected string secretKey;

        /// <summary>
        /// Initializes RublonConsumer.
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
            this.APIServer = apiServer;
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
        /// <returns>true if configured, false otherwise</returns>
        public bool IsConfigured()
        {
            return !string.IsNullOrEmpty(this.systemToken) && !string.IsNullOrEmpty(this.secretKey);
        }

        /// <summary>
        /// Get system token.
        /// </summary>
        public string SystemToken
        {
            get { return systemToken; }
        }

        /// <summary>
        /// Get secret key.
        /// </summary>
        public string SecretKey
        {
            get { return secretKey; }
        }

        /// <summary>
        /// Get Rublon API server.
        /// </summary>
        public string APIServer
        {
            get;
            private set;
        }

        /// <summary>
        /// Get or set language code.
        /// Set 2-letter language code compliant with <a href="https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes">ISO 639-1</a>.
        /// </summary>
        public string Language
        {
            get;
            set;
        }
    }
}
