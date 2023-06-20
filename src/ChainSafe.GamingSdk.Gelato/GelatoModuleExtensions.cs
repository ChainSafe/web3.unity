using ChainSafe.GamingSDK.EVM.Web3.Core;
using ChainSafe.GamingSDK.EVM.Web3.Core.Evm;
using ChainSafe.GamingSdk.Gelato.Types;
using ChainSafe.GamingWeb3.Build;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.GamingSdk.Gelato
{
    public static class GelatoModuleExtensions
    {
        private static readonly GelatoConfig DefaultConfig = new()
        {
            Url = "https://api.gelato.digital",
            GelatoRelayErc2771Address = "0xb539068872230f20456CF38EC52EF2f91AF4AE49",
            GelatoRelay1BalanceErc2771Address = "0xd8253782c45a12053594b9deB72d8e8aB2Fca54c",
            GelatoRelayErc2771ZkSyncAddress = "0x22DCC39b2AC376862183dd35A1664798dafC7Da6",
            GelatoRelay1BalanceErc2771ZkSyncAddress = "0x97015cD4C3d456997DD1C40e2a18c79108FCc412",
        };

        /// <summary>
        /// Binds Gelato module to Web3.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseGelatoModule(this IWeb3ServiceCollection collection, GelatoConfig configuration)
        {
            collection.UseGelatoModule();
            collection.ConfigureGelatoModule(configuration);
            return collection;
        }

        /// <summary>
        /// Binds Gelato module to Web3.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseGelatoModule(this IWeb3ServiceCollection collection)
        {
            // config
            collection.TryAddSingleton(DefaultConfig);

            // Gelato module
            collection.AddSingleton<IGelatoModule, ILifecycleParticipant, GelatoModule>();

            return collection;
        }

        /// <summary>
        /// Configures Gelato module.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection ConfigureGelatoModule(this IWeb3ServiceCollection collection, GelatoConfig configuration)
        {
            collection.Replace(ServiceDescriptor.Singleton(typeof(GelatoConfig), configuration));
            return collection;
        }
    }
}