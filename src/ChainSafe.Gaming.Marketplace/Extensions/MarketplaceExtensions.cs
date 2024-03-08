using ChainSafe.Gaming.Web3.Build;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.Marketplace.Extensions
{
    public static class MarketplaceExtensions
    {
        public static IWeb3ServiceCollection ConfigureMarketplace(
            this IWeb3ServiceCollection services,
            IMarketplaceConfig config)
        {
            services.Replace(ServiceDescriptor.Singleton(typeof(IMarketplaceConfig), config));
            return services;
        }

        public static IWeb3ServiceCollection UseMarketplace(this IWeb3ServiceCollection services)
        {
            services.AssertServiceNotBound<MarketplaceClient>();
            services.AddSingleton<MarketplaceClient>();
            return services;
        }

        public static IWeb3ServiceCollection UseMarketplace(
            this IWeb3ServiceCollection services,
            IMarketplaceConfig config)
        {
            services.AssertServiceNotBound<MarketplaceClient>();
            services.ConfigureMarketplace(config);
            services.UseMarketplace(config);
            return services;
        }

        public static MarketplaceClient Marketplace(this Web3.Web3 web3)
        {
            return web3.ServiceProvider.GetRequiredService<MarketplaceClient>();
        }
    }
}