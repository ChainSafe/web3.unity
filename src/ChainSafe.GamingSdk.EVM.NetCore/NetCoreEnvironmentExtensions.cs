using ChainSafe.GamingWeb3.Build;
using ChainSafe.GamingWeb3.Environment;
using ChainSafe.GamingWeb3.Logger;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.GamingWeb3.Evm.NetCore
{
  public static class NetCoreEnvironmentExtensions
  {
    public static void UseNetCoreEnvironment(this IWeb3ServiceCollection services)
    {
      services.AddSingleton<Web3Environment>();
      services.AddSingleton<IHttpClient, NetCoreHttpClient>();
      services.AddSingleton<ILogWriter, NetCoreLogWriter>();
      services.AddSingleton<IAnalyticsClient, NetCoreAnalytics>();
    }
  }
}