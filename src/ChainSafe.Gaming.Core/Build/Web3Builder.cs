﻿using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Configuration;
using ChainSafe.Gaming.Environment;
using ChainSafe.Gaming.Evm;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.Build
{
    /// <summary>
    /// Builder object for Web3. Used to configure set of services.
    /// </summary>
    public class Web3Builder
    {
        private readonly Web3ServiceCollection serviceCollection;

        public Web3Builder()
        {
            serviceCollection = new Web3ServiceCollection();

            // Bind default services
            serviceCollection.AddSingleton<ChainProvider>();
        }

        // todo inline parameterless constructor into this one (therefore remove that overload)
        public Web3Builder(IProjectConfig projectConfig, IChainConfig chainConfig)
            : this()
        {
            serviceCollection.AddSingleton(projectConfig);
            serviceCollection.AddSingleton(chainConfig);
        }

        public Web3Builder(ICompleteProjectConfig projectConfig)
            : this(projectConfig, projectConfig)
        {
        }

        public delegate void ConfigureServicesDelegate(IWeb3ServiceCollection services);

        public Web3Builder Configure(ConfigureServicesDelegate configureMethod)
        {
            configureMethod(serviceCollection);
            return this;
        }

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
            // todo test what happens when of environment components is not bound
            try
            {
                serviceProvider.GetRequiredService<Web3Environment>();
            }
            catch (InvalidOperationException e)
            {
                var message = $"{nameof(Web3Environment)} is required for Web3 to work." +
                              "Don't forget to bind it when building Web3.";
                throw new Web3Exception(message, e);
            }
        }
    }
}