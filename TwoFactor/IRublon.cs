using Newtonsoft.Json.Linq;
using Rublon.Sdk.Core;
using Rublon.Sdk.Core.Logging;

namespace Rublon.Sdk.TwoFactor
{
    public interface IRublon : IRublonConsumer
    {
        IRublonLogger Logger { get; set; }

        string Auth(string callbackUrl, string userId, string userEmail);
        string Auth(string callbackUrl, string userId, string userEmail, JObject consumerParams);
        Credentials GetCredentials(string accessToken);
    }
}