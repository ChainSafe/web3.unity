using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.WalletConnect
{
    public static class WalletConnectProviderExtensions
    {
        /// <summary>
        /// Binds Web implementation of EVM Provider to Web3.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseWalletConnectProvider(this IWeb3ServiceCollection collection, WalletConnectConfig config)
        {
            collection.AssertServiceNotBound<IWalletConnectProvider>();

            // wallet
            collection.AddSingleton<IWalletConnectProvider, ILifecycleParticipant, WalletConnectProvider>();

            // configure provider
            collection.Replace(ServiceDescriptor.Singleton(typeof(WalletConnectConfig), config));
            return collection;
        }
    }
}