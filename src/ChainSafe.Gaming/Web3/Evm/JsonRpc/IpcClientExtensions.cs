using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.Evm.JsonRpc
{
    public static class IpcClientExtensions
    {
        private static readonly IpcClientConfig DefaultClientConfig = new();

        /// <summary>
        /// Binds IPC Client implementation of EVM Provider to Web3.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseIpcProvider(this IWeb3ServiceCollection collection, IpcClientConfig config)
        {
            collection.ConfigureIpcProvider(config);
            collection.UseIpcProvider();
            return collection;
        }

        /// <summary>
        /// Configures IPC Client implementation of EVM Provider.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection ConfigureIpcProvider(this IWeb3ServiceCollection collection, IpcClientConfig config)
        {
            collection.Replace(ServiceDescriptor.Singleton(config));
            return collection;
        }

        /// <summary>
        /// Binds IPC Client implementation of EVM Provider to Web3.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseIpcProvider(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<IRpcProvider>();
            collection.TryAddSingleton(DefaultClientConfig);
            collection.AddSingleton<IRpcProvider, ILifecycleParticipant, IpcClientProvider>();
            return collection;
        }
    }
}