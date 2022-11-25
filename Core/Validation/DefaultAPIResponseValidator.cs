using Newtonsoft.Json.Linq;
using Rublon.Sdk.Core.Exception;
using Rublon.Sdk.Core.Rest;
using Rublon.Sdk.Core.Signature;
using System.Net;

namespace Rublon.Sdk.Core.Validation
{
    internal class DefaultAPIResponseValidator : IAPIResponseValidator
    {

        private JObject response;
        private JObject responseResult;

        public const string STATUS_OK = "OK";
        public const string STATUS_ERROR = "ERROR";
        public const string FIELD_RESULT = "result";
        public const string FIELD_STATUS = "status";

        public IAPIExceptionFactory ApiExceptionFactory
        {
            get;
            set;
        } = new DefaultAPIExceptionFactory();

        public RESTClient RestClient { get; set; }

        public RublonMessageSigner RublonMessageSigner
        {
            get;
            set;
        } = new RublonMessageSigner();

        public string SecretKey { get; set; }

        public void validateResponse()
        {
            assertResponseContentAndResponseCodeIsValidOrThrow();

            var status = response.Value<string>(FIELD_STATUS);
            if (string.IsNullOrEmpty(status))
            {
                throw new APIException.MissingFieldException(RestClient, FIELD_STATUS);
            }

            if (status == STATUS_OK)
            {
                assertSignatureIsValidOrThrow();
            }
            else if (status == STATUS_ERROR)
            {
                throw this.ApiExceptionFactory.createException(RestClient);
            }
            else
            {
                throw new APIException.InvalidFieldException(RestClient, "Invalid status field", status);
            }
        }

        private void assertResponseContentAndResponseCodeIsValidOrThrow()
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

            responseResult = response.Value<JObject>(FIELD_RESULT);
            if (responseResult == null || responseResult.Count == 0)
            {
                throw new APIException.MissingFieldException(RestClient, FIELD_RESULT);
            }
        }

        private void ValidateHttpStatus(HttpStatusCode statusCode)
        {
            if ((int)statusCode >= 500 && (int)statusCode <= 599)
                throw new APIException.InvalidCoreResponseHttpStatus(string.Format("Server error occured: {0}", statusCode), response);
            else
            {
                bool isHttpCodeWhichCanBeHandled = statusCode == HttpStatusCode.OK || statusCode == HttpStatusCode.BadRequest;
                if (!isHttpCodeWhichCanBeHandled)
                {
                    throw new APIException(RestClient, "Unexpected response HTTP status code: " + RestClient.GetHTTPStatusCode());
                }
            }
        }

        private void assertSignatureIsValidOrThrow()
        {
            var signature = RestClient.GetSignature();
            if (signature == null)
            {
                throw new APIException.MissingHeaderException(RestClient, RESTClient.HEADER_NAME_SIGNATURE);
            }

            if (!RublonMessageSigner.VerifySignatureForData(RestClient.RawResponse, SecretKey, signature))
            {
                throw new APIException.InvalidSignatureException(RestClient, "Invalid response signature: " + signature);
            }
        }
    }
}