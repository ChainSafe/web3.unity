using ChainSafe.GamingWeb3.Logger;

namespace ChainSafe.GamingWeb3.Environment
{
  /// <summary>
  /// The environment Web3 is being executed in.
  /// </summary>
  public class Web3Environment
  {
    // todo remove dependencies on Web3Environment and replace with desired component dependency
    
    public IHttpClient HttpClient { get; }
    public ILogWriter LogWriter { get; }
    public IAnalyticsClient AnalyticsClient { get; }

    public Web3Environment(IHttpClient httpClient, ILogWriter logWriter, IAnalyticsClient analyticsClient)
    {
      HttpClient = httpClient;
      LogWriter = logWriter;
      AnalyticsClient = analyticsClient;
    }
  }
}