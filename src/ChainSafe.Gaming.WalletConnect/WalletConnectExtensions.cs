using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.WalletConnect
{
    /// <summary>
    /// <see cref="WalletConnectCustomProvider"/>, <see cref="WalletConnectSigner"/> and <see cref="WalletConnectTransactionExecutor"/> extension methods.
    /// </summary>
    public static class WalletConnectExtensions
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

        /// <summary>
        /// Binds implementation of <see cref="ISigner"/> as <see cref="WalletConnectSigner"/> to Web3 as a service.
        /// </summary>
        /// <param name="collection">Service collection to bind implementations to.</param>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseWalletConnectSigner(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<ISigner>();

            // wallet
            collection.AddSingleton<ISigner, ILifecycleParticipant, WalletConnectSigner>();

            return collection;
        }

        /// <summary>
        /// Binds implementation of <see cref="ITransactionExecutor"/> as <see cref="WalletConnectTransactionExecutor"/> to Web3 as a service.
        /// </summary>
        /// <param name="collection">Service collection to bind implementations to.</param>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseWalletConnectTransactionExecutor(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<ITransactionExecutor>();

            collection.AddSingleton<ITransactionExecutor, ILifecycleParticipant, WalletConnectTransactionExecutor>();

            return collection;
        }
    }
}