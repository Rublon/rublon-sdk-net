namespace Rublon.Sdk.Core
{
    public interface IRublonConsumer
    {
        string APIServer { get; }
        string Language { get; set; }
        string SecretKey { get; }
        string SystemToken { get; }

        string ProxyHost { get; }
        int ProxyPort { get; }
        string ProxyUser { get; }
        string ProxyPassword { get; }


        bool IsConfigured();
        void TestConfiguration();
    }
}