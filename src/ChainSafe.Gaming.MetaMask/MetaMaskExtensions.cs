using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.MetaMask
{
    /// <summary>
    /// <see cref="MetaMaskSigner"/> and <see cref="MetaMaskTransactionExecutor"/> extension methods.
    /// </summary>
    public static class MetaMaskExtensions
    {
        /// <summary>
        /// Binds implementation of <see cref="ISigner"/> as <see cref="MetaMaskSigner"/> to Web3 as a service.
        /// </summary>
        /// <param name="collection">Service collection to bind implementations to.</param>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseMetaMaskSigner(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<ISigner>();

            // wallet
            collection.AddSingleton<ISigner, ILifecycleParticipant, MetaMaskSigner>();

            return collection;
        }

        /// <summary>
        /// Binds implementation of <see cref="ITransactionExecutor"/> as <see cref="MetaMaskTransactionExecutor"/> to Web3 as a service.
        /// </summary>
        /// <param name="collection">Service collection to bind implementations to.</param>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseMetaMaskTransactionExecutor(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<ITransactionExecutor>();

            // wallet
            collection.AddSingleton<ITransactionExecutor, MetaMaskTransactionExecutor>();

            return collection;
        }
    }
}