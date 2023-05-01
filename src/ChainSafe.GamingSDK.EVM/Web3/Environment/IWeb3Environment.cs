namespace ChainSafe.GamingWeb3.Environment
{
  /// <summary>
  /// The environment Web3 SDK is being executed in.
  /// </summary>
  public interface IWeb3Environment
  {
    public IHttpClient HttpClient { get; }
  }
}