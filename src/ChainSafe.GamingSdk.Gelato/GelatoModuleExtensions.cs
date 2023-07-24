﻿using ChainSafe.GamingSDK.EVM.Web3.Core;
using ChainSafe.GamingSdk.Gelato.Types;
using ChainSafe.GamingWeb3;
using ChainSafe.GamingWeb3.Build;
using ChainSafe.GamingWeb3.Environment;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Web3Unity.Scripts.Library.Ethers.Contracts;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;

namespace ChainSafe.GamingSdk.Gelato
{
    public static class GelatoModuleExtensions
    {
        public static IGelato Gelato(this Web3 web3) => web3.ServiceProvider.GetRequiredService<IGelato>();

        public static GelatoConfig DefaultConfig() => new()
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
        public static IWeb3ServiceCollection UseGelato(this IWeb3ServiceCollection collection, GelatoConfig configuration)
        {
            collection.UseGelato();
            collection.ConfigureGelatoModule(configuration);
            return collection;
        }

        /// <summary>
        /// Binds Gelato module to Web3.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseGelato(this IWeb3ServiceCollection collection, string sponsorApiKey)
        {
            // config
            var config = DefaultConfig();
            config.SponsorApiKey = sponsorApiKey;
            collection.TryAddSingleton(config);

            // Gelato module
            collection.UseGelato();

            return collection;
        }

        /// <summary>
        /// Binds Gelato module to Web3.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseGelato(this IWeb3ServiceCollection collection)
        {
            // config
            var config = DefaultConfig();
            collection.TryAddSingleton(config);

            // Gelato module
            collection.AddSingleton<IGelato, ILifecycleParticipant, Gelato>(
                sp => new Gelato(
                    sp.GetRequiredService<IHttpClient>(),
                    sp.GetRequiredService<IChainConfig>(),
                    sp.GetRequiredService<GelatoConfig>(),
                    sp.GetRequiredService<IRpcProvider>(),
                    sp.GetService<ISigner>(),
                    sp.GetRequiredService<IContractBuilder>()));

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