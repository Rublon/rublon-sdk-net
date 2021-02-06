namespace Rublon.Sdk.Core.Exception
{
    /// <summary>
    /// Callback exception class.
    /// </summary>
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
