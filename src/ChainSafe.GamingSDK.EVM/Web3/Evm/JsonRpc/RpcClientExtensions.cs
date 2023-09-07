using ChainSafe.GamingSDK.EVM.Web3.Core;
using ChainSafe.GamingWeb3.Build;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Web3Unity.Scripts.Library.Ethers.Providers;

namespace Web3Unity.Scripts.Library.Ethers.JsonRpc
{
    public static class RpcClientExtensions
    {
        private static readonly RpcClientConfig DefaultClientConfig = new();

        /// <summary>
        /// Binds JSON RPC implementation of EVM Provider to Web3.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseRpcProvider(this IWeb3ServiceCollection collection, RpcClientConfig config)
        {
            collection.ConfigureRpcProvider(config);
            collection.UseRpcProvider();
            return collection;
        }

        /// <summary>
        /// Configures JSON RPC implementation of EVM Provider.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection ConfigureRpcProvider(this IWeb3ServiceCollection collection, RpcClientConfig config)
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
            collection.TryAddSingleton(DefaultClientConfig);
            collection.AddSingleton<IRpcProvider, ILifecycleParticipant, RpcClientProvider>();
            return collection;
        }
    }
}