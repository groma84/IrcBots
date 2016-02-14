using System;

namespace Messaging.Contracts
{
    public interface IMessagingClient
    {
        void Connect();
        void StartToListen();
        void Disconnect();
        void RegisterEventHandler(EventType eventType, MessageReceivedEventHandler messageReceivedEventHandler);


        bool? AmIChannelAdmin(String channel);

        void SendMessage(String receiver, String message);
        void ChangeTopic(String channel, String newTopic);
        int CountUsersInChannel(string channel);
        void LeaveChannel(string channel);
        void JoinChannel(string channel);
    }
}
