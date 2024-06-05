using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core.Evm;
using Microsoft.Extensions.DependencyInjection;
using Nethereum.JsonRpc.Client;

namespace ChainSafe.Gaming.InProcessTransactionExecutor.Unity
{
    /// <summary>
    /// Extension methods for <see cref="InProcessTransactionExecutor"/>.
    /// </summary>
    public static class InProcessTransactionExecutorExtensions
    {
        /// <summary>
        /// Binds implementation of <see cref="ITransactionExecutor"/> as <see cref="InProcessTransactionExecutor"/> to Web3 as a service.
        /// </summary>
        /// <param name="collection">Service collection to bind implementations to.</param>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseInProcessTransactionExecutor(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<ITransactionExecutor>();
            collection.AddSingleton<ITransactionExecutor, InProcessTransactionExecutor>();

            return collection;
        }
    }
}