using BotBehaviour.Contracts;
using Configuration.Contracts;
using Logging.Contracts;
using Messaging.Contracts;
using System;
using System.Threading;

namespace TopicBot
{
    public class TopicBot
    {
        public static void Main(string[] args)
        {
            Run();
        }

        public static void Run()
        {
            ILogging logging = null;

            while (true)
            {

                try
                {
                    Action<SimpleInjector.Container>[] botSpecificRegistrations = new Action<SimpleInjector.Container>[]
                             {
                                (simpleInjectorContainer) => simpleInjectorContainer.Register<IBehaviour, TopicBotBehaviour.Behaviour>() // damit legen wir fest, dass sich unser Bot wie ein TopicBot verhalten soll
                             };

                    var container = DIMappings.Container.Init(botSpecificRegistrations);

                    var client = container.GetInstance<IMessagingClient>();
                    var configuration = container.GetInstance<IConfiguration>();
                    var behaviour = container.GetInstance<IBehaviour>();
                    logging = container.GetInstance<ILogging>();

                    RegisterBehaviours(behaviour, client);

                    client.Connect();

                    StartOpGetterThread(client, configuration);

                    client.StartToListen(); // blockierender Aufruf, der dann nur noch Events handled

                    client.Disconnect(); // IRC-Session wurde beendet - hier kommen wir im Normalfall nie an
                }
                catch (Exception ex)
                {
                    if (logging != null)
                    {
                        logging.Log("TopicBot", ex.Message + " -- " + ex.StackTrace + (ex.InnerException != null ? " Inner: " + ex.Message + " -- " + ex.StackTrace : string.Empty));
                    }

                    Thread.Sleep(300000); // 5 Minuten auf bessere Zeiten warten
                }
            }
        }

        private static void StartOpGetterThread(IMessagingClient client, IConfiguration configuration)
        {
            // Zusätzlichen Thread starten, der regelmäßig prüft, ob der TopicBot OP hat, und falls nicht, den Channel verlässt und neu beitritt, 
            // sobald er der einzige User im Channel ist (um so OP zu bekommen)
            var opGetter = new TopicBotBehaviour.TryToGetOp(new TimeSpan(0, 3, 0), client, configuration);
            new Thread(new ThreadStart(opGetter.CheckIfOpAndTryToGetOpIfNotLoop)).Start();
        }

        private static void RegisterBehaviours(IBehaviour behaviour, IMessagingClient client)
        {
            foreach (var handler in behaviour.GetMessageReceivedBehaviours())
            {
                client.RegisterEventHandler(handler.EventType, handler.EventHandler);
            }
        }
    }
}
