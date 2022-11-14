using Newtonsoft.Json.Linq;
using Rublon.Sdk.Core.Rest;
using System;

namespace Rublon.Sdk.Core.Exception
{
    /// <summary>
    /// API exceptions thrown when Rublon Authentication  server returns error.
    /// </summary>
    public class APIException : RublonException
    {
                
        protected RESTClient client;

        /// <summary>
        /// Construct the exception instance.
        /// </summary>
        /// <param name="client">REST client instance which exectuted the request to Rublon.</param>
        public APIException(RESTClient client)
            : this(client, null)
        {

        }

        /// <summary>
        /// Construct the exception instance.
        /// </summary>
        /// <param name="client">REST client instance, which exectuted the request to Rublon.</param>
        /// <param name="message">Exception message.</param>
        public APIException(RESTClient client, string message)
            : base(message)
        {
            this.client = client;
        }

        /// <summary>
        /// Get REST client instance, which exectuted the request to Rublon.
        /// </summary>
        public RESTClient Client
        {
            get { return client; }
        }

        [Serializable]
        public class InvalidCoreResponseHttpStatus : RublonException
        {
            public InvalidCoreResponseHttpStatus() : base() { }
            public InvalidCoreResponseHttpStatus(string message) : base(message) { }
            public InvalidCoreResponseHttpStatus(string message, JObject response) : base(message) { }
            public InvalidCoreResponseHttpStatus(string message, System.Exception innerException) : base(message, innerException) { }
        }

        public class EmptyInputException : APIException
        {
            public EmptyInputException(RESTClient client, string message)
                : base(client, message)
            {

            }

            public EmptyInputException(RESTClient client)
                : base(client, null)
            {

            }
        }

        public class InvalidJSONException : APIException
        {
            public InvalidJSONException(RESTClient client, string message)
                : base(client, message)
            {

            }

            public InvalidJSONException(RESTClient client)
                : base(client, null)
            {

            }
        }
        
        public class InvalidSignatureException : APIException
        {
            public InvalidSignatureException(RESTClient client, string message)
                : base(client, message)
            {

            }

            public InvalidSignatureException(RESTClient client)
                : base(client, null)
            {

            }
        }

        public class UnsupportedVersionException : APIException
        {
            public UnsupportedVersionException(RESTClient client, string message)
                : base(client, message)
            {

            }

            public UnsupportedVersionException(RESTClient client)
                : base(client, null)
            {

            }
        }

        public class ForbiddenMethodException : APIException
        {
            public ForbiddenMethodException(RESTClient client, string message)
                : base(client, message)
            {

            }

            public ForbiddenMethodException(RESTClient client)
                : base(client, null)
            {

            }
        }

        public class MissingFieldException : APIException
        {
            private string itemName;

            public MissingFieldException(RESTClient client, string message, string itemName)
                : base(client, message)
            {
                this.itemName = itemName;
            }

            public MissingFieldException(RESTClient client, string itemName)
                : this(client, "Missing field", itemName)
            {

            }

            public string ItemName
            {
                get { return itemName; }
            }
        }

        public class MissingHeaderException : MissingFieldException
        {
            public MissingHeaderException(RESTClient client, string message, string itemName)
                : base(client, message, itemName)
            {

            }

            public MissingHeaderException(RESTClient client, string itemName)
                : base(client, "Missing header", itemName)
            {

            }
        }

        public class InvalidFieldException : MissingFieldException
        {
            public InvalidFieldException(RESTClient client, string message, string itemName)
                : base(client, message, itemName)
            {

            }

            public InvalidFieldException(RESTClient client, string itemName)
                : base(client, null, itemName)
            {

            }
        }

        public class UserBypassedException : APIException
        {
            public UserBypassedException(RESTClient client, string message) : base(client, message) { }
            public UserBypassedException(RESTClient client) : base(client) { }

        }
                
        public class ApplicationDeniedException : APIException
        {

            public ApplicationDeniedException(RESTClient client, String message)
            : base(client, message)
            { }
            
            public ApplicationDeniedException(RESTClient client)
            :base(client){}
            
        }

        public class UnauthorizedUserException : APIException
        {
            public UnauthorizedUserException(RESTClient client, string message)
                : base(client, message)
            {

            }

            public UnauthorizedUserException(RESTClient client)
                : base(client, null)
            {

            }
        }

        public class TransactionLockedException : APIException
        {
           public TransactionLockedException(RESTClient client, String message)
            : base(client, message) { }
            public TransactionLockedException(RESTClient client)
            : base(client, null) { }
        }

        public class AccessTokenExpiredException : APIException
        {
            public AccessTokenExpiredException(RESTClient client, string message)
                : base(client, message)
            {

            }

            public AccessTokenExpiredException(RESTClient client)
                : base(client, null)
            {

            }
        }

        public class UnknownAccessTokenException : APIException
        {
            public UnknownAccessTokenException(RESTClient client, string message)
                : base(client, message)
            {

            }

            public UnknownAccessTokenException(RESTClient client)
                : base(client, null)
            {

            }
        }


    }
}
