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
        protected string proxyHost;
        protected int proxyPort;
        protected string proxyUser;
        protected string proxyPassword;
        protected int proxyMode;

        /// <summary>
        /// Initializes RublonConsumer.
        /// </summary>
        /// <param name="systemToken">Consumer's system token string.</param>
        /// <param name="secretKey">Consumer's secret key string.</param>
        public RublonConsumer(string systemToken, string secretKey, string proxyHost, int proxyPort, string proxyUser, string proxyPassword, int proxyMode)
            : this(systemToken, secretKey, DEFAULT_API_SERVER, proxyHost, proxyPort, proxyUser, proxyPassword, proxyMode)
        {
        }

        /// <summary>
        /// Initialize RublonConsumer.
        /// </summary>
        /// <param name="systemToken">Consumer's system token string.</param>
        /// <param name="secretKey">Consumer's secret key string.</param>
        /// <param name="apiServer">API server's URI</param>
        public RublonConsumer(string systemToken, string secretKey, string apiServer, string proxyHost, int proxyPort, string proxyUser, string proxyPassword, int proxyMode)
        {
            this.systemToken = systemToken;
            this.secretKey = secretKey;
            this.APIServer = apiServer;
            this.proxyHost = proxyHost;
            this.proxyPort = proxyPort;
            this.proxyUser = proxyUser;
            this.proxyPassword = proxyPassword;
            this.proxyMode = proxyMode;
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

        public string ProxyHost { get { return this.proxyHost; } }
        public int ProxyPort { get { return this.proxyPort; } }
        public string ProxyUser { get { return this.proxyUser; } }
        public string ProxyPassword { get { return this.proxyPassword; } }
        public int ProxyMode { get { return this.proxyMode; } }

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
