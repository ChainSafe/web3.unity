using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;

namespace ChainSafe.Gaming.WalletConnect
{
    /// <summary>
    /// <see cref="WalletConnectTransactionExecutor"/> extension methods.
    /// </summary>
    public static class WalletConnectTransactionExecutorExtensions
    {
        /// <summary>
        /// Binds implementation of <see cref="ITransactionExecutor"/> as <see cref="WalletConnectTransactionExecutor"/> to Web3 as a service.
        /// </summary>
        /// <param name="collection">Service collection to bind implementations to.</param>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseWalletConnectTransactionExecutor(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<ITransactionExecutor>();

            collection.AddSingleton<ITransactionExecutor, ILifecycleParticipant, WalletConnectTransactionExecutor>();

            return collection;
        }
    }
}