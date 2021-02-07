namespace Rublon.Sdk.Core.Exception
{
    public class RublonException : System.Exception
    {
        public RublonException(string message)
            : base(message)
        {

        }

        public RublonException(string message, System.Exception innerException)
            : base(message, innerException)
        {

        }

        public class ConfigurationException : RublonException
        {
            public ConfigurationException()
                : base("System token or/and secret key must be provided before calling this method.")
            {

            }
        }
    }
}
