using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core.Evm;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.Web3.Core.Nethereum
{
    public static class NethereumExtensions
    {
        public static IWeb3ServiceCollection UseNethereumAdapters(this IWeb3ServiceCollection services)
        {
            services.AddSingleton<INethereumWeb3Adapter, NethereumWeb3Adapter>();

            // build Adapters for Writing too if we can
            if (services.IsBound<ISigner>() && services.IsBound<ITransactionExecutor>())
            {
                services.AddSingleton<NethereumAccountAdapter>();
            }

            return services;
        }

        public static bool IsNethereumAdaptersBound(this IWeb3ServiceCollection services)
        {
            return services.IsBound<INethereumWeb3Adapter>();
        }
    }
}