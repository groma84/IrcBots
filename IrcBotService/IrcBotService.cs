using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace IrcBotService
{
    public partial class IrcBotService : ServiceBase
    {
        public IrcBotService()
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

        private void BotRunner()
        {
            var topicBotTask = Task.Run(() =>
            {
                TopicBot.TopicBot.Run();
            });

            var botTasks = new[]
            {
                topicBotTask
            };

            foreach (var t in botTasks)
            {
                t.Start();
            }

            Task.WaitAll(botTasks);
        }
    }
}
