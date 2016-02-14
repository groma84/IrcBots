using Configuration.Contracts;

namespace Configuration
{
    public class HardcodedConfiguration : IConfiguration
    {
        ClientConfiguration IConfiguration.LoadClientConfiguration()
        {
            return new ClientConfiguration(
                @"buildme2.is-lan.local",
                6697,
                "_TopicBot_",
                "#webdev"
                );
        }
    }
}
