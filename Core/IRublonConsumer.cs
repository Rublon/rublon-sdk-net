namespace Rublon.Sdk.Core
{
    public interface IRublonConsumer
    {
        string APIServer { get; }
        string Language { get; set; }
        string SecretKey { get; }
        string SystemToken { get; }

        bool IsConfigured();
        void TestConfiguration();
    }
}