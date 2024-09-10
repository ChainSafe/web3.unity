using System.Linq;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.WalletConnect.Storage;
using ChainSafe.Gaming.WalletConnect.Wallets;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using ChainSafe.Gaming.Web3.Core.Logout;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.WalletConnect
{
    public static class WalletConnectExtensions
    {
        /// <summary>
        /// Use this to enable WalletConnect functionality for this instance of Web3.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        private static IWeb3ServiceCollection UseWalletConnect(this IWeb3ServiceCollection services)
        {
            services.AddSingleton<DataStorage>();
            services.AddSingleton<IWalletRegistry, ILifecycleParticipant, WalletRegistry>();
            services.AddSingleton<RedirectionHandler>();
            services.AddSingleton<IConnectionHelper, ILifecycleParticipant, WalletConnectProvider>();

            return services;
        }

        /// <summary>
        /// Use this to enable WalletConnect functionality for this instance of Web3.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseWalletConnect(this IWeb3ServiceCollection services, IWalletConnectConfig config)
        {
            services = UseWalletConnect(services);

            services.Replace(ServiceDescriptor.Singleton(typeof(IWalletConnectConfig), config));

            return services.UseWalletProvider<WalletConnectProvider>(config);
        }

        /// <summary>
        /// Access additional services related to WalletConnect.
        /// </summary>
        /// <returns>The WalletConnect subcategory of services.</returns>
        public static WalletConnectSubCategory WalletConnect(this Web3.Web3 web3)
        {
            return new WalletConnectSubCategory(web3);
        }

        /// <summary>
        /// Access the <see cref="IWalletRegistry"/> service.
        /// </summary>
        /// <returns>The <see cref="IWalletRegistry"/> service.</returns>
        public static IWalletRegistry WalletRegistry(this WalletConnectSubCategory walletConnect)
        {
            return ((IWeb3SubCategory)walletConnect).Web3.ServiceProvider.GetRequiredService<IWalletRegistry>();
        }

        /// <summary>
        /// Access the <see cref="RedirectionHandler"/> service.
        /// </summary>
        /// <returns>The <see cref="RedirectionHandler"/> service.</returns>
        public static RedirectionHandler RedirectionHandler(this WalletConnectSubCategory walletConnect)
        {
            return ((IWeb3SubCategory)walletConnect).Web3.ServiceProvider.GetRequiredService<RedirectionHandler>();
        }

        /// <summary>
        /// Access the <see cref="IConnectionHelper"/> service.
        /// </summary>
        /// <returns>The <see cref="IConnectionHelper"/> service.</returns>
        public static IConnectionHelper ConnectionHelper(this WalletConnectSubCategory walletConnect)
        {
            return ((IWeb3SubCategory)walletConnect).Web3.ServiceProvider.GetRequiredService<IConnectionHelper>();
        }
    }
}