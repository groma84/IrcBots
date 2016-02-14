using Configuration.Contracts;

namespace Mocks
{
    public class ConfigurationMock : IConfiguration
    {
        public ClientConfiguration LoadClientConfiguration;
        ClientConfiguration IConfiguration.LoadClientConfiguration()
        {
            return LoadClientConfiguration;
        }
    }
}