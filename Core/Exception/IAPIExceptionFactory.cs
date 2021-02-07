using Rublon.Sdk.Core.Rest;

namespace Rublon.Sdk.Core.Exception
{
    public interface IAPIExceptionFactory
    {
        APIException createException(RESTClient client);
    }
}
