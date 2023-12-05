using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;

namespace ChainSafe.Gaming.MetaMask.Unity
{
    public static class MetaMaskTransactionExecutorExtensions
    {
        public static IWeb3ServiceCollection UseMetaMaskTransactionExecutor(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<ITransactionExecutor>();

            // wallet
            collection.AddSingleton<ITransactionExecutor, ILifecycleParticipant, MetaMaskTransactionExecutor>();

            return collection;
        }
    }
}