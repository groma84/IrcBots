using Configuration.Contracts;
using Messaging.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;
using System;
using TopicBotBehaviour;

namespace TopicBotBehaviourTest
{
    [TestClass]
    public class TryToGetOpTest
    {
        private TryToGetOp _tryToGetOp;
        private MessagingClientMock _messagingClientMock;
        private ConfigurationMock _configurationMock;

        MessageData _messageData;
        ClientConfiguration _clientConfiguration = new ClientConfiguration("testserver", 6667, "testBot", "testChannel");

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _messageData = new MessageData("TestRunner", "testChannel", "!topic newTopic", new String[] { "!topic", "newTopic" });

            _messagingClientMock = new MessagingClientMock();

            _configurationMock = new ConfigurationMock();
            _configurationMock.LoadClientConfiguration = _clientConfiguration;

            _tryToGetOp = new TryToGetOp(new TimeSpan(1000), _messagingClientMock, _configurationMock);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _messagingClientMock = null;
            _configurationMock = null;
            _tryToGetOp = null;
        }

        [TestMethod]
        public void TryToGetOpTest_NotConnectedYet_DoNothing()
        {
            _messagingClientMock.AmIChannelAdmin.Add(_clientConfiguration.Channel, null);

            _tryToGetOp.CheckIfOpAndTryToGetOpIfNotOnce();

            Assert.IsFalse(_messagingClientMock.CountUsersInChannelCalls.ContainsKey(_clientConfiguration.Channel));
            Assert.IsFalse(_messagingClientMock.LeaveChannelCalls.ContainsKey(_clientConfiguration.Channel));
            Assert.IsFalse(_messagingClientMock.JoinChannelCalls.ContainsKey(_clientConfiguration.Channel));
        }

        [TestMethod]
        public void TryToGetOpTest_AlreadyOp_DoNothing()
        {
            _messagingClientMock.AmIChannelAdmin.Add(_clientConfiguration.Channel, true);

            _tryToGetOp.CheckIfOpAndTryToGetOpIfNotOnce();

            Assert.IsFalse(_messagingClientMock.CountUsersInChannelCalls.ContainsKey(_clientConfiguration.Channel));
            Assert.IsFalse(_messagingClientMock.LeaveChannelCalls.ContainsKey(_clientConfiguration.Channel));
            Assert.IsFalse(_messagingClientMock.JoinChannelCalls.ContainsKey(_clientConfiguration.Channel));
        }

        [TestMethod]
        public void TryToGetOpTest_NotOpAndOnlyUser_RejoinChannel()
        {
            _messagingClientMock.AmIChannelAdmin.Add(_clientConfiguration.Channel, false);
            _messagingClientMock.CountUsersInChannel.Add(_clientConfiguration.Channel, 1);

            _tryToGetOp.CheckIfOpAndTryToGetOpIfNotOnce();

            Assert.AreEqual(1, _messagingClientMock.CountUsersInChannelCalls[_clientConfiguration.Channel]);
            Assert.AreEqual(1, _messagingClientMock.LeaveChannelCalls[_clientConfiguration.Channel]);
            Assert.AreEqual(1, _messagingClientMock.JoinChannelCalls[_clientConfiguration.Channel]);

        }

        [TestMethod]
        public void TryToGetOpTest_NotOpAndOtherUsersPresent_DoNothing()
        {
            _messagingClientMock.AmIChannelAdmin.Add(_clientConfiguration.Channel, false);
            _messagingClientMock.CountUsersInChannel.Add(_clientConfiguration.Channel, 42);

            _tryToGetOp.CheckIfOpAndTryToGetOpIfNotOnce();

            Assert.AreEqual(1, _messagingClientMock.CountUsersInChannelCalls[_clientConfiguration.Channel]);
            Assert.IsFalse(_messagingClientMock.LeaveChannelCalls.ContainsKey(_clientConfiguration.Channel));
            Assert.IsFalse(_messagingClientMock.JoinChannelCalls.ContainsKey(_clientConfiguration.Channel));
        }
    }
}
