using Configuration.Contracts;
using Logging.Contracts;
using Messaging.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using TopicBotBehaviour;

namespace TopicBotBehaviourTest
{
    [TestClass]
    public class TryToGetOpTest
    {
        private TryToGetOp _tryToGetOp;

        MessageData _messageData;
        ClientConfiguration _clientConfiguration = new ClientConfiguration("testserver", 6667, "testBot", "testChannel");
        private Mock<IMessagingClient> _messagingClientMock;
        private Mock<IConfiguration> _configurationMock;
        private Mock<ILogging> _loggingMock;

        [TestInitialize]
        public void TestInitialize()
        {
            TestCleanup();

            _messageData = new MessageData("TestRunner", "testChannel", "!topic newTopic", new String[] { "!topic", "newTopic" });

            _messagingClientMock = new Mock<IMessagingClient>();

            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(mock => mock.LoadClientConfiguration()).Returns(_clientConfiguration);

            _loggingMock = new Mock<ILogging>();

            _tryToGetOp = new TryToGetOp(new TimeSpan(1000), _messagingClientMock.Object, _configurationMock.Object, _loggingMock.Object);
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
            _messagingClientMock.Setup(mock => mock.AmIChannelAdmin(It.Is<string>(a => a == _clientConfiguration.Channel))).Returns((bool?)null);

            _tryToGetOp.CheckIfOpAndTryToGetOpIfNotOnce();

            _messagingClientMock.Verify(mock => mock.CountUsersInChannel(It.Is<string>(p => p == _clientConfiguration.Channel)), Times.Never);
            _messagingClientMock.Verify(mock => mock.LeaveChannel(It.Is<string>(p => p == _clientConfiguration.Channel)), Times.Never);
            _messagingClientMock.Verify(mock => mock.JoinChannel(It.Is<string>(p => p == _clientConfiguration.Channel)), Times.Never);
        }

        [TestMethod]
        public void TryToGetOpTest_AlreadyOp_DoNothing()
        {
            _messagingClientMock.Setup(mock => mock.AmIChannelAdmin(It.Is<string>(a => a == _clientConfiguration.Channel))).Returns(true);

            _tryToGetOp.CheckIfOpAndTryToGetOpIfNotOnce();

            _messagingClientMock.Verify(mock => mock.CountUsersInChannel(It.Is<string>(p => p == _clientConfiguration.Channel)), Times.Never);
            _messagingClientMock.Verify(mock => mock.LeaveChannel(It.Is<string>(p => p == _clientConfiguration.Channel)), Times.Never);
            _messagingClientMock.Verify(mock => mock.JoinChannel(It.Is<string>(p => p == _clientConfiguration.Channel)), Times.Never);
        }

        [TestMethod]
        public void TryToGetOpTest_NotOpAndOnlyUser_RejoinChannel()
        {
            _messagingClientMock.Setup(mock => mock.AmIChannelAdmin(It.Is<string>(a => a == _clientConfiguration.Channel))).Returns(false);
            _messagingClientMock.Setup(mock => mock.CountUsersInChannel(It.Is<string>(a => a == _clientConfiguration.Channel))).Returns(1);

            _tryToGetOp.CheckIfOpAndTryToGetOpIfNotOnce();

            _messagingClientMock.Verify(mock => mock.CountUsersInChannel(It.Is<string>(p => p == _clientConfiguration.Channel)), Times.Once);
            _messagingClientMock.Verify(mock => mock.LeaveChannel(It.Is<string>(p => p == _clientConfiguration.Channel)), Times.Once);
            _messagingClientMock.Verify(mock => mock.JoinChannel(It.Is<string>(p => p == _clientConfiguration.Channel)), Times.Once);
        }

        [TestMethod]
        public void TryToGetOpTest_NotOpAndOtherUsersPresent_DoNothing()
        {
            _messagingClientMock.Setup(mock => mock.AmIChannelAdmin(It.Is<string>(a => a == _clientConfiguration.Channel))).Returns(false);
            _messagingClientMock.Setup(mock => mock.CountUsersInChannel(It.Is<string>(a => a == _clientConfiguration.Channel))).Returns(42);

            _tryToGetOp.CheckIfOpAndTryToGetOpIfNotOnce();

            _messagingClientMock.Verify(mock => mock.CountUsersInChannel(It.Is<string>(p => p == _clientConfiguration.Channel)), Times.Once);
            _messagingClientMock.Verify(mock => mock.LeaveChannel(It.Is<string>(p => p == _clientConfiguration.Channel)), Times.Never);
            _messagingClientMock.Verify(mock => mock.JoinChannel(It.Is<string>(p => p == _clientConfiguration.Channel)), Times.Never);
        }
    }
}
