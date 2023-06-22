using ChainSafe.Gaming.Analytics;
using ChainSafe.Gaming.Build;
using ChainSafe.Gaming.Environment;
using ChainSafe.Gaming.Unity.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.Unity.Environment
{
    public static class UnityEnvironmentExtensions
    {
        public static IWeb3ServiceCollection UseUnityEnvironment(this IWeb3ServiceCollection services)
        {
            services.UseApiAnalytics();
            services.AddSingleton<Web3Environment>();
            services.AddSingleton<IMainThreadRunner, UnityDispatcherAdapter>();
            services.AddSingleton<IHttpClient, UnityHttpClient>();
            services.AddSingleton<ILogWriter, UnityLogWriter>();
            services.AddSingleton<IOperatingSystemMediator, UnityOperatingSystemMediator>();
            return services;
        }
    }
}