namespace ChainSafe.GamingWeb3.Environment
{
    public interface ILogWriter
    {
        public void Log(string message);

        public void LogError(string message);
    }
}