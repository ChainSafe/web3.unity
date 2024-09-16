using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Chains;

namespace ChainSafe.Gaming.RPC.Events
{
    public static class EventExtensions
    {
        public static IWeb3ServiceCollection UseEvents(this IWeb3ServiceCollection services)
        {
            // todo bind EventPoller implementation of IEventManager when running in WebGL build
            return services
                .AddSingleton<IEventManager, ILifecycleParticipant, IChainSwitchHandler, EventManager>();
        }
    }
}