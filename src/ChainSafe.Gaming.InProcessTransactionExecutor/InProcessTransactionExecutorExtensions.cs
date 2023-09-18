using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core.Evm;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.InProcessTransactionExecutor
{
    public static class InProcessTransactionExecutorExtensions
    {
        public static IWeb3ServiceCollection UseInProcessSigner(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<ITransactionExecutor>();
            collection.AddSingleton<ITransactionExecutor, InProcessTransactionExecutor>();
            return collection;
        }
    }
}
