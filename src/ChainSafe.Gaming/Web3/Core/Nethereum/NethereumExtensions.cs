using ChainSafe.Gaming.Web3.Build;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.Web3.Core.Nethereum
{
    public static class NethereumExtensions
    {
        public static IWeb3ServiceCollection UseNethereumAdapters(this IWeb3ServiceCollection services)
        {
            services.AddSingleton<INethereumWeb3Adapter, ILifecycleParticipant, NethereumWeb3Adapter>();
            services.AddSingleton<NethereumSignerAdapter>();

            return services;
        }

        public static bool IsNethereumAdaptersBound(this IWeb3ServiceCollection services)
        {
            return services.IsBound<INethereumWeb3Adapter>();
        }
    }
}