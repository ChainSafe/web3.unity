using System.Linq;
using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Evm;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.WalletConnect
{
    public static class WalletConnectExtensions
    {
        public static IWeb3ServiceCollection ConfigureWalletConnectNew(this IWeb3ServiceCollection services, IWalletConnectConfigNew config)
        {
            services.Replace(ServiceDescriptor.Singleton(typeof(IWalletConnectConfigNew), config));
            return services;
        }

        public static IWeb3ServiceCollection UseWalletConnectNew(this IWeb3ServiceCollection services)
        {
            services.AssertServiceNotBound<IWalletConnectProviderNew>();

            services.AddSingleton<IDataStorage, DataStorage>();
            services.AddSingleton<IWalletRegistry, ILifecycleParticipant, WalletRegistry>();
            services.AddSingleton<RedirectionHandler>();
            services.AddSingleton<IWalletConnectProviderNew, ILifecycleParticipant, WalletConnectProviderNew>();

            return services;
        }

        public static IWeb3ServiceCollection UseWalletConnectNew(this IWeb3ServiceCollection services, IWalletConnectConfigNew config)
        {
            services.AssertServiceNotBound<IWalletConnectProviderNew>();

            services.ConfigureWalletConnectNew(config);
            services.UseWalletConnectNew();

            return services;
        }

        public static IWeb3ServiceCollection UseWalletConnectSignerNew(this IWeb3ServiceCollection services)
        {
            EnsureProviderBoundFirst(services);
            services.AssertServiceNotBound<ISigner>();

            services.AddSingleton<ISigner, ILifecycleParticipant, WalletConnectSignerNew>();

            return services;
        }

        public static IWeb3ServiceCollection UseWalletConnectTransactionExecutorNew(this IWeb3ServiceCollection services)
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

        /// <summary>
        /// We need this to be sure <see cref="WalletConnectProviderNew"/>
        /// initializes first as a <see cref="ILifecycleParticipant"/>.
        /// </summary>
        private static void EnsureProviderBoundFirst(IWeb3ServiceCollection services)
        {
            if (services.Any(descriptor => descriptor.ServiceType == typeof(WalletConnectProviderNew)))
            {
                return;
            }

            throw new Web3BuildException($"You should bind {nameof(WalletConnectProviderNew)} first.");
        }
    }
}