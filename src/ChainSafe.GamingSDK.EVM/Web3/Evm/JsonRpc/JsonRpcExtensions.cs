using ChainSafe.GamingWeb3.Build;
using Microsoft.Extensions.DependencyInjection;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;

namespace Web3Unity.Scripts.Library.Ethers.JsonRpc
{
    public static class JsonRpcExtensions
    {
        /// <summary>
        /// Binds JSON RPC implementation of EVM Provider to Web3
        /// </summary>
        public static void UseJsonRpcProvider(this IWeb3ServiceCollection serviceCollection, JsonRpcProviderConfiguration configuration)
        {
            serviceCollection.ConfigureJsonRpcProvider(configuration);
            serviceCollection.UseJsonRpcProvider();
        }

        /// <summary>
        /// Configures JSON RPC implementation of EVM Provider
        /// </summary>
        public static void ConfigureJsonRpcProvider(this IWeb3ServiceCollection serviceCollection, JsonRpcProviderConfiguration configuration)
        {
            serviceCollection.AssertConfigurationNotBound<JsonRpcProviderConfiguration>();
            serviceCollection.AddSingleton(configuration);
        }

        /// <summary>
        /// Binds JSON RPC implementation of EVM Provider to Web3
        /// </summary>
        public static void UseJsonRpcProvider(this IWeb3ServiceCollection serviceCollection)
        {
            serviceCollection.AssertServiceNotBound<IEvmProvider>();
            serviceCollection.AddSingleton<IEvmProvider, JsonRpcProvider>();
        }

        /// <summary>
        /// Binds JSON RPC implementation of EVM Wallet to Web3
        /// </summary>
        public static void UseJsonRpcSigner(this IWeb3ServiceCollection serviceCollection, JsonRpcSignerConfiguration configuration)
        {
            serviceCollection.ConfigureJsonRpcSigner(configuration);
            serviceCollection.UseJsonRpcSigner();
        }

        /// <summary>
        /// Configures JSON RPC implementation of EVM Wallet
        /// </summary>
        public static void ConfigureJsonRpcSigner(this IWeb3ServiceCollection serviceCollection, JsonRpcSignerConfiguration configuration)
        {
            serviceCollection.AssertConfigurationNotBound<JsonRpcSignerConfiguration>();
            serviceCollection.AddSingleton(configuration);
        }

        /// <summary>
        /// Binds JSON RPC implementation of EVM Wallet to Web3
        /// </summary>
        public static void UseJsonRpcSigner(this IWeb3ServiceCollection serviceCollection)
        {
            serviceCollection.AssertServiceNotBound<IEvmSigner>();
            serviceCollection.AddSingleton<IEvmSigner, JsonRpcSigner>();
        }
    }
}