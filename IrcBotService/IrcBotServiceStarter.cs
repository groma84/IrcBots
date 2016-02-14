using System.ServiceProcess;

namespace IrcBotService
{
    static class IrcBotServiceStarter
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new IrcBotService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
