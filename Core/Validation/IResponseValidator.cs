using Rublon.Sdk.Core.Rest;
using Rublon.Sdk.Core.Signature;
using System;

namespace Rublon.Sdk.Core.Validation
{
    public interface IResponseValidator
    {
        void validateResponse();

        RESTClient RestClient { get; set; }

        String SecretKey { get; set; }
    }
}