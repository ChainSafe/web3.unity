using Chainsafe.Gaming.Chainlink;
using ChainSafe.Gaming.Chainlink.Lootboxes;
using ChainSafe.Gaming.Debugging;
using ChainSafe.GamingWeb3;
using ChainSafe.GamingWeb3.Build;
using ChainSafe.GamingWeb3.Unity;
using LootBoxes.Scene;
using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.JsonRpc;

namespace LootBoxes
{
    public class TempLauncher : MonoBehaviour
    {
        public LootBoxScene lootBoxScene;
        public string ContractAbi;
        
        private Web3 web3;

        private class Web3Config : ICompleteProjectConfig
        {
            public string ProjectId => string.Empty;
            public string ChainId => "31337";
            public string Chain => "Anvil";
            public string Network => "GoChain Testnet";
            public string Rpc => $"http://127.0.0.1:8545";
        }

        private async void Awake()
        {
            web3 = await new Web3Builder(new Web3Config())
                .Configure(services =>
                {
                    services.UseUnityEnvironment();
                    services.UseJsonRpcProvider();
                    services.Debug().UseJsonRpcWallet(new JsonRpcWalletConfig { AccountIndex = 2, });
                    services.UseChainlinkLootboxService(new LootboxServiceConfig
                    {
                        ContractAddress = "0x46E334e90454aDDF311Cd75D4Ae19e2fA06285Ff",
                        ContractAbi = ContractAbi
                    });
                })
                .BuildAsync();
            
            lootBoxScene.Configure(web3.Chainlink().Lootboxes());
            lootBoxScene.Launch();
        }
    }
}