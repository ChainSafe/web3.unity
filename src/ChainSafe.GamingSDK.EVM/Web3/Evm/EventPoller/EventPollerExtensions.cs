using ChainSafe.GamingSDK.EVM.Web3.Core.Evm;
using ChainSafe.GamingWeb3.Build;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.GamingSDK.EVM.Web3.Evm.EventPoller
{
    public static class EventPollerExtensions
    {
        internal static IWeb3ServiceCollection UseEventPoller(this IWeb3ServiceCollection services) =>
            services
                .AddSingleton<EventPollerConfiguration>()
                .AddSingleton<IEvmEvents, EventPoller>()
            as IWeb3ServiceCollection;

        public static IWeb3ServiceCollection ConfigureEventPoller(this IWeb3ServiceCollection services, EventPollerConfiguration eventPollerConfiguration) =>
            services
                .Replace(ServiceDescriptor.Singleton(eventPollerConfiguration))
            as IWeb3ServiceCollection;
    }
}
