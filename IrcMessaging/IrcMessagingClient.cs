using Configuration.Contracts;
using Meebey.SmartIrc4net;
using Messaging.Contracts;
using System;
using System.Collections.Generic;

namespace IrcMessaging
{
    public class IrcMessagingClient : IMessagingClient
    {
        IrcClient _ircClient;

        Dictionary<string, bool> _isOperatorInChannel = new Dictionary<string, bool>();
        private readonly ClientConfiguration _clientConfiguration;

        public IrcMessagingClient(IConfiguration configuration)
        {
            _ircClient = new IrcClient();

            // UTF-8 test
            _ircClient.Encoding = System.Text.Encoding.UTF8;

            // wait time between messages, we can set this lower on own irc servers
            _ircClient.SendDelay = 200;

            // we use channel sync, means we can use irc.GetChannel() and so on
            _ircClient.ActiveChannelSyncing = true;

            _clientConfiguration = configuration.LoadClientConfiguration();
        }
              
        void IMessagingClient.Connect()
        {
            _ircClient.Connect(_clientConfiguration.ServerUrl, _clientConfiguration.ServerPort);
            _ircClient.Login(_clientConfiguration.Nickname, _clientConfiguration.Nickname);
            _ircClient.RfcJoin(_clientConfiguration.Channel);

        }

        void IMessagingClient.StartToListen()
        {          
            _ircClient.Listen();
        }

        void IMessagingClient.Disconnect()
        {
            _ircClient.Disconnect();
        }

        void IMessagingClient.RegisterEventHandler(EventType eventType, MessageReceivedEventHandler messageReceivedEventHandler)
        {
            switch (eventType)
            {
                case EventType.ChannelMessage:
                    Action<object, IrcEventArgs> handler = ((s, e) =>
                    {
                        var convertedData = new MessageData(e.Data.Nick, e.Data.Channel, e.Data.Message, e.Data.MessageArray);
                        messageReceivedEventHandler(convertedData);
                    });
                    _ircClient.OnChannelMessage += new IrcEventHandler(handler);
                    break;

                default:
                    throw new NotImplementedException($"EventType {eventType} not supported yet");

            }
        }

        bool? IMessagingClient.AmIChannelAdmin(string channel)
        {
            var me = _ircClient.GetChannelUser(channel, _clientConfiguration.Nickname);

            return me?.IsOp;
        }

        void IMessagingClient.SendMessage(string receiver, string message)
        {
            _ircClient.SendMessage(SendType.Message, receiver, message);
        }

        void IMessagingClient.ChangeTopic(string channel, string newTopic)
        {
            _ircClient.RfcTopic(channel, newTopic);
        }

        int IMessagingClient.CountUsersInChannel(string channel)
        {
            var chan =_ircClient.GetChannel(channel);
            return chan.Users.Count;
        }

        void IMessagingClient.LeaveChannel(string channel)
        {
            _ircClient.RfcPart(channel);
        }

        void IMessagingClient.JoinChannel(string channel)
        {
            _ircClient.RfcJoin(channel);
        }
    }
}
