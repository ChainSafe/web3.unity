using ChainSafe.GamingWeb3.Build;
using Microsoft.Extensions.DependencyInjection;
using Web3Unity.Scripts.Library.Ethers.Signers;

namespace ChainSafe.GamingSDK.EVM.MetaMaskBrowserWallet
{
    public static class MetaMaskBrowserExtensions
    {
        /// <summary>
        /// Binds Web implementation of EVM Provider to Web3
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseWebSigner(this IWeb3ServiceCollection collection, MetaMaskBrowserSignerConfiguration configuration)
        {
            collection.ConfigureWebSigner(configuration);
            collection.UseWebSigner();
            return collection;
        }

        /// <summary>
        /// Binds Web implementation of EVM Provider to Web3
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseWebSigner(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<IEvmSigner>();
            collection.AddSingleton<IEvmSigner, MetaMaskBrowserSigner>();
            return collection;
        }

        /// <summary>
        /// Configures Web implementation of EVM Provider
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection ConfigureWebSigner(this IWeb3ServiceCollection collection, MetaMaskBrowserSignerConfiguration configuration)
        {
            collection.AssertConfigurationNotBound<MetaMaskBrowserSignerConfiguration>();
            collection.AddSingleton(configuration);
            return collection;
        }
    }
}