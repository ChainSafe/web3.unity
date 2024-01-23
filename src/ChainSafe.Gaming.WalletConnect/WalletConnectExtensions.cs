using System.Linq;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.WalletConnect.Storage;
using ChainSafe.Gaming.WalletConnect.Wallets;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using ChainSafe.Gaming.Web3.Core.Logout;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.WalletConnect
{
    public static class WalletConnectExtensions
    {
        /// <summary>
        /// Use this to configure WalletConnect functionality for this instance of <see cref="Web3.Web3"/>.
        /// </summary>
        public static IWeb3ServiceCollection ConfigureWalletConnect(this IWeb3ServiceCollection services, IWalletConnectConfig config)
        {
            services.Replace(ServiceDescriptor.Singleton(typeof(IWalletConnectConfig), config));
            return services;
        }

        /// <summary>
        /// Use this to enable WalletConnect functionality for this instance of Web3.
        /// </summary>
        public static IWeb3ServiceCollection UseWalletConnect(this IWeb3ServiceCollection services)
        {
            services.AssertServiceNotBound<IWalletConnectProvider>();

            services.AddSingleton<IDataStorage, DataStorage>();
            services.AddSingleton<IWalletRegistry, ILifecycleParticipant, WalletRegistry>();
            services.AddSingleton<RedirectionHandler>();
            services.AddSingleton<IWalletConnectProvider, ILoginHelper, ILifecycleParticipant, WalletConnectProvider>();

            return services;
        }

        /// <summary>
        /// Use this to enable WalletConnect functionality for this instance of Web3.
        /// </summary>
        public static IWeb3ServiceCollection UseWalletConnect(this IWeb3ServiceCollection services, IWalletConnectConfig config)
        {
            services.AssertServiceNotBound<IWalletConnectProvider>();

            services.ConfigureWalletConnect(config);
            services.UseWalletConnect();

            return services;
        }

        /// <summary>
        /// Use this to set <see cref="WalletConnectSigner"/> as the <see cref="ISigner"/> for this instance of Web3.
        /// </summary>
        public static IWeb3ServiceCollection UseWalletConnectSigner(this IWeb3ServiceCollection services)
        {
            EnsureProviderBoundFirst(services);
            services.AssertServiceNotBound<ISigner>();

            services.AddSingleton<ISigner, ILifecycleParticipant, ILogoutHandler, WalletConnectSigner>();

            return services;
        }

        /// <summary>
        /// Use this to set <see cref="WalletConnectTransactionExecutor"/> as the <see cref="ITransactionExecutor"/>
        /// for this instance of Web3.
        /// </summary>
        public static IWeb3ServiceCollection UseWalletConnectTransactionExecutor(this IWeb3ServiceCollection services)
        {
            EnsureProviderBoundFirst(services);
            services.AssertServiceNotBound<ITransactionExecutor>();

            services.AddSingleton<ITransactionExecutor, WalletConnectTransactionExecutor>();

            return services;
        }

        /// <summary>
        /// Access additional services related to WalletConnect.
        /// </summary>
        public static WalletConnectSubCategory WalletConnect(this Web3.Web3 web3)
        {
            return new WalletConnectSubCategory(web3);
        }

        /// <summary>
        /// Access the <see cref="IWalletRegistry"/> service.
        /// </summary>
        public static IWalletRegistry WalletRegistry(this WalletConnectSubCategory walletConnect)
        {
            return ((IWeb3SubCategory)walletConnect).Web3.ServiceProvider.GetRequiredService<IWalletRegistry>();
        }

        /// <summary>
        /// Access the <see cref="RedirectionHandler"/> service.
        /// </summary>
        public static RedirectionHandler RedirectionHandler(this WalletConnectSubCategory walletConnect)
        {
            return ((IWeb3SubCategory)walletConnect).Web3.ServiceProvider.GetRequiredService<RedirectionHandler>();
        }

        /// <summary>
        /// Access the <see cref="ILoginHelper"/> service.
        /// </summary>
        public static ILoginHelper LoginHelper(this WalletConnectSubCategory walletConnect)
        {
            return ((IWeb3SubCategory)walletConnect).Web3.ServiceProvider.GetRequiredService<ILoginHelper>();
        }

        /// <summary>
        /// We need this to be sure <see cref="WalletConnectProvider"/>
        /// initializes first as a <see cref="ILifecycleParticipant"/>.
        /// </summary>
        private static void EnsureProviderBoundFirst(IWeb3ServiceCollection services)
        {
            if (services.Any(descriptor => descriptor.ServiceType == typeof(WalletConnectProvider)))
            {
                return;
            }

            throw new Web3BuildException($"You should bind {nameof(WalletConnectProvider)} first.");
        }
    }
}