namespace ChainSafe.GamingWeb3.Logger
{
  public interface ILogWriter
  {
    public void Log(string message);
    public void LogError(string message);
  }
}