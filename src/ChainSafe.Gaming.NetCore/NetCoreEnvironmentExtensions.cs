using ChainSafe.Gaming.Analytics;
using ChainSafe.Gaming.Build;
using ChainSafe.Gaming.Environment;
using Microsoft.Extensions.DependencyInjection;

namespace Web3Unity.Scripts.Library.Ethers.NetCore
{
    public static class NetCoreEnvironmentExtensions
    {
        public static IWeb3ServiceCollection UseNetCoreEnvironment(this IWeb3ServiceCollection services)
        {
            services.UseApiAnalytics();
            services.AddSingleton<Web3Environment>();
            services.AddSingleton<IHttpClient, NetCoreHttpClient>();
            services.AddSingleton<ILogWriter, NetCoreLogWriter>();
            services.AddSingleton<IOperatingSystemMediator, NetCoreOperatingSystemMediator>();
            return services;
        }
    }
}