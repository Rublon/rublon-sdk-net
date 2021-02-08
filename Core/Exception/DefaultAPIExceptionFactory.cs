using Newtonsoft.Json.Linq;
using Rublon.Sdk.Core.Rest;
using Rublon.Sdk.Core.Validation;

namespace Rublon.Sdk.Core.Exception
{
    public class DefaultAPIExceptionFactory : IAPIExceptionFactory
    {
        public class ExceptionDescription
        {
            public string Exception { get; set; } = "";
            public string ErrorMessage { get; set; }
            public string Name { get; set; }
        }

       
        public APIException createException(RESTClient client)
        {
            APIException resultException = null;
            ExceptionDescription exceptionDesc = null;
            try
            {               
                var responseInJSON = JObject.Parse(client.RawResponse);                
                var status = responseInJSON.Value<string>(DefaultAPIResponseValidator.FIELD_STATUS);
                if (status != null && status == DefaultAPIResponseValidator.STATUS_ERROR)
                {
                    var result = responseInJSON.Value<JObject>(APIMethod.FIELD_RESULT);
                    if (result != null)
                    {
                        exceptionDesc = result.ToObject<ExceptionDescription>();
                        resultException = createException(exceptionDesc.Exception, client, exceptionDesc.ErrorMessage, exceptionDesc.Name);
                    }
                }
                if (resultException == null)
                {
                    resultException = new APIException(client, (exceptionDesc != null ? exceptionDesc.ErrorMessage : ""));
                }
            }
            catch (Newtonsoft.Json.JsonReaderException )
            {
                resultException = new APIException(client, "Invalid json with error description");
            }
            return resultException;
        }

        private static APIException createException(string name, RESTClient client, string message, string itemName)
        {
            APIException result = null;
            var loweredName = name.ToLower();
            switch (loweredName) {
                case "userbypassedexception" :
                    result = new APIException.UserBypassedException(client, message);
                    break;
                case "missingfieldexception":
                    result = new APIException.MissingFieldException(client, message, itemName);
                    break;
                case "missingheaderexception":
                    result = new APIException.MissingHeaderException(client, message, itemName);
                    break;
                case "emptyinputexception":
                    result = new APIException.EmptyInputException(client,message);
                    break;
                case "invalidjsonexception":
                    result = new APIException.InvalidJSONException(client, message);
                    break;
                case "invalidsignatureexception":
                    result = new APIException.InvalidSignatureException(client,message);
                    break;
                case "unsupportedversionexception":
                    result = new APIException.UnsupportedVersionException(client, message);
                    break;
                case "accesstokenexpiredexception":
                    result = new APIException.AccessTokenExpiredException(client, message);
                    break;
                case "unknownaccesstokenexception":
                    result = new APIException.UnknownAccessTokenException(client, message);
                    break;
                case "unauthorizeduserexception":
                    result = new APIException.UnauthorizedUserException(client, message);
                    break;
                case "forbiddenmethodexception":
                    result = new APIException.ForbiddenMethodException(client, message);
                    break;
                case "applicationdeniedexception":
                    result = new APIException.ApplicationDeniedException(client, message);
                    break;
                case "transactionlockedexception":
                    result = new APIException.TransactionLockedException(client, message);
                    break;
            }
            return result;            
        }       
    }
}
