using BotBehaviour.Contracts;
using System.Collections.Generic;
using Messaging.Contracts;

namespace TopicBotBehaviour
{
    public class Behaviour : IBehaviour
    {
        private readonly IMessagingClient _messageClient;

        public Behaviour(IMessagingClient messageClient)
        {
            _messageClient = messageClient;
        }

        IEnumerable<MessageReceivedEventHandlerMapping> IBehaviour.GetMessageReceivedBehaviours()
        {
            var onChannelMessage = new ChannelMessageActions(_messageClient);

            return new MessageReceivedEventHandlerMapping[] {
                new MessageReceivedEventHandlerMapping(EventType.ChannelMessage, onChannelMessage.ChangeTopic),
            };
        }
    }
}
