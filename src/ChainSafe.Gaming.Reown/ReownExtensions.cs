using ChainSafe.Gaming.Reown.Wallets;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.Reown
{
    public static class ReownExtensions
    {
        /// <summary>
        /// Use this to configure Reown for this instance of Web3.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection ConfigureReown(this IWeb3ServiceCollection services, IReownConfig config)
        {
            services.Replace(ServiceDescriptor.Singleton(typeof(IReownConfig), config));

            return services;
        }

        /// <summary>
        /// Use this to enable Reown functionality for this instance of Web3.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseReown(this IWeb3ServiceCollection services, IReownConfig config)
        {
            services.AssertServiceNotBound<IWalletProvider>();

            services.ConfigureReown(config);

            services.AddSingleton<IWalletRegistry, ILifecycleParticipant, ReownWalletRegistry>();
            services.AddSingleton<RedirectionHandler>();
            services.AddSingleton<ReownHttpClient>();
            services.AddSingleton<IConnectionHelper, ILifecycleParticipant, ReownProvider>();
            services.UseWalletProvider<ReownProvider>(config);

            return services;
        }

        /// <summary>
        /// Access additional services related to Reown.
        /// </summary>
        /// <returns>The Reown subcategory of services.</returns>
        public static ReownSubCategory Reown(this Web3.Web3 web3)
        {
            return new ReownSubCategory(web3);
        }

        /// <summary>
        /// Access the <see cref="IWalletRegistry"/> service.
        /// </summary>
        /// <returns>The <see cref="IWalletRegistry"/> service.</returns>
        public static IWalletRegistry WalletRegistry(this ReownSubCategory reown)
        {
            return ((IWeb3SubCategory)reown).Web3.ServiceProvider.GetRequiredService<IWalletRegistry>();
        }

        /// <summary>
        /// Access the <see cref="RedirectionHandler"/> service.
        /// </summary>
        /// <returns>The <see cref="RedirectionHandler"/> service.</returns>
        public static RedirectionHandler RedirectionHandler(this ReownSubCategory reown)
        {
            return ((IWeb3SubCategory)reown).Web3.ServiceProvider.GetRequiredService<RedirectionHandler>();
        }

        /// <summary>
        /// Access the <see cref="IConnectionHelper"/> service.
        /// </summary>
        /// <returns>The <see cref="IConnectionHelper"/> service.</returns>
        public static IConnectionHelper ConnectionHelper(this ReownSubCategory reown)
        {
            return ((IWeb3SubCategory)reown).Web3.ServiceProvider.GetRequiredService<IConnectionHelper>();
        }
    }
}