using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Chains;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.RPC.Events
{
    public static class EventExtensions
    {
        public static IWeb3ServiceCollection UseEventsWithWebSocket(this IWeb3ServiceCollection services)
        {
            services.AssertServiceNotBound<IEventManager>();
            services.AddSingleton<IEventManager, ILifecycleParticipant, IChainSwitchHandler, WebSocketEventManager>();
            return services;
        }

        public static IWeb3ServiceCollection UseEventsWithPolling(this IWeb3ServiceCollection services)
        {
            services.AssertServiceNotBound<IEventManager>();
            services.AddSingleton<IEventManager, IChainSwitchHandler, PollingEventManager>();
            return services;
        }

        public static IWeb3ServiceCollection UseEventsWithPolling(this IWeb3ServiceCollection services, PollingEventManagerConfig config)
        {
            services.AssertServiceNotBound<IEventManager>();
            services.AssertConfigurationNotBound<PollingEventManagerConfig>();

            services.ConfigureEventsWithPolling(config);
            services.UseEventsWithPolling();

            return services;
        }

        public static IWeb3ServiceCollection ConfigureEventsWithPolling(this IWeb3ServiceCollection services, PollingEventManagerConfig pollingConfig)
        {
            services.Replace(ServiceDescriptor.Singleton(pollingConfig));
            return services;
        }
    }
}