using System;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;

namespace IrcBotService
{
    static class IrcBotServiceStarter
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (System.Environment.UserInteractive)
            {
                try
                {
                    string parameter = string.Concat(args);
                    switch (parameter)
                    {
                        case "--install":
                            ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
                            Console.WriteLine("install");
                            break;
                        case "--uninstall":
                            ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
                            Console.WriteLine("uninstall");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Fehler: " + ex);
                }
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new IrcBotServiceInstall()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
