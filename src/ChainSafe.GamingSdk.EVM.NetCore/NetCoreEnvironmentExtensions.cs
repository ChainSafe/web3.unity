using ChainSafe.GamingWeb3.Build;
using ChainSafe.GamingWeb3.Environment;
using Microsoft.Extensions.DependencyInjection;

namespace Web3Unity.Scripts.Library.Ethers.NetCore
{
    public static class NetCoreEnvironmentExtensions
    {
        public static IWeb3ServiceCollection UseNetCoreEnvironment(this IWeb3ServiceCollection services)
        {
            services.AddSingleton<Web3Environment>();
            services.AddSingleton<IHttpClient, NetCoreHttpClient>();
            services.AddSingleton<ILogWriter, NetCoreLogWriter>();
            services.AddSingleton<IAnalyticsClient, NetCoreAnalytics>();
            services.AddSingleton<ISettingsProvider, NetCoreSettingsProvider>();
            return services;
        }
    }
}