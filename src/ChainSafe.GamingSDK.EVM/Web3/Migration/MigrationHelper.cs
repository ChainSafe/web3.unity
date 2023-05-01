using ChainSafe.GamingWeb3.Build;
using ChainSafe.GamingWeb3.Evm;
using ChainSafe.GamingWeb3.Evm.Providers;

namespace ChainSafe.GamingWeb3.Migration
{
  public static class MigrationHelper
  {
    public static JsonRpcProvider BuildJsonRpcProvider(string url, Network network)
    {
      var web3 = new Web3Builder()
        .Configure(services =>
        {
          // todo
        }).Build();

      return (JsonRpcProvider)web3.Provider;
    }
  }
}