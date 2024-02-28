using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using UnityEngine;

namespace ChainSafe.Gaming.Exchangers.Ramp
{
    public static class RampExchangerExtensions
    {
        public static IRampExchanger RampExchanger(this Web3.Web3 web3)
        {
            return web3.ServiceProvider.GetRequiredService<IRampExchanger>();
        }

        public static IWeb3ServiceCollection UseRampExchanger(this IWeb3ServiceCollection services,
            IRampExchangerConfig config)
        {
            services.AssertServiceNotBound<IRampExchanger>();
            services.ConfigureRampExchanger(config);
            services.UseRampExchanger();
            return services;
        }

        public static IWeb3ServiceCollection ConfigureRampExchanger(this IWeb3ServiceCollection services,
            IRampExchangerConfig config)
        {
            services.Replace(ServiceDescriptor.Singleton(config));
            return services;
        }

        public static IWeb3ServiceCollection UseRampExchanger(this IWeb3ServiceCollection services)
        {
            services.AssertServiceNotBound<IRampExchanger>();
            services.AddSingleton<IRampExchanger, ILifecycleParticipant, RampExchangerUniversal>();
            return services;
        }
    }
}