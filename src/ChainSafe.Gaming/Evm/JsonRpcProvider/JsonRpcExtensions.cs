using ChainSafe.Gaming.Build;
using ChainSafe.Gaming.Lifecycle;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.Evm.JsonRpcProvider
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