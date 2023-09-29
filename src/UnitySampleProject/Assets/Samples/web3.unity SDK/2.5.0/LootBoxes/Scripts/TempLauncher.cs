using Chainsafe.Gaming.Chainlink;
using ChainSafe.Gaming.Chainlink.Lootboxes;
using ChainSafe.Gaming.Debugging;
using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.Wallets;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Unity;
using LootBoxes.Scene;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;


namespace LootBoxes
{
    /// <summary>
    /// Initializes Web3 using local RPC node and node's user
    /// </summary>
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
            public string Ipc { get; }
            public string Ws { get; }
        }

        private async void Awake()
        {
            web3 = await new Web3Builder(new Web3Config())
                .Configure(services =>
                {
                    services.UseUnityEnvironment();
                    services.UseRpcProvider();
                    services.Debug().UseJsonRpcWallet(new JsonRpcWalletConfig { AccountIndex = 2 });
                    services.UseChainlinkLootboxService(new LootboxServiceConfig
                    {
                        ContractAddress = "0x46E334e90454aDDF311Cd75D4Ae19e2fA06285Ff",
                        ContractAbi = ContractAbi
                    });
                    services.AddSingleton<Erc1155MetaDataReader>();
                })
                .BuildAsync();

            lootBoxScene.Configure(
                web3.Chainlink().Lootboxes(),
                web3.ContractBuilder,
                web3.ServiceProvider.GetRequiredService<Erc1155MetaDataReader>());

            lootBoxScene.Launch();
        }
    }
}