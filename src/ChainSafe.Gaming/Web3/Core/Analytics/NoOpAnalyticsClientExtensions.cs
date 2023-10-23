using System.Linq;
using ChainSafe.Gaming.Web3.Build;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.Web3.Analytics
{
    public static class NoOpAnalyticsClientExtensions
    {
        /// <summary>
        /// Disables analytics for the <see cref="Web3"/> instance.
        /// </summary>
        /// <param name="serviceCollection">The Web3 service collection.</param>
        /// <returns>Service collection to enable fluent syntax.</returns>
        public static IWeb3ServiceCollection DisableAnalytics(this IWeb3ServiceCollection serviceCollection)
        {
            serviceCollection.Replace(ServiceDescriptor.Singleton<IAnalyticsClient, NoOpAnalyticsClient>());
            return serviceCollection;
        }

        /// <summary>
        /// Returns true if analytics are disabled.
        /// </summary>
        /// <param name="serviceCollection">The Web3 service collection.</param>
        /// <returns>True if analytics are disabled.</returns>
        public static bool AnalyticsDisabled(this IWeb3ServiceCollection serviceCollection)
        {
            return serviceCollection.Any(d =>
                d.ServiceType == typeof(IAnalyticsClient)
                && d.ImplementationType == typeof(NoOpAnalyticsClient));
        }
    }
}