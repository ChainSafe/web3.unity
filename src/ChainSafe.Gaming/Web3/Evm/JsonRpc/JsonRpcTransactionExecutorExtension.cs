using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;

namespace ChainSafe.Gaming.Web3.Evm.JsonRpc
{
    public static class JsonRpcTransactionExecutorExtension
    {
        public static IWeb3ServiceCollection UseJsonRpcTransactionExecutor(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<ITransactionExecutor>();
            collection.AddSingleton<ITransactionExecutor, ILifecycleParticipant, JsonRpcTransactionExecutor>();
            return collection;
        }
    }
}