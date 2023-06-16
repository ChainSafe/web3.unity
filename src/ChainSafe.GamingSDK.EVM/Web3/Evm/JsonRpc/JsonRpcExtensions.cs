using ChainSafe.GamingSDK.EVM.Web3.Core;
using ChainSafe.GamingWeb3.Build;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;

namespace Web3Unity.Scripts.Library.Ethers.JsonRpc
{
    public static class JsonRpcExtensions
    {
        private static readonly JsonRpcProviderConfig DefaultProviderConfig = new();

        /// <summary>
        /// Binds JSON RPC implementation of EVM Provider to Web3.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseJsonRpcProvider(this IWeb3ServiceCollection collection, JsonRpcProviderConfig config)
        {
            collection.ConfigureJsonRpcProvider(config);
            collection.UseJsonRpcProvider();
            return collection;
        }

        /// <summary>
        /// Configures JSON RPC implementation of EVM Provider.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection ConfigureJsonRpcProvider(this IWeb3ServiceCollection collection, JsonRpcProviderConfig config)
        {
            collection.Replace(ServiceDescriptor.Singleton(config));
            return collection;
        }

        /// <summary>
        /// Binds JSON RPC implementation of EVM Provider to Web3.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseJsonRpcProvider(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<IRpcProvider>();
            collection.TryAddSingleton(DefaultProviderConfig);
            collection.AddSingleton<IRpcProvider, ILifecycleParticipant, JsonRpcProvider>();
            return collection;
        }
    }
}