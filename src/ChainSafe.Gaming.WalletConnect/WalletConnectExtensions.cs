using System.Linq;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.WalletConnect.Storage;
using ChainSafe.Gaming.WalletConnect.Wallets;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.WalletConnect
{
    public static class WalletConnectExtensions
    {
        public static IWeb3ServiceCollection ConfigureWalletConnect(this IWeb3ServiceCollection services, IWalletConnectConfig config)
        {
            services.Replace(ServiceDescriptor.Singleton(typeof(IWalletConnectConfig), config));
            return services;
        }

        public static IWeb3ServiceCollection UseWalletConnect(this IWeb3ServiceCollection services)
        {
            services.AssertServiceNotBound<IWalletConnectProvider>();

            services.AddSingleton<IDataStorage, DataStorage>();
            services.AddSingleton<IWalletRegistry, ILifecycleParticipant, WalletRegistry>();
            services.AddSingleton<RedirectionHandler>();
            services.AddSingleton<IWalletConnectProvider, ILoginHelper, ILifecycleParticipant, WalletConnectProvider>();

            return services;
        }

        public static IWeb3ServiceCollection UseWalletConnect(this IWeb3ServiceCollection services, IWalletConnectConfig config)
        {
            services.AssertServiceNotBound<IWalletConnectProvider>();

            services.ConfigureWalletConnect(config);
            services.UseWalletConnect();

            return services;
        }

        public static IWeb3ServiceCollection UseWalletConnectSigner(this IWeb3ServiceCollection services)
        {
            EnsureProviderBoundFirst(services);
            services.AssertServiceNotBound<ISigner>();

            services.AddSingleton<ISigner, ILifecycleParticipant, WalletConnectSigner>();

            return services;
        }

        public static IWeb3ServiceCollection UseWalletConnectTransactionExecutor(this IWeb3ServiceCollection services)
        {
            EnsureProviderBoundFirst(services);
            services.AssertServiceNotBound<ITransactionExecutor>();

            services.AddSingleton<ITransactionExecutor, WalletConnectTransactionExecutorNew>();

            return services;
        }

        public static WalletConnectSubCategory WalletConnect(this Web3.Web3 web3)
        {
            return new WalletConnectSubCategory(web3);
        }

        public static IWalletRegistry WalletRegistry(this WalletConnectSubCategory walletConnect)
        {
            return ((IWeb3SubCategory)walletConnect).Web3.ServiceProvider.GetRequiredService<IWalletRegistry>();
        }

        public static RedirectionHandler RedirectionHandler(this WalletConnectSubCategory walletConnect)
        {
            return ((IWeb3SubCategory)walletConnect).Web3.ServiceProvider.GetRequiredService<RedirectionHandler>();
        }

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