using Configuration.Contracts;
using Configuration;
using Logging.Contracts;

namespace DIMappings
{
    public class CrossCuttingMappings : IInitializeMapping
    {
        void IInitializeMapping.Init(SimpleInjector.Container container)
        {
            container.RegisterSingleton<ILogging, Logging.Logging>();
            container.RegisterSingleton<IConfiguration, HardcodedConfiguration>();
        }
    }
}
