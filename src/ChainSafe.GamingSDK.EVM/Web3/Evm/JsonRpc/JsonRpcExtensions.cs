using ChainSafe.GamingWeb3.Build;
using Microsoft.Extensions.DependencyInjection;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;

namespace Web3Unity.Scripts.Library.Ethers.JsonRpc
{
    public static class JsonRpcExtensions
    {
        /// <summary>
        /// Binds JSON RPC implementation of EVM Provider to Web3.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseJsonRpcProvider(this IWeb3ServiceCollection serviceCollection, JsonRpcProviderConfiguration configuration)
        {
            serviceCollection.ConfigureJsonRpcProvider(configuration);
            serviceCollection.UseJsonRpcProvider();
            return serviceCollection;
        }

        /// <summary>
        /// Configures JSON RPC implementation of EVM Provider.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection ConfigureJsonRpcProvider(this IWeb3ServiceCollection serviceCollection, JsonRpcProviderConfiguration configuration)
        {
            serviceCollection.AssertConfigurationNotBound<JsonRpcProviderConfiguration>();
            serviceCollection.AddSingleton(configuration);
            return serviceCollection;
        }

        /// <summary>
        /// Binds JSON RPC implementation of EVM Provider to Web3.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseJsonRpcProvider(this IWeb3ServiceCollection serviceCollection)
        {
            serviceCollection.AssertServiceNotBound<IEvmProvider>();
            serviceCollection.AddSingleton<IEvmProvider, JsonRpcProvider>();
            return serviceCollection;
        }

        /// <summary>
        /// Binds JSON RPC implementation of EVM Wallet to Web3.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseJsonRpcSigner(this IWeb3ServiceCollection serviceCollection, JsonRpcSignerConfiguration configuration)
        {
            serviceCollection.ConfigureJsonRpcSigner(configuration);
            serviceCollection.UseJsonRpcSigner();
            return serviceCollection;
        }

        /// <summary>
        /// Configures JSON RPC implementation of EVM Wallet.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection ConfigureJsonRpcSigner(this IWeb3ServiceCollection serviceCollection, JsonRpcSignerConfiguration configuration)
        {
            serviceCollection.AssertConfigurationNotBound<JsonRpcSignerConfiguration>();
            serviceCollection.AddSingleton(configuration);
            return serviceCollection;
        }

        /// <summary>
        /// Binds JSON RPC implementation of EVM Wallet to Web3.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseJsonRpcSigner(this IWeb3ServiceCollection serviceCollection)
        {
            serviceCollection.AssertServiceNotBound<IEvmSigner>();
            serviceCollection.AddSingleton<IEvmSigner, JsonRpcSigner>();
            return serviceCollection;
        }
    }
}