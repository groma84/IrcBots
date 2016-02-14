using Configuration.Contracts;
using Configuration;

namespace DIMappings.CrossCutting
{
    public class ConfigurationMappings : IInitializeMapping
    {
        void IInitializeMapping.Init(SimpleInjector.Container container)
        {
            container.RegisterSingle<IConfiguration, HardcodedConfiguration>();
        }
    }
}
