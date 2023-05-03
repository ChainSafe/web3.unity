using System;
using ChainSafe.GamingWeb3.Build;
using ChainSafe.GamingWeb3.Evm.JsonRpc;
using ChainSafe.GamingWeb3.Evm.Providers;
using ChainSafe.GamingWeb3.Evm.Signers;

namespace ChainSafe.GamingWeb3.Evm.Migration
{
  public static class MigrationHelperBase
  {
    public static JsonRpcProvider NewJsonRpcProvider(string url, Network network, Action<IWeb3ServiceCollection> bindEnvironment)
    {
      var web3 = new Web3Builder()
        .Configure(services =>
        {
          bindEnvironment(services);
          services.UseJsonRpcProvider(new JsonRpcProviderConfiguration { RpcNodeUrl = url, Network = network });
        }).Build();

      return (JsonRpcProvider)web3.Provider;
    }

    public static JsonRpcWallet GetSigner(JsonRpcProvider provider, int index, Action<IWeb3ServiceCollection> bindEnvironment)
    {
      var web3 = new Web3Builder()
        .Configure(services =>
        {
          bindEnvironment(services);
          services.UseJsonRpcProvider(new JsonRpcProviderConfiguration
          {
            RpcNodeUrl = provider.RpcNodeUrl,
            Network = provider.Network
          });
          services.UseJsonRpcWallet(new JsonRpcWalletConfiguration { AccountIndex = index });
        }).Build();

      return (JsonRpcWallet) web3.Wallet;
    }
  }
}