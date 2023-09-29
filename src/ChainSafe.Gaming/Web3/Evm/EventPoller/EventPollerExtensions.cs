using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Evm.EventPoller;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.Web3.Core.Evm.EventPoller
{
    public static class EventPollerExtensions
    {
        internal static IWeb3ServiceCollection UseEventPoller(this IWeb3ServiceCollection services)
        {
            return services
                    .AddSingleton<EventPollerConfiguration>()
                    .AddSingleton<IEvmEvents, EventPoller>()
                as IWeb3ServiceCollection;
        }

        public static IWeb3ServiceCollection ConfigureEventPoller(this IWeb3ServiceCollection services, EventPollerConfiguration eventPollerConfiguration)
        {
            return services
                    .Replace(ServiceDescriptor.Singleton(eventPollerConfiguration))
                as IWeb3ServiceCollection;
        }
    }
}