using Newtonsoft.Json.Linq;
using Rublon.Sdk.Core.Exception;
using Rublon.Sdk.Core.Rest;
using Rublon.Sdk.Core.Signature;
using System;
using System.Net;

namespace Rublon.Sdk.Core.Validation
{
    internal class CheckApplicationAPIResponseValidator : DefaultAPIResponseValidator
    {

       
        public override void validateResponse()
        {
            assertResponseContentAndResponseCodeIsValidOrThrow();

            var status = response.Value<string>(FIELD_STATUS);
            Console.WriteLine("Status: {0}", status);
            if (string.IsNullOrEmpty(status))
            {
                throw new APIException.MissingFieldException(RestClient, FIELD_STATUS);
            }
            if (status == STATUS_OK)
            { }
            else if (status == STATUS_ERROR)
            {
                throw this.ApiExceptionFactory.createException(RestClient);
            }
            else
            {
                throw new APIException.InvalidFieldException(RestClient, "Invalid status field", status);
            }
        }

        protected override void assertResponseContentAndResponseCodeIsValidOrThrow()
        {
            if (RestClient.RawResponse == null)
            {
                throw new APIException(RestClient, "Empty response body.");
            }

            response = JObject.Parse(RestClient.RawResponse);
            if (response == null && response.Count == 0)
            {
                throw new APIException.InvalidJSONException(RestClient);
            }

            ValidateHttpStatus(RestClient.GetHTTPStatusCode());
        }
    }
}
