using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core.Evm;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.EmbeddedWallet
{
    public static class EmbeddedWalletExtensions
    {
        public static IWeb3ServiceCollection UseEmbeddedWallet(this IWeb3ServiceCollection services, IEmbeddedWalletConfig config)
        {
            services.AddSingleton(_ => config);

            services.AddSingleton<TransactionPool>();

            services.AddSingleton<ITransactionExecutor, IEmbeddedWalletTransactionHandler, EmbeddedWalletTransactionExecutor>();

            return services;
        }
    }
}