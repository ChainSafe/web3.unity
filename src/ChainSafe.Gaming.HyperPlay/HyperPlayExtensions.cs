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
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseHyperPlay(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<IWalletProvider>();

            collection.AddSingleton<IWalletProvider, HyperPlayProvider>();

            return collection;
        }

        /// <summary>
        /// Binds implementation of <see cref="ISigner"/> as <see cref="HyperPlaySigner"/> to Web3 as a service.
        /// </summary>
        /// <param name="collection">Service collection to bind implementations to.</param>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseHyperPlaySigner(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<ISigner>();

            collection.AddSingleton<ILifecycleParticipant, ISigner, HyperPlaySigner>();

            return collection;
        }

        /// <summary>
        /// Binds implementation of <see cref="ITransactionExecutor"/> as <see cref="HyperPlayTransactionExecutor"/> to Web3 as a service.
        /// </summary>
        /// <param name="collection">Service collection to bind implementations to.</param>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseHyperPlayTransactionExecutor(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<ITransactionExecutor>();

            collection.AddSingleton<ITransactionExecutor, HyperPlayTransactionExecutor>();

            return collection;
        }
    }
}