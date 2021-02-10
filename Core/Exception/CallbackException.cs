namespace Rublon.Sdk.Core.Exception
{

    public class CallbackException : RublonException
    {
        public CallbackException(string message)
            : base(message)
        {

        }

        public CallbackException(string message, System.Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
