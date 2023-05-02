using System;
using ChainSafe.GamingWeb3.Environment;
using ChainSafe.GamingWeb3.Evm;
using ChainSafe.GamingWeb3.Evm.Signers;
using Microsoft.Extensions.DependencyInjection;

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
      var environment = AssertWeb3EnvironmentBound(serviceProvider);
      var provider = serviceProvider.GetService<IEvmProvider>();
      var signer = serviceProvider.GetService<IEvmSigner>();
      var wallet = serviceProvider.GetService<IEvmWallet>();

      var web3 = new Web3(
        serviceProvider,
        environment,
        provider,
        signer,
        wallet
        );

      return web3;
    }

    private static IWeb3Environment AssertWeb3EnvironmentBound(IServiceProvider serviceProvider)
    {
      IWeb3Environment env;
      try
      {
        env = serviceProvider.GetRequiredService<IWeb3Environment>();
      }
      catch (InvalidOperationException e)
      {
        throw new Web3Exception($"{nameof(IWeb3Environment)} is a required service for Web3 to work.", e);
      }

      return env;
    }
  }
}