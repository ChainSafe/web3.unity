using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.WalletConnect
{
    /// <summary>
    /// <see cref="WalletConnectCustomProvider"/> extension methods.
    /// </summary>
    public static class WalletConnectProviderExtensions
    {
        /// <summary>
        /// Binds implementation of <see cref="IWalletConnectCustomProvider"/> as <see cref="WalletConnectCustomProvider"/> and <see cref="WalletConnectConfig"/> to Web3 as a service.
        /// </summary>
        /// <param name="collection">Service collection to bind implementations to.</param>
        /// <param name="config">Wallet Connect Configuration.</param>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseWalletConnect(this IWeb3ServiceCollection collection, WalletConnectConfig config)
        {
            collection.AssertServiceNotBound<IWalletConnectCustomProvider>();

            // wallet
            collection.AddSingleton<IWalletConnectCustomProvider, ILifecycleParticipant, WalletConnectCustomProvider>();

            // configure provider
            collection.Replace(ServiceDescriptor.Singleton(typeof(WalletConnectConfig), config));
            return collection;
        }
    }
}