using Newtonsoft.Json.Linq;

namespace Rublon.Sdk.TwoFactor
{
    /// <summary>
    /// Authentication parameters used during transaction initialisation (transaction init).
    /// </summary>
    public class AuthenticationParameters
    {
        /// <summary>
        /// Url which Rublon will redirect browser, when authentication is finished
        /// </summary>
        public string CallbackUrl {
            get;
            set;
        }

        /// <summary>
        /// Username of a user who is authenticated
        /// </summary>
        public string Username {
            get; set;
        }

        /// <summary>
        /// Email of a user who is authenticated, this is optional
        /// </summary>
        public string UserEmail
        {
            get;
            set;
        } = "";

        /// <summary>
        /// Additional parameters send to the transation init request
        /// </summary>
        public JObject AdditionalParams { get; set; } = new JObject();
    }
}