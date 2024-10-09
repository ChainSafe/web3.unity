using ChainSafe.Gaming.Unity;
using ChainSafe.Gaming.Web3.Analytics;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core.Unity;
using ChainSafe.Gaming.Web3.Environment;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.Web3.Unity
{
    public static class UnityEnvironmentExtensions
    {
        /// <summary>
        /// Registers Unity specific services for the Web3 environment.
        /// </summary>
        /// <param name="services">Service collection that is already in use.</param>
        /// <returns>Updated Service collection.</returns>
        public static IWeb3ServiceCollection UseUnityEnvironment(this IWeb3ServiceCollection services)
        {
            services.AddSingleton<Web3Environment>();
            services.AddSingleton<IMainThreadRunner, UnityDispatcherAdapter>();
            services.AddSingleton<IHttpClient, UnityHttpClient>();
            services.AddSingleton<ILogWriter, UnityLogWriter>();
            services.AddSingleton<IOperatingSystemMediator, UnityOperatingSystemMediator>();
#if ENABLE_ANALYTICS
            services.AddSingleton<IAnalyticsClient, CountlyAnalytics>();
#else
            services.DisableAnalytics();
#endif
            return services;
        }
    }
}