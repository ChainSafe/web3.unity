using System.Linq;
using ChainSafe.Gaming.Build;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.Analytics
{
    public static class ApiAnalyticsClientExtensions
    {
        public static IWeb3ServiceCollection UseApiAnalytics(this IWeb3ServiceCollection serviceCollection)
        {
            if (serviceCollection.AnalyticsDisabled())
            {
                return serviceCollection;
            }

            serviceCollection.Replace(ServiceDescriptor.Singleton<IAnalyticsClient, ApiAnalyticsClient>());
            return serviceCollection;
        }

        public static bool AnalyticsDisabled(this IWeb3ServiceCollection serviceCollection)
        {
            return serviceCollection.Any(d =>
                d.ServiceType == typeof(IAnalyticsClient)
                && d.ImplementationType == typeof(NoOpAnalyticsClient));
        }
    }
}