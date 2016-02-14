namespace IrcBotService
{
    partial class IrcBotService
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.IrcBotServiceBase = new System.ServiceProcess.ServiceBase();
            this.IrcBotServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.IrcBotServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // IrcBotServiceBase
            // 
            this.IrcBotServiceBase.ExitCode = 0;
            this.IrcBotServiceBase.ServiceName = "IrcBotService";
            // 
            // IrcBotServiceProcessInstaller
            // 
            this.IrcBotServiceProcessInstaller.Password = null;
            this.IrcBotServiceProcessInstaller.Username = null;
            // 
            // IrcBotServiceInstaller
            // 
            this.IrcBotServiceInstaller.Description = "IRC Bots der IW-Entwicklung";
            this.IrcBotServiceInstaller.DisplayName = "IrcBotService";
            this.IrcBotServiceInstaller.ServiceName = "IrcBotService";
            this.IrcBotServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // IrcBotService
            // 
            this.ServiceName = "Service1";

        }

        #endregion

        private System.ServiceProcess.ServiceBase IrcBotServiceBase;
        private System.ServiceProcess.ServiceProcessInstaller IrcBotServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller IrcBotServiceInstaller;
    }
}
