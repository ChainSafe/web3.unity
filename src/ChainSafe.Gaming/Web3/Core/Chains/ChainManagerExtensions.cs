using ChainSafe.Gaming.Web3.Build;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.Web3.Core.Chains
{
    public static class ChainManagerExtensions
    {
        internal static IServiceCollection AddChainManager(this IServiceCollection services)
        {
            return services
                .AddSingleton<IChainManager, ChainManager>()
                .AddSingleton<IChainConfig, ChainManagerChainConfig>()
                .AddSingleton<SwitchChainHandlersProvider>();
        }
    }
}