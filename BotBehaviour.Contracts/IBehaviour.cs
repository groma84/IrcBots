using System.Collections.Generic;

namespace BotBehaviour.Contracts
{
    public interface IBehaviour
    {
        IEnumerable<MessageReceivedEventHandlerMapping> GetMessageReceivedBehaviours();
    }
}
