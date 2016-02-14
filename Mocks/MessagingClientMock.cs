using Messaging.Contracts;
using System;
using System.Collections.Generic;

namespace Mocks
{
    public class MessagingClientMock : IMessagingClient
    {
        public Dictionary<String, bool?> AmIChannelAdmin = new Dictionary<string, bool?>();
        public Queue<String> AmIChannelAdminCalls = new Queue<string>();
        bool? IMessagingClient.AmIChannelAdmin(string channel)
        {
            AmIChannelAdminCalls.Enqueue(channel);

            return AmIChannelAdmin[channel];
        }

        void IMessagingClient.Connect()
        {
            throw new NotImplementedException();
        }

        void IMessagingClient.Disconnect()
        {
            throw new NotImplementedException();
        }

        void IMessagingClient.RegisterEventHandler(EventType eventType, MessageReceivedEventHandler messageReceivedEventHandler)
        {
            throw new NotImplementedException();
        }

        public Queue<Tuple<String, String>> SendMessageCalls = new Queue<Tuple<String, String>>();
        void IMessagingClient.SendMessage(string receiver, string message)
        {
            SendMessageCalls.Enqueue(new Tuple<string, string>(receiver, message));
        }

        void IMessagingClient.StartToListen()
        {
            throw new NotImplementedException();
        }

        public Queue<Tuple<String, String>> ChangeTopicCalls = new Queue<Tuple<String, String>>();
        void IMessagingClient.ChangeTopic(string channel, string newTopic)
        {
            ChangeTopicCalls.Enqueue(new Tuple<string, string>(channel, newTopic));
        }

        public Dictionary<String, int> CountUsersInChannel = new Dictionary<string, int>();
        public Dictionary<String, int> CountUsersInChannelCalls = new Dictionary<String, int>();
        int IMessagingClient.CountUsersInChannel(string channel)
        {
            if (!CountUsersInChannelCalls.ContainsKey(channel))
            {
                CountUsersInChannelCalls[channel] = 0;
            }

            ++CountUsersInChannelCalls[channel];

            return CountUsersInChannel[channel];
        }

        public Dictionary<String, int> LeaveChannelCalls = new Dictionary<String, int>();
        void IMessagingClient.LeaveChannel(string channel)
        {
            if (!LeaveChannelCalls.ContainsKey(channel))
            {
                LeaveChannelCalls[channel] = 0; 
            }

            ++LeaveChannelCalls[channel];
        }

        public Dictionary<String, int> JoinChannelCalls = new Dictionary<String, int>();
        void IMessagingClient.JoinChannel(string channel)
        {
            if (!JoinChannelCalls.ContainsKey(channel))
            {
                JoinChannelCalls[channel] = 0;
            }

            ++JoinChannelCalls[channel];
        }
    }
}
