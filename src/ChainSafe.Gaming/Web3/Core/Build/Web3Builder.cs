using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Web3.Core.Evm.EventPoller;
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
                .UseEventPoller()
                .AddSingleton<ChainRegistryProvider>()
                .AddSingleton<IContractBuilder, ContractBuilder>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Web3Builder"/> class.
        /// </summary>
        /// <param name="projectConfig">Project config to use with the resulting Web3 instance.</param>
        /// <param name="chainConfig">Chain config to use with the resulting Web3 instance.</param>
        /// <exception cref="ArgumentException">One of the arguments is null.</exception>
        public Web3Builder(IProjectConfig projectConfig, IChainConfig chainConfig)
            : this()
        {
            if (projectConfig == null)
            {
                throw new ArgumentNullException(nameof(projectConfig), $"{nameof(IProjectConfig)} is required for Web3 to work.");
            }

            if (chainConfig == null)
            {
                throw new ArgumentNullException(nameof(chainConfig), $"{nameof(IChainConfig)} is required for Web3 to work.");
            }

            serviceCollection.AddSingleton(projectConfig);
            serviceCollection.AddSingleton(chainConfig);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Web3Builder"/> class.
        /// </summary>
        /// <param name="projectConfig">Complete project config to use with the resulting Web3 instance.</param>
        /// <exception cref="ArgumentException">"projectConfig" is null.</exception>
        public Web3Builder(ICompleteProjectConfig projectConfig)
            : this(projectConfig, projectConfig)
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
        public async ValueTask<Web3> BuildAsync()
        {
            var serviceProvider = serviceCollection.BuildServiceProvider();
            AssertWeb3EnvironmentBound(serviceProvider);

            var web3 = new Web3(serviceProvider);
            await web3.InitializeAsync();

            return web3;
        }

        private static void AssertWeb3EnvironmentBound(IServiceProvider serviceProvider)
        {
            // TODO: test what happens when of environment components is not bound
            try
            {
                serviceProvider.GetRequiredService<Web3Environment>();
            }
            catch (InvalidOperationException e)
            {
                var message = $"{nameof(Web3Environment)} is required for Web3 to work." +
                              "Don't forget to bind it when building Web3.";
                throw new Web3BuildException(message, e);
            }
        }
    }
}