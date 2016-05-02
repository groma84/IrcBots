using System.ComponentModel;

namespace SuperVisor.Service
{
    [RunInstaller(true)]
    public partial class IrcBotServiceInstaller : System.Configuration.Install.Installer
    {
        public IrcBotServiceInstaller()
        {
            InitializeComponent();
        }
    }
}