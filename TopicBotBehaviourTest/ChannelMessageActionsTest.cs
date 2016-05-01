using Messaging.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using TopicBotBehaviour;

namespace TopicBotBehaviourTest
{
    [TestClass]
    public class ChannelMessageActionsTest
    {
        private ChannelMessageActions _channelMessageActions;
        private Mock<IMessagingClient> _messagingClientMock;

        MessageData _messageData;

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _messageData = new MessageData("TestRunner", "testChannel", "!topic newTopic", new String[] { "!topic", "newTopic" });

            _messagingClientMock = new Mock<IMessagingClient>();

            _channelMessageActions = new ChannelMessageActions(_messagingClientMock.Object);
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

            _messagingClientMock.Setup(mock => mock.AmIChannelAdmin(It.Is<string>(a => a == _messageData.Channel))).Returns(false);

            _messagingClientMock.Verify(mock => mock.AmIChannelAdmin(It.Is<string>(a => a == _messageData.Channel)), Times.Never);
            _messagingClientMock.Verify(mock => mock.SendMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _messagingClientMock.Verify(mock => mock.ChangeTopic(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

            _channelMessageActions.ChangeTopic(_messageData);

            _messagingClientMock.Verify(mock => mock.AmIChannelAdmin(It.IsAny<string>()), Times.Never);
            _messagingClientMock.Verify(mock => mock.SendMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _messagingClientMock.Verify(mock => mock.ChangeTopic(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void ChannelMessageActionsTest_ChangeTopic_NotOp_MessageSentToChannel()
        {
            _messagingClientMock.Setup(mock => mock.AmIChannelAdmin(It.Is<string>(a => a == _messageData.Channel))).Returns(false);

            _messagingClientMock.Verify(mock => mock.AmIChannelAdmin(It.IsAny<string>()), Times.Never);
            _messagingClientMock.Verify(mock => mock.SendMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

            _channelMessageActions.ChangeTopic(_messageData);

            _messagingClientMock.Verify(mock => mock.AmIChannelAdmin(It.Is<string>(a => a == _messageData.Channel)), Times.Once);
            _messagingClientMock.Verify(mock => mock.SendMessage(It.Is<string>(p => p == _messageData.Channel), It.Is<string>(p => !string.IsNullOrEmpty(p))), Times.Once);
            _messagingClientMock.Verify(mock => mock.ChangeTopic(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void ChannelMessageActionsTest_ChangeTopic_Op_NewTopicSet()
        {
            _messagingClientMock.Setup(mock => mock.AmIChannelAdmin(It.Is<string>(a => a == _messageData.Channel))).Returns(true);

            _messagingClientMock.Verify(mock => mock.AmIChannelAdmin(It.IsAny<string>()), Times.Never);
            _messagingClientMock.Verify(mock => mock.ChangeTopic(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

            _channelMessageActions.ChangeTopic(_messageData);

            _messagingClientMock.Verify(mock => mock.AmIChannelAdmin(It.Is<string>(a => a == _messageData.Channel)), Times.Once);
            _messagingClientMock.Verify(mock => mock.ChangeTopic(It.Is<string>(a => a == _messageData.Channel), It.Is<string>(p => p == "newTopic [TestRunner]")), Times.Once);
            _messagingClientMock.Verify(mock => mock.SendMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}
