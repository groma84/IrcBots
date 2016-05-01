using Configuration.Contracts;
using Logging.Contracts;
using Messaging.Contracts;
using System;
using System.Threading;

namespace TopicBotBehaviour
{
    public class TryToGetOp
    {
        private readonly ILogging _logging;
        private readonly ClientConfiguration _clientConfiguration;
        private readonly IMessagingClient _messageClient;
        private readonly TimeSpan _sleepTimeBetweenChecks;

        public TryToGetOp(TimeSpan sleepTimeBetweenChecks, IMessagingClient messageClient, IConfiguration configuration, ILogging logging)
        {
            _sleepTimeBetweenChecks = sleepTimeBetweenChecks;
            _messageClient = messageClient;
            _clientConfiguration = configuration.LoadClientConfiguration();
            _logging = logging;
        }

        public void CheckIfOpAndTryToGetOpIfNotLoop()
        {
            while (true)
            {
                try
                {
                    CheckIfOpAndTryToGetOpIfNotOnce();

                    Thread.Sleep(_sleepTimeBetweenChecks);
                }
                catch (Exception ex)
                {
                    _logging.LogError("TopicBot", "TopicBot->CheckIfOpAndTryToGetOpIfNotLoop:" + ex.Message + " -- " + ex.StackTrace + (ex.InnerException != null ? " Inner: " + ex.Message + " -- " + ex.StackTrace : string.Empty));
                }
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
