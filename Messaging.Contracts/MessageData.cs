using System.Collections.Generic;
using System.Linq;

namespace Messaging.Contracts
{
    public class MessageData
    {
        public readonly string Nickname;
        public readonly string Channel;
        public readonly string Message;
        public readonly string[] MessageParts;

        public MessageData(string nickname, string channel, string message, IEnumerable<string> messageParts)
        {
            Nickname = nickname;
            Channel = channel;
            Message = message;
            MessageParts = messageParts.ToArray();
        }
    }
}