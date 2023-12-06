using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;

namespace ChainSafe.Gaming.MetaMask.Unity
{
    /// <summary>
    /// <see cref="MetaMaskTransactionExecutor"/> extension methods.
    /// </summary>
    public static class MetaMaskTransactionExecutorExtensions
    {
        /// <summary>
        /// Binds implementation of <see cref="ITransactionExecutor"/> as <see cref="MetaMaskTransactionExecutor"/> to Web3 as a service.
        /// </summary>
        /// <param name="collection">Service collection to bind implementations to.</param>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseMetaMaskTransactionExecutor(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<ITransactionExecutor>();

            // wallet
            collection.AddSingleton<ITransactionExecutor, ILifecycleParticipant, MetaMaskTransactionExecutor>();

            return collection;
        }
    }
}