using ChainSafe.GamingWeb3.Build;
using Microsoft.Extensions.DependencyInjection;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;

namespace Web3Unity.Scripts.Library.Ethers.JsonRpc
{
  public static class JsonRpcExtensions
  {
    /// <summary>
    /// Binds JSON RPC implementation of EVM Provider to Web3
    /// </summary>
    public static void UseJsonRpcProvider(this IWeb3ServiceCollection serviceCollection, JsonRpcProviderConfiguration configuration)
    {
      serviceCollection.AddSingleton(configuration);
      serviceCollection.AddSingleton<IEvmProvider, JsonRpcProvider>();
    }
    
    /// <summary>
    /// Binds JSON RPC implementation of EVM Provider to Web3
    /// </summary>
    public static void UseJsonRpcProvider(this IWeb3ServiceCollection serviceCollection)
    {
      serviceCollection.AddSingleton<IEvmProvider, JsonRpcProvider>();
    }
    
    /// <summary>
    /// Configures JSON RPC implementation of EVM Provider
    /// </summary>
    public static void ConfigureJsonRpcProvider(this IWeb3ServiceCollection serviceCollection, JsonRpcProviderConfiguration configuration)
    {
      serviceCollection.AddSingleton(configuration);
    }
    
    /// <summary>
    /// Binds JSON RPC implementation of EVM Wallet to Web3
    /// </summary>
    public static void UseJsonRpcWallet(this IWeb3ServiceCollection serviceCollection, JsonRpcSignerConfiguration configuration)
    {
      serviceCollection.ConfigureJsonRpcWallet(configuration);
      serviceCollection.UseJsonRpcWallet();
    }
    
    /// <summary>
    /// Binds JSON RPC implementation of EVM Wallet to Web3
    /// </summary>
    public static void UseJsonRpcWallet(this IWeb3ServiceCollection serviceCollection)
    {
      serviceCollection.AddSingleton<IEvmWallet, JsonRpcSigner>();
      serviceCollection.AddSingleton(provider => (IEvmSigner) provider.GetService<IEvmWallet>());
    }
    
    /// <summary>
    /// Configures JSON RPC implementation of EVM Wallet
    /// </summary>
    public static void ConfigureJsonRpcWallet(this IWeb3ServiceCollection serviceCollection, JsonRpcSignerConfiguration configuration)
    {
      serviceCollection.AddSingleton(configuration);
    }
  }
}