using System.Linq;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core.Nethereum;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.Mud
{
    public static class MudExtensions
    {
        public static IWeb3ServiceCollection UseMud(this IWeb3ServiceCollection services)
        {
            services.AddSingleton(typeof(MudFacade));
            services.AddSingleton<MudWorldFactory>();

            if (!services.IsNethereumAdaptersBound())
            {
                services.UseNethereumAdapters();
            }

            return services;
        }

        public static MudFacade Mud(this Web3.Web3 web3) => web3.ServiceProvider.GetRequiredService<MudFacade>();
    }
}