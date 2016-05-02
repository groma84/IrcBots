using System;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace IrcBotService
{
    public partial class IrcBotServiceRun : ServiceBase
    {
        public IrcBotServiceRun()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var botRunnerThread = new Thread(new ThreadStart(BotRunner));
            botRunnerThread.Start();
        }

        protected override void OnStop()
        {
        }

        public void BotRunner()
        {
            var topicBotTask = Task.Run(() =>
            {
                TopicBot.TopicBot.Run();
            });

            var botTasks = new[]
            {
                topicBotTask
            };

            Task.WaitAll(botTasks);
        }
    }
}
