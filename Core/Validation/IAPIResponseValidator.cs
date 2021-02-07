using Rublon.Sdk.Core.Rest;
using System;

namespace Rublon.Sdk.Core.Validation
{
    public interface IAPIResponseValidator
    {
        void validateResponse();

        RESTClient RestClient { get; set; }

        String SecretKey { get; set; }
    }
}