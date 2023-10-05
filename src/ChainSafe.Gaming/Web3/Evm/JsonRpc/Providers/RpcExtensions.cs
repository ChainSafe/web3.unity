using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.Web3.Evm.JsonRpc.Providers
{
    public static class RpcExtensions
    {
        private static readonly RpcConfig DefaultConfig = new();

        /// <summary>
        /// Binds JSON RPC implementation of EVM Provider to Web3.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseRpcProvider(this IWeb3ServiceCollection collection, RpcConfig config)
        {
            collection.ConfigureRpcProvider(config);
            collection.UseRpcProvider();
            return collection;
        }

        /// <summary>
        /// Configures JSON RPC implementation of EVM Provider.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection ConfigureRpcProvider(this IWeb3ServiceCollection collection, RpcConfig config)
        {
            collection.Replace(ServiceDescriptor.Singleton(config));
            return collection;
        }

        /// <summary>
        /// Binds JSON RPC implementation of EVM Provider to Web3.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseRpcProvider(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<IRpcProvider>();
            collection.TryAddSingleton(DefaultConfig);
            collection.AddSingleton<IRpcProvider, ILifecycleParticipant, RpcProvider>();
            return collection;
        }
    }
}