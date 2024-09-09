using System.Threading.Tasks;
using ChainSafe.Gaming.Connection;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.Marketplace.Extensions;
using ChainSafe.Gaming.MultiCall;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Unity;
using ChainSafe.GamingSdk.Gelato;
using Microsoft.Extensions.DependencyInjection;
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
        public IWeb3BuilderServiceAdapter[] Web3BuilderServiceAdapters { get; }

        /// <summary>
        /// Login by Building a <see cref="Web3"/> Instance.
        /// </summary>
        public async Task<CWeb3> Connect(ConnectionProvider provider)
        {
            Web3Builder web3Builder = new Web3Builder(ProjectConfigUtilities.Load())
                .Configure(ConfigureCommonServices)
                .ConfigureServices(Web3BuilderServiceAdapters)
                .ConfigureServices(provider);

            var web3 = await web3Builder.LaunchAsync();

            await OnWeb3Initialized(web3);

            return web3;
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
            // TODO: most of these can/should be service adapters
            services
                .UseUnityEnvironment()
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