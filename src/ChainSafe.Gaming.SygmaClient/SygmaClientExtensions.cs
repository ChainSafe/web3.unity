using ChainSafe.Gaming.SygmaClient.Types;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.SygmaClient
{
    public static class SygmaClientExtensions
    {
        public static IWeb3ServiceCollection UseSygmaClient(this IWeb3ServiceCollection services)
        {
            services.AddSingleton<ISygmaClient, ILifecycleParticipant, SygmaClient>();
            return services;
        }

        public static ISygmaClient SygmaClient(this Web3.Web3 web3)
        {
            return web3.ServiceProvider.GetService<ISygmaClient>();
        }
    }
}