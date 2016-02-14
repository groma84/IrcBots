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
        private readonly Dictionary<string, ILogger> _loggers;
        private readonly string _logDirectoryName;

        public Logging()
        {
            _logDirectoryName = Path.Combine(Path.GetTempPath(), @"IrcBotService");
            if (!Directory.Exists(_logDirectoryName))
            {
                Directory.CreateDirectory(_logDirectoryName);
            }

            _loggers = new Dictionary<string, ILogger>();
        }

        void ILogging.Log(string botName, string message)
        {
            CreateLoggerIfNotExists(botName);

            _loggers[botName].Information(message);
        }

        private void CreateLoggerIfNotExists(string botName)
        {
            if (!_loggers.ContainsKey(botName))
            {
                var filePath = Path.Combine(_logDirectoryName, botName + "_{Date}.txt");
                var newLogger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                            .WriteTo.RollingFile(filePath, LogEventLevel.Debug, retainedFileCountLimit: 8, fileSizeLimitBytes: null)
                            .CreateLogger();

                _loggers[botName] = newLogger;
            }
        }
    }
}
