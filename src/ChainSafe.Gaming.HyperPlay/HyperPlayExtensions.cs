using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.HyperPlay
{
    public static class HyperPlayExtensions
    {
        /// <summary>
        /// Use HyperPlay to connect to wallet, sign messages, transactions, and execute transactions.
        /// </summary>
        /// <param name="collection">Service collection.</param>
        public static void UseHyperPlay(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<IHyperPlayProvider>();

            collection.AddSingleton<IHyperPlayProvider, HyperPlayProvider>();

            UseHyperPlaySigner(collection);

            UseHyperPlayTransactionExecutor(collection);
        }

        private static void UseHyperPlaySigner(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<ISigner>();

            collection.AddSingleton<ILifecycleParticipant, ISigner, HyperPlaySigner>();
        }

        private static void UseHyperPlayTransactionExecutor(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<ITransactionExecutor>();

            collection.AddSingleton<ITransactionExecutor, HyperPlayTransactionExecutor>();
        }
    }
}