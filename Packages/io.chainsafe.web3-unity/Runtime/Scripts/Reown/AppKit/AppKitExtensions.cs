using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using ChainSafe.Gaming.Web3.Core.Logout;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.Reown.AppKit
{
    public static class AppKitExtensions
    {
        public static IWeb3ServiceCollection UseAppKit(
            this IWeb3ServiceCollection services,
            IReownConfig config)
        {
            services.AssertServiceNotBound<IWalletProvider>();
            services.ConfigureReown(config);
            services.AddSingleton<IConnectionHelper, ILifecycleParticipant, AppKitProvider>();
            services.AddSingleton<ReownHttpClient>();
            services.UseWalletProvider<AppKitProvider>(config);
            return services;
        }


    }
}