using Messaging.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;
using System;
using System.Linq;
using TopicBotBehaviour;

namespace TopicBotBehaviourTest
{
    [TestClass]
    public class ChannelMessageActionsTest
    {
        private ChannelMessageActions _channelMessageActions;
        private MessagingClientMock _messagingClientMock;

        MessageData _messageData;

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _messageData = new MessageData("TestRunner", "testChannel", "!topic newTopic", new String[] { "!topic", "newTopic" });

            _messagingClientMock = new MessagingClientMock();

            _channelMessageActions = new ChannelMessageActions(_messagingClientMock);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _messagingClientMock = null;
            _channelMessageActions = null;
        }

        [TestMethod]
        public void ChannelMessageActionsTest_ChangeTopic_MessageWithoutCommand_NothingDone()
        {
            _messageData = new MessageData(String.Empty, "testChannel", "BANANAAAA!", new String[] { "BANANAAAA!" });

            _messagingClientMock.AmIChannelAdmin.Add(_messageData.Channel, false);

            Assert.AreEqual(0, _messagingClientMock.AmIChannelAdminCalls.Count);
            Assert.AreEqual(0, _messagingClientMock.SendMessageCalls.Count);
            Assert.AreEqual(0, _messagingClientMock.ChangeTopicCalls.Count);

            _channelMessageActions.ChangeTopic(_messageData);

            Assert.AreEqual(0, _messagingClientMock.AmIChannelAdminCalls.Count);
            Assert.AreEqual(0, _messagingClientMock.SendMessageCalls.Count);
            Assert.AreEqual(0, _messagingClientMock.ChangeTopicCalls.Count);
        }

        [TestMethod]
        public void ChannelMessageActionsTest_ChangeTopic_NotOp_MessageSentToChannel()
        {
            _messagingClientMock.AmIChannelAdmin.Add(_messageData.Channel, false);

            Assert.AreEqual(0, _messagingClientMock.AmIChannelAdminCalls.Count);
            Assert.AreEqual(0, _messagingClientMock.SendMessageCalls.Count);

            _channelMessageActions.ChangeTopic(_messageData);

            Assert.AreEqual(1, _messagingClientMock.AmIChannelAdminCalls.Count);
            Assert.AreEqual(_messageData.Channel, _messagingClientMock.AmIChannelAdminCalls.First());

            Assert.AreEqual(1, _messagingClientMock.SendMessageCalls.Count);
            Assert.AreEqual(_messageData.Channel, _messagingClientMock.SendMessageCalls.First().Item1);
            Assert.IsFalse(string.IsNullOrEmpty(_messagingClientMock.SendMessageCalls.First().Item2));

            Assert.AreEqual(0, _messagingClientMock.ChangeTopicCalls.Count);

        }

        [TestMethod]
        public void ChannelMessageActionsTest_ChangeTopic_Op_NewTopicSet()
        {
            _messagingClientMock.AmIChannelAdmin.Add(_messageData.Channel, true);

            Assert.AreEqual(0, _messagingClientMock.AmIChannelAdminCalls.Count);
            Assert.AreEqual(0, _messagingClientMock.ChangeTopicCalls.Count);

            _channelMessageActions.ChangeTopic(_messageData);

            Assert.AreEqual(1, _messagingClientMock.AmIChannelAdminCalls.Count);
            Assert.AreEqual(_messageData.Channel, _messagingClientMock.AmIChannelAdminCalls.First());

            Assert.AreEqual(1, _messagingClientMock.ChangeTopicCalls.Count);
            Assert.AreEqual(_messageData.Channel, _messagingClientMock.ChangeTopicCalls.First().Item1);
            Assert.AreEqual("newTopic [TestRunner]", _messagingClientMock.ChangeTopicCalls.First().Item2);

            Assert.AreEqual(0, _messagingClientMock.SendMessageCalls.Count);
        }
    }
}
