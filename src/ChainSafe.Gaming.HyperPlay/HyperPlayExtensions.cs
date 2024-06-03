using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.HyperPlay
{
    public static class HyperPlayExtensions
    {
        /// <summary>
        /// Binds implementation of <see cref="IWalletProvider"/> as <see cref="HyperPlayProvider"/> to Web3 as a service.
        /// </summary>
        /// <param name="collection">Service collection to bind implementations to.</param>
        /// <param name="config">Config for wallet provider.</param>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseHyperPlay(this IWeb3ServiceCollection collection, HyperPlayConfig config = null)
        {
            config ??= new HyperPlayConfig();

            return collection.UseWalletProvider<HyperPlayProvider>(config);
        }
    }
}