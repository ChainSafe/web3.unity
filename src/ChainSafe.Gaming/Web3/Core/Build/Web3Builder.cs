using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.Contracts.BuiltIn;
using ChainSafe.Gaming.LocalStorage;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Core.Chains;
using ChainSafe.Gaming.Web3.Core.Logout;
using ChainSafe.Gaming.Web3.Environment;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.Web3.Build
{
    /// <summary>
    /// Builder object for <see cref="Web3"/>. Used to configure the set of services and other settings.
    /// </summary>
    public class Web3Builder
    {
        private readonly Web3ServiceCollection serviceCollection;

        private Web3Builder()
        {
            serviceCollection = new Web3ServiceCollection();

            // Bind default services
            serviceCollection
                .AddSingleton<IContractBuilder, ILifecycleParticipant, ContractBuilder>()
                .AddSingleton<ILocalStorage, DataStorage>()
                .AddSingleton<ChainRegistryProvider>()
                .AddSingleton<ILogoutManager, LogoutManager>()
                .AddSingleton<Erc20Service>()
                .AddSingleton<Erc721Service>()
                .AddSingleton<Erc1155Service>()
                .AddSingleton<LifecycleManager>()
                .AddChainManager();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Web3Builder"/> class.
        /// </summary>
        /// <param name="chainConfigSet">Chain config set to use with the resulting Web3 instance.</param>
        public Web3Builder(IChainConfigSet chainConfigSet)
            : this()
        {
            if (chainConfigSet == null)
            {
                throw new ArgumentNullException(nameof(chainConfigSet), $"{nameof(IChainConfigSet)} is required for Web3 to work.");
            }

            serviceCollection.AddSingleton(chainConfigSet);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Web3Builder"/> class.
        /// </summary>
        /// <param name="chainConfigs">Chain configs to use with the resulting Web3 instance.</param>
        public Web3Builder(params IChainConfig[] chainConfigs)
            : this(new ChainConfigSet(chainConfigs))
        {
        }

        /// <summary>
        /// Delegate used to configure services for <see cref="Web3"/>.
        /// </summary>
        public delegate void ConfigureServicesDelegate(IWeb3ServiceCollection services);

        /// <summary>
        /// Configure services for <see cref="Web3"/>.
        /// </summary>
        /// <param name="configureMethod">Delegate used to configure services for <see cref="Web3"/>.</param>
        /// <returns>Builder object to enable fluent syntax.</returns>
        public Web3Builder Configure(ConfigureServicesDelegate configureMethod)
        {
            if (configureMethod is null)
            {
                return this;
            }

            configureMethod(serviceCollection);
            return this;
        }

        /// <summary>
        /// Build <see cref="Web3"/> object using the settings provided by this Web3Builder object.
        /// </summary>
        /// <returns><see cref="Web3"/> object.</returns>
        public async ValueTask<Web3> LaunchAsync()
        {
            var serviceProvider = serviceCollection.BuildServiceProvider();
            AssertWeb3EnvironmentBound(serviceProvider);

            var web3 = new Web3(serviceProvider);
            await web3.InitializeAsync();

            return web3;
        }

        private static void AssertWeb3EnvironmentBound(IServiceProvider serviceProvider)
        {
            try
            {
                serviceProvider.GetRequiredService<Web3Environment>();
            }
            catch (InvalidOperationException e)
            {
                var message = $"{nameof(Web3Environment)} is required for Web3 to work." +
                              "Don't forget to bind it when configuring your Web3 instance.";
                throw new Web3BuildException(message, e);
            }
        }
    }
}
