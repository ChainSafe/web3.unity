using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using ChainSafe.Gaming.Web3.Core.Logout;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.EmbeddedWallet
{
    /// <summary>
    /// Extension methods for Embedded Wallet.
    /// </summary>
    public static class EmbeddedWalletExtensions
    {
        /// <summary>
        /// Use Embedded Wallet as signer and transaction executor.
        /// </summary>
        /// <param name="services">Service collection to bind implementations to.</param>
        /// <param name="config">Configuration for the embedded wallet.</param>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseEmbeddedWallet(this IWeb3ServiceCollection services, IEmbeddedWalletConfig config)
        {
            services.AddSingleton(_ => config);

            services.AddSingleton<EmbeddedWalletRequestHandler>();

            services.AddSingleton<ISigner, ILifecycleParticipant, ILogoutHandler, EmbeddedWalletSigner>();

            services.AddSingleton<ITransactionExecutor, EmbeddedWalletTransactionExecutor>();

            return services;
        }
    }
}