using ChainSafe.Gaming.Web3.Build;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.Web3.Analytics
{
    public static class NoOpAnalyticsClientExtensions
    {
        public static IWeb3ServiceCollection DisableAnalytics(this IWeb3ServiceCollection serviceCollection)
        {
            serviceCollection.Replace(ServiceDescriptor.Singleton<IAnalyticsClient, NoOpAnalyticsClient>());
            return serviceCollection;
        }
    }
}