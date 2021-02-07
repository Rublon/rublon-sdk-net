namespace Rublon.Sdk.Core.Exception
{
    public class ConnectionException : RublonException
    {
        public ConnectionException(string message)
            : base(message)
        {

        }

        public ConnectionException(string message, System.Exception inner) : base(message, inner) { }
    }
}
