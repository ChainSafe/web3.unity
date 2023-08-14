using ChainSafe.GamingSDK.EVM.Web3.Core.Evm;
using ChainSafe.GamingWeb3.Build;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.GamingSdk.EVM.InProcessTransactionExecutor
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
