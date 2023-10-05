using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.Web3.Evm.JsonRpc
{
    public static class TransactionExecutorExtension
    {
        public static IWeb3ServiceCollection UseJsonRpcTransactionExecutor(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<ITransactionExecutor>();
            collection.AddSingleton<ITransactionExecutor, ILifecycleParticipant, TransactionExecutor>();
            return collection;
        }
    }
}