using System;
using ChainSafe.GamingWeb3.Build;
using Web3Unity.Scripts.Library.Ethers.JsonRpc;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;

namespace Web3Unity.Scripts.Library.Ethers.Migration
{
    internal static class MigrationHelper
    {
        public static JsonRpcProvider NewJsonRpcProvider(string url, Network.Network network, Action<IWeb3ServiceCollection> bindEnvironment)
        {
            var web3 = new Web3Builder()
              .Configure(services =>
              {
                  bindEnvironment(services);
                  services.UseJsonRpcProvider(new JsonRpcProviderConfig { RpcNodeUrl = url, Network = network });
              }).Build();

            return (JsonRpcProvider)web3.Provider;
        }
    }
}