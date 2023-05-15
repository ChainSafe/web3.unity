using System;
using ChainSafe.GamingWeb3.Environment;
using Web3Unity.Scripts.Library.Ethers;
using Web3Unity.Scripts.Library.Ethers.Signers;
using Microsoft.Extensions.DependencyInjection;
using Web3Unity.Scripts.Library.Ethers.Providers;

namespace ChainSafe.GamingWeb3.Build
{
    /// <summary>
    /// Builder object for Web3. Used to configure set of services.
    /// </summary>
    public class Web3Builder
    {
        public delegate void ConfigureServicesDelegate(IWeb3ServiceCollection services);

        private readonly Web3ServiceCollection _serviceCollection;

        public Web3Builder()
        {
            _serviceCollection = new Web3ServiceCollection();

            // Bind default services
            _serviceCollection.AddSingleton<ChainProvider>();
        }

        public Web3Builder Configure(ConfigureServicesDelegate configureMethod)
        {
            configureMethod(_serviceCollection);
            return this;
        }

        public Web3 Build()
        {
            var serviceProvider = _serviceCollection.BuildServiceProvider();
            AssertWeb3EnvironmentBound(serviceProvider);
            var provider = serviceProvider.GetService<IEvmProvider>();
            var signer = serviceProvider.GetService<IEvmSigner>();

            var web3 = new Web3(
              serviceProvider,
              provider,
              signer
              );

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