using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.Marketplace.Extensions;
using ChainSafe.Gaming.MultiCall;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Unity;
using ChainSafe.GamingSdk.Gelato;
using Scripts.EVM.Token;
using UnityEngine;
using UnityEngine.SceneManagement;
using Environment = ChainSafe.Gaming.SygmaClient.Types.Environment;

namespace ChainSafe.Gaming.UnityPackage.Common
{
    /// <summary>
    /// Builds <see cref="Web3"/> Instance and Login using a Wallet or a provider.
    /// </summary>
    public interface ILoginProvider
    {
        /// <summary>
        /// Gelato API key from Gelato's Web Dashboard.
        /// </summary>
        public string GelatoApiKey { get; }

        /// <summary>
        /// All service providers used for configuring <see cref="Web3"/> instance services.
        /// </summary>
        public IWeb3BuilderServiceAdapter[] Web3BuilderServiceAdapters { get; }

        /// <summary>
        /// All Web3 initialized handlers called when Web3 instance is initialized.
        /// </summary>
        public IWeb3InitializedHandler[] Web3InitializedHandlers { get; }

        /// <summary>
        /// Login by Building a <see cref="Web3"/> Instance.
        /// </summary>
        public async Task Login()
        {
            Web3.Web3 web3;

            Web3Builder web3Builder = new Web3Builder(ProjectConfigUtilities.Load()).Configure(ConfigureCommonServices);

            web3Builder = ConfigureWeb3Services(web3Builder);

            web3 = await web3Builder.LaunchAsync();

            Web3Accessor.Set(web3);

            OnWeb3Initialized();
        }

        private void OnWeb3Initialized()
        {
            foreach (var web3InitializedHandler in Web3InitializedHandlers)
            {
                web3InitializedHandler.OnWeb3Initialized();
            }
        }

        /// <summary>
        /// Configure services to inject based on the type of Login/Provider you want to use.
        /// </summary>
        /// <param name="web3Builder">Builder for services to use.</param>
        /// <returns>Builder with new services added/injected.</returns>
        private Web3Builder ConfigureWeb3Services(Web3Builder web3Builder)
        {
            foreach (var adapter in Web3BuilderServiceAdapters)
            {
                web3Builder = adapter.ConfigureServices(web3Builder);
            }

            return web3Builder;
        }

        private void ConfigureCommonServices(IWeb3ServiceCollection services)
        {
            services
                .UseUnityEnvironment()
                .UseGelato(GelatoApiKey)
                .UseMultiCall()
                .UseRpcProvider()
                .UseMarketplace();

            /* As many contracts as needed may be registered here.
             * It is better to register all contracts the application
             * will be interacting with at configuration time if they
             * are known in advance. We're just registering shiba
             * here to show how it's done. You can look at the
             * `Scripts/Prefabs/Wallet/RegisteredContract` script
             * to see how it's used later on.
             */
            services.ConfigureRegisteredContracts(contracts =>
                contracts.RegisterContract("CsTestErc20", ABI.Erc20, ChainSafeContracts.Erc20));

        }
    }
}