using ChainSafe.Gaming.Web3.Analytics;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Environment;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.NetCore
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