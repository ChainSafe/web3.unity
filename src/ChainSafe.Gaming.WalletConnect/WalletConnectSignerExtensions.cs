using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.WalletConnect
{
    public static class WalletConnectSignerExtensions
    {
        private static readonly WalletConnectConfig DefaultConfig = new();

        /// <summary>
        /// Binds Web implementation of EVM Provider to Web3.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseWalletConnectSigner(this IWeb3ServiceCollection collection, WalletConnectConfig configuration)
        {
            collection.UseWalletConnectSigner();
            collection.ConfigureWalletConnectSigner(configuration);
            return collection;
        }

        /// <summary>
        /// Binds Web implementation of EVM Provider to Web3.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseWalletConnectSigner(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<ISigner>();

            // config
            collection.TryAddSingleton(DefaultConfig);

            // wallet
            collection.AddSingleton<ISigner, ILifecycleParticipant, WalletConnectSigner>();

            return collection;
        }

        /// <summary>
        /// Configures Web implementation of EVM Provider.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection ConfigureWalletConnectSigner(this IWeb3ServiceCollection collection, WalletConnectConfig configuration)
        {
            collection.Replace(ServiceDescriptor.Singleton(typeof(WalletConnectConfig), configuration));
            return collection;
        }
    }
}