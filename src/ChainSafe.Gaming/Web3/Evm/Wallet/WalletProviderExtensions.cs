using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Chains;
using ChainSafe.Gaming.Web3.Core.Evm;
using ChainSafe.Gaming.Web3.Core.Logout;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.Web3.Evm.Wallet
{
    public static class WalletProviderExtensions
    {
        /// <summary>
        /// Inject a Wallet Provider for <see cref="IWalletProvider"/>.
        /// </summary>
        /// <param name="collection">Collection of services.</param>
        /// <param name="config">Config for wallet connection, injected <see cref="IWalletProviderConfig"/>.</param>
        /// <typeparam name="TProvider">Concrete Type of provider to be injected.</typeparam>
        /// <returns>Collection of services with services injected.</returns>
        public static IWeb3ServiceCollection UseWalletProvider<TProvider>(this IWeb3ServiceCollection collection, IWalletProviderConfig config)
            where TProvider : WalletProvider
        {
            collection.AssertServiceNotBound<IWalletProvider>();

            collection.AddSingleton<IWalletProvider, IChainSwitchHandler, TProvider>();

            collection.Replace(ServiceDescriptor.Singleton(typeof(IWalletProviderConfig), config));

            return collection;
        }

        /// <summary>
        /// Inject <see cref="WalletSigner"/> for <see cref="ISigner"/>.
        /// </summary>
        /// <param name="collection">Collection of registered services.</param>
        /// <returns>Collection of registered services with a registered <see cref="ISigner"/>.</returns>
        public static IWeb3ServiceCollection UseWalletSigner(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<ISigner>();

            collection.AddSingleton<ISigner, ILifecycleParticipant, ILogoutHandler, WalletSigner>();

            return collection;
        }

        /// <summary>
        /// Inject <see cref="WalletTransactionExecutor"/> for <see cref="ITransactionExecutor"/>.
        /// </summary>
        /// <param name="collection">Collection of registered services.</param>
        /// <returns>Collection of registered services with a registered <see cref="ITransactionExecutor"/>.</returns>
        public static IWeb3ServiceCollection UseWalletTransactionExecutor(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<ITransactionExecutor>();

            collection.AddSingleton<ITransactionExecutor, WalletTransactionExecutor>();

            return collection;
        }
    }
}