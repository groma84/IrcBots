using Logging.Contracts;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging
{
    public class Logging : ILogging
    {
        private readonly ILogger _logger;

        public Logging()
        {
            _logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.ColoredConsole()
#if RELEASE
                    .WriteTo.EventLog("IrcBotService", "IrcBotService", restrictedToMinimumLevel: LogEventLevel.Warning)
#endif
                            .CreateLogger();
        }

        void ILogging.LogInfo(string botName, string message)
        {
            _logger.Information(message);
        }

        void ILogging.LogError(string botName, string message)
        {
            _logger.Error(message);
        }
    }
}
