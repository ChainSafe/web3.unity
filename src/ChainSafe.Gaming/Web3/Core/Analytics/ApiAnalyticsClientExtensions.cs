using ChainSafe.Gaming.Web3.Build;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.Web3.Analytics
{
    public static class ApiAnalyticsClientExtensions
    {
        /// <summary>
        /// Binds <see cref="ApiAnalyticsClient"/> thereby enabling analytics.
        /// </summary>
        /// <returns>Service collection to enable fluent syntax.</returns>
        public static IWeb3ServiceCollection UseApiAnalytics(this IWeb3ServiceCollection serviceCollection)
        {
            if (serviceCollection.AnalyticsDisabled())
            {
                return serviceCollection;
            }

            serviceCollection.Replace(ServiceDescriptor.Singleton<IAnalyticsClient, ApiAnalyticsClient>());
            return serviceCollection;
        }
    }
}