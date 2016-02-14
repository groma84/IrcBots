namespace Configuration.Contracts
{
    public class ClientConfiguration
    {
        public readonly string Channel;
        public readonly string Nickname;
        public readonly int ServerPort;
        public readonly string ServerUrl;

        public ClientConfiguration(string serverUrl, int serverPort, string nickname, string channel)
        {
            Channel = channel;
            Nickname = nickname;
            ServerPort = serverPort;
            ServerUrl = serverUrl;
        }
    }
}