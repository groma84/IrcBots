using Messaging.Contracts;

namespace TopicBotBehaviour
{
    public class ChannelMessageActions
    {
        private readonly IMessagingClient _messagingClient;

        public ChannelMessageActions(IMessagingClient messagingClient)
        {
            _messagingClient = messagingClient;
        }

        /// <summary>
        /// Event-Handler, der bei dem Befehl !topic <neues Topic> im Channel das Topic setzt (sofern der Bot OP hat, und sonst einen Fehler ausgibt)
        /// </summary>
        /// <param name="messageData"></param>
        public void ChangeTopic(MessageData messageData)
        {
            string command = "!topic";

            if (messageData.MessageParts.Length == 0 || !messageData.MessageParts[0].StartsWith(command)) {
                return;
            }

            if (!_messagingClient.AmIChannelAdmin(messageData.Channel).Value)
            {
                _messagingClient.SendMessage(messageData.Channel, Texts.TopicChangeRequestedButIAmNotOperator);

                return;
            }

            string newTopic = PrepareTopic(messageData.Message, command, messageData.Nickname);

            _messagingClient.ChangeTopic(messageData.Channel, newTopic);
        }

        private string PrepareTopic(string message, string command, string settingUser)
        {
            // wir müssen das command entfernen und das darauf folgende Leerzeichen und dann den setzenden User ranhängen
            return (message.Remove(0, command.Length + 1) + $" [{settingUser}]");
        }
    }
}
