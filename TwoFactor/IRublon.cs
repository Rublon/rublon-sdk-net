using Newtonsoft.Json.Linq;
using Rublon.Sdk.Core;
using Rublon.Sdk.Core.Logging;

namespace Rublon.Sdk.TwoFactor
{
    public interface IRublon : IRublonConsumer
    {
        /// <summary>
        /// Provides logging feature, for debugging purposes its log method is called 
        /// before and after every call to Rublon authentication server.
        /// </summary>        
        IRublonLogger Logger { get; set; }

        /// <summary>
        /// Initializes the Rublon 2-factor authentication transaction.
        /// Returns the URL to which browser should be redirected in order to continue authentication process for the started transaction.
        /// The URL will redirect to the so called Rublon prompt.
        /// If something will fail the method will throw exception which inherits from RublonException.
        /// When authentication process is finished in Rublon prompt the process will redirect to authenticationParameters.callbackUrl 
        /// with access token and <see cref="GetCredentials(string)"/> can be called to finish authentication.
        /// Instead of get credentials you can also use <see cref="RublonCallback"/> which contains already some logic related to getting access token parameter.
        /// </summary>
        /// <param name="authenticationParameters">authentication parameters for the user</param>
        /// <returns>web URI to Rublon prompt for the created transaction</returns>
        string Auth(AuthenticationParameters authenticationParameters);

        /// <summary>
        /// Finishes authentication for a given accessToken and get user's credentials using one-time use access token.
        /// 
        /// One-time use access token is a session identifier which will be deleted after first usage.
        /// This method can be called only once in authentication process.
        /// </summary>
        /// <param name="accessToken">access token</param>
        /// <returns>Credentials</returns>
        Credentials GetCredentials(string accessToken);
    }
}