using ChainSafe.GamingSdk.Evm.Unity;
using ChainSafe.GamingSdk.EVM.Unity;
using ChainSafe.GamingWeb3.Build;
using ChainSafe.GamingWeb3.Environment;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.GamingWeb3.Unity
{
    public static class UnityEnvironmentExtensions
    {
        public static IWeb3ServiceCollection UseUnityEnvironment(this IWeb3ServiceCollection services, UnityEnvironmentConfiguration configuration)
        {
            services.UseUnityEnvironment();
            services.ConfigureUnityEnvironment(configuration);
            return services;
        }

        public static IWeb3ServiceCollection UseUnityEnvironment(this IWeb3ServiceCollection services)
        {
            services.AddSingleton<Web3Environment>();
            services.AddSingleton<IMainThreadRunner, UnityDispatcherAdapter>();
            services.AddSingleton<IHttpClient, UnityHttpClient>();
            services.AddSingleton<ILogWriter, UnityLogWriter>();
            services.AddSingleton<IAnalyticsClient, DataDogAnalytics>();
            services.AddSingleton<ISettingsProvider, UnitySettingsProvider>();
            return services;
        }

        public static IWeb3ServiceCollection ConfigureUnityEnvironment(this IWeb3ServiceCollection services, UnityEnvironmentConfiguration configuration)
        {
            services.AddSingleton(configuration.DataDog);
            return services;
        }
    }
}