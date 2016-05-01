namespace Logging.Contracts
{
    public interface ILogging
    {
        void LogInfo(string botName, string message);
        void LogError(string botName, string message);
    }
}
