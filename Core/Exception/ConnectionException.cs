namespace Rublon.Sdk.Core.Exception
{
    /// <summary>
    /// Connection error exception
    /// </summary>
    public class ConnectionException : RublonException
    {
        public ConnectionException(string message)
            : base(message)
        {

        }

        public ConnectionException(string message, System.Exception inner) : base(message, inner) { }
    }
}
