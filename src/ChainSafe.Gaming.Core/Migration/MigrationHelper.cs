using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Build;
using ChainSafe.Gaming.Evm;
using ChainSafe.Gaming.Evm.JsonRpcProvider;

namespace ChainSafe.Gaming.Migration
{
    internal static class MigrationHelper
    {
        public static async ValueTask<JsonRpcProvider> NewJsonRpcProviderAsync(string url, Network network, Action<IWeb3ServiceCollection> bindEnvironment)
        {
            var web3 = await new Web3Builder()
              .Configure(services =>
              {
                  bindEnvironment(services);
                  services.UseJsonRpcProvider(new JsonRpcProviderConfig { RpcNodeUrl = url, Network = network });
              }).BuildAsync();

            return (JsonRpcProvider)web3.RpcProvider;
        }
    }
}