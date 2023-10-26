using ChainSafe.Gaming.Web3.Analytics;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Environment;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.NetCore
{
    /// <summary>
    /// NetCore environment extension.
    /// Binds implementations to Web3 as a service for NetCore environment.
    /// </summary>
    public static class NetCoreEnvironmentExtensions
    {
        /// <summary>
        /// Binds implementation of <see cref="IHttpClient"/>, <see cref="ILogWriter"/> and <see cref="IOperatingSystemMediator"/> to Web3 as a service for NetCore environment.
        /// </summary>
        /// <param name="services">Service collection to bind implementations to.</param>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
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