using ChainSafe.Gaming.Web3.Analytics;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Environment;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.Tests.Core
{
    public static class StubWeb3EnvironmentExtensions
    {
        public static IWeb3ServiceCollection UseStubWeb3Environment(
            this IWeb3ServiceCollection services,
            IHttpClient httpClient = null,
            ILogWriter logWriter = null,
            IAnalyticsClient analyticsClient = null,
            IOperatingSystemMediator operatingSystemMediator = null)
        {
            services.AddSingleton<Web3Environment>();
            services.AddSingleton(httpClient ?? new StubHttpClient());
            services.AddSingleton(logWriter ?? new TestLogWriter());
            services.AddSingleton(analyticsClient ?? new StubAnalyticsClient());
            services.AddSingleton(operatingSystemMediator ?? new StubOperatingSystemMediator());
            return services;
        }
    }
}