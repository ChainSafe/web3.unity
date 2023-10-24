using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;

namespace ChainSafe.Gaming.WalletConnect
{
    public static class WalletConnectTransactionExecutorExtensions
    {
        /// <summary>
        /// Binds Web implementation of EVM Provider to Web3.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseWalletConnectTransactionExecutor(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<ITransactionExecutor>();

            collection.AddSingleton<ITransactionExecutor, ILifecycleParticipant, WalletConnectTransactionExecutor>();

            return collection;
        }
    }
}