using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Connection;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.UnityPackage.UI;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core.Operations;
using ChainSafe.Gaming.Web3.Unity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Scripts.EVM.Token;
using CWeb3 = ChainSafe.Gaming.Web3.Web3;

namespace ChainSafe.Gaming.UnityPackage.Connection
{
    /// <summary>
    /// Builds <see cref="Web3"/> Instance and Login using a Wallet or a provider.
    /// </summary>
    public interface IConnectionHandler
    {
        /// <summary>
        /// All service providers used for configuring <see cref="Web3"/> instance services.
        /// </summary>
        public HashSet<IServiceAdapter> Web3BuilderServiceAdapters { get; }

        /// <summary>
        /// Login by Building a <see cref="Web3"/> Instance.
        /// </summary>
        public async Task Connect(ConnectionProvider provider)
        {
            try
            {
                await LaunchWeb3(Web3BuilderServiceAdapters.Append(provider));
            }
            catch (Exception e)
            {
                provider.HandleException(e);
            }
        }

        private async Task LaunchWeb3(IEnumerable<IServiceAdapter> adapters)
        {
            var web3Builder = new Web3Builder(ProjectConfigUtilities.Load())
                .Configure(ConfigureCommonServices)
                .ConfigureServices(adapters)
                .Configure(ConfigureExtraServices);

            var web3 = await web3Builder.LaunchAsync();

            await OnWeb3Initialized(web3);
        }

        public async Task LaunchLightWeightWeb3()
        {
            await LaunchWeb3(Web3BuilderServiceAdapters.OfType<ILightWeightServiceAdapter>());
        }

        private async Task OnWeb3Initialized(CWeb3 web3)
        {
            var web3InitializedHandlers = web3.ServiceProvider.GetServices<IWeb3InitializedHandler>();

            foreach (var web3InitializedHandler in web3InitializedHandlers)
            {
                await web3InitializedHandler.OnWeb3Initialized(web3);
            }
        }

        private void ConfigureCommonServices(IWeb3ServiceCollection services)
        {
            services
                .UseUnityEnvironment()
                .UseRpcProvider();
            
            services.ConfigureRegisteredContracts(contracts =>
                contracts.RegisterContract("CsTestErc20", ABI.Erc20, ChainSafeContracts.Erc20));

        }

        private void ConfigureExtraServices(IWeb3ServiceCollection services)
        {
            services.TryAddSingleton<IOperationNotificationHandler, GuiOperationNotificationHandler>();
        }
    }
}