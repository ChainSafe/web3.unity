using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.LocalStorage;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using ChainSafe.Gaming.Web3.Core.Logout;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.HyperPlay
{
    public static class HyperPlayExtensions
    {
        /// <summary>
        /// Binds implementation of <see cref="IWalletProvider"/> as <see cref="HyperPlayProvider"/> to Web3 as a service.
        /// </summary>
        /// <param name="collection">Service collection to bind implementations to.</param>
        /// <param name="config">Config for connecting via HyperPlay.</param>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseHyperPlay(this IWeb3ServiceCollection collection, IHyperPlayConfig config)
        {
            collection.AddSingleton<IHyperPlayData, IStorable, HyperPlayData>();

            collection.UseWalletProvider<HyperPlayProvider>(config);

            return collection;
        }
    }
}