using Configuration.Contracts;
using Messaging.Contracts;
using System;
using System.Threading;

namespace TopicBotBehaviour
{
    public class TryToGetOp
    {
        private readonly ClientConfiguration _clientConfiguration;
        private readonly IMessagingClient _messageClient;
        private readonly TimeSpan _sleepTimeBetweenChecks;

        public TryToGetOp(TimeSpan sleepTimeBetweenChecks, IMessagingClient messageClient, IConfiguration configuration)
        {
            _sleepTimeBetweenChecks = sleepTimeBetweenChecks;
            _messageClient = messageClient;
            _clientConfiguration = configuration.LoadClientConfiguration();
        }

        public void CheckIfOpAndTryToGetOpIfNotLoop()
        {
            while (true)
            {
                CheckIfOpAndTryToGetOpIfNotOnce();

                Thread.Sleep(_sleepTimeBetweenChecks);
            }
        }

        internal void CheckIfOpAndTryToGetOpIfNotOnce()
        {
            var alreadyOp = _messageClient.AmIChannelAdmin(_clientConfiguration.Channel);

            if (alreadyOp.HasValue && !alreadyOp.Value)
            {
                var userCount = _messageClient.CountUsersInChannel(_clientConfiguration.Channel);

                if (userCount == 1) // nur noch unser Bot drin
                {
                    // nach einem Rejoin wird der Channel neu erzeugt und wir haben als 1. Bewohnen direkt die Macht
                    _messageClient.LeaveChannel(_clientConfiguration.Channel);
                    _messageClient.JoinChannel(_clientConfiguration.Channel);
                }
            }
        }
    }
}
