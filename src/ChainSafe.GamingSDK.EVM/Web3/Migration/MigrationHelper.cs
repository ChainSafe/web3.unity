using ChainSafe.GamingWeb3.Build;
using ChainSafe.GamingWeb3.Evm;
using ChainSafe.GamingWeb3.Evm.JsonRpc;
using ChainSafe.GamingWeb3.Evm.Providers;
using ChainSafe.GamingWeb3.Evm.Signers;

namespace ChainSafe.GamingWeb3.Migration
{
  public static class MigrationHelper
  {
    public static JsonRpcProvider NewJsonRpcProvider(string url = "", Network network = null)
    {
      var web3 = new Web3Builder()
        .Configure(services =>
        {
          BindEnvironment(services);
          services.UseJsonRpcProvider(new JsonRpcProviderConfiguration
          {
            RpcNodeUrl = url,
            Network = network
          });
        }).Build();

      return (JsonRpcProvider)web3.Provider;
    }

    public static JsonRpcWallet GetSigner(this JsonRpcProvider provider, int index = 0)
    {
      var web3 = new Web3Builder()
        .Configure(services =>
        {
          BindEnvironment(services);
          services.UseJsonRpcProvider(new JsonRpcProviderConfiguration
          {
            RpcNodeUrl = provider.RpcNodeUrl,
            Network = provider.Network
          });
          services.UseJsonRpcWallet(new JsonRpcWalletConfiguration
          {
            AccountIndex = index
          });
        }).Build();

      return (JsonRpcWallet) web3.Wallet;
    }

    public static void BindEnvironment(IWeb3ServiceCollection services)
    {
      // todo bind environment for Unity or NetCore
    }
  }
}