using Messaging.Contracts;

namespace BotBehaviour.Contracts
{
    public class MessageReceivedEventHandlerMapping
    {
        public readonly EventType EventType;
        public readonly MessageReceivedEventHandler EventHandler;

        public MessageReceivedEventHandlerMapping(EventType eventType, MessageReceivedEventHandler eventHandler)
        {
            EventType = eventType;
            EventHandler = eventHandler;
        }
    }
}