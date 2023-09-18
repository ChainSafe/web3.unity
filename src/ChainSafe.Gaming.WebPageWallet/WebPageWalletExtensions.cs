using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.Wallets
{
    public static class WebPageWalletExtensions
    {
        private static readonly WebPageWalletConfig DefaultConfig = new();

        /// <summary>
        /// Binds Web implementation of EVM Provider to Web3.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseWebPageWallet(this IWeb3ServiceCollection collection, WebPageWalletConfig configuration)
        {
            collection.UseWebPageWallet();
            collection.ConfigureWebPageWallet(configuration);
            return collection;
        }

        /// <summary>
        /// Binds Web implementation of EVM Provider to Web3.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseWebPageWallet(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<ISigner>();
            collection.AssertServiceNotBound<ITransactionExecutor>();

            // config
            collection.TryAddSingleton(DefaultConfig);

            // wallet
            collection.AddSingleton<ISigner, ITransactionExecutor, ILifecycleParticipant, WebPageWallet>();

            return collection;
        }

        /// <summary>
        /// Configures Web implementation of EVM Provider.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection ConfigureWebPageWallet(this IWeb3ServiceCollection collection, WebPageWalletConfig configuration)
        {
            collection.Replace(ServiceDescriptor.Singleton(typeof(WebPageWalletConfig), configuration));
            return collection;
        }
    }
}