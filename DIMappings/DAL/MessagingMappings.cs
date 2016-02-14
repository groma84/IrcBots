using Messaging.Contracts;
using IrcMessaging;

namespace DIMappings.DAL
{
    public class MessagingMappings : IInitializeMapping
    {
        void IInitializeMapping.Init(SimpleInjector.Container container)
        {
            container.RegisterSingle<IMessagingClient, IrcMessagingClient>();
        }
    }
}
