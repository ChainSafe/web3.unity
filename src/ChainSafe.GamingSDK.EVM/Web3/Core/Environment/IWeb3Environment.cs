using ChainSafe.GamingWeb3.Logger;

namespace ChainSafe.GamingWeb3.Environment
{
  /// <summary>
  /// The environment Web3 is being executed in.
  /// </summary>
  public interface IWeb3Environment
  {
    public IHttpClient HttpClient { get; }
    public ILogWriter LogWriter { get; }
  }
}