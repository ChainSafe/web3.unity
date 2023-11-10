using LootBoxes.Chainlink.Scene;
using Chainsafe.Gaming.Chainlink;
using ChainSafe.Gaming.Debugging;
using ChainSafe.Gaming.Evm.JsonRpc;
using ChainSafe.Gaming.Lootboxes.Chainlink;
using ChainSafe.Gaming.Wallets;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Unity;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;


namespace LootBoxes.Chainlink
{
    /// <summary>
    /// Initializes Web3 using local RPC node and node's user
    /// </summary>
    public class ChainlinkLootboxSampleLauncher : MonoBehaviour
    {
        // Lootbox alternate chain contracts
        // 0xF899Bc1f149030c3912f1Cb473A7b2db0ed9cE5f - Sepolia Testnet LootboxFactory
        // 0xfa97aCce8E4929e6d13a7c418BfbA0311e9D3Bfd - Mumbai Testnet LootboxFactory
        // 0xfa97aCce8E4929e6d13a7c418BfbA0311e9D3Bfd - Binance Testnet LootboxFactory
        // 0xfa97aCce8E4929e6d13a7c418BfbA0311e9D3Bfd - Fuji Testnet LootboxFactory
        // 0xfa97aCce8E4929e6d13a7c418BfbA0311e9D3Bfd - Fantom Testnet LootboxFactory
        // 0xfa97aCce8E4929e6d13a7c418BfbA0311e9D3Bfd - Arbitrum Goerli Testnet LootboxFactory
        // 0x1993e2dD323B5dcBd8b52dB7d370bC36D280424B - Anvil
        public LootBoxScene lootBoxScene;
        public string ContractAbi;

        private Web3 web3;

        private class Web3Config : ICompleteProjectConfig
        {
            public string ProjectId => string.Empty;
            public string ChainId => "11155111";
            public string Chain => "Ethereum";
            public string Network => "Sepolia";
            public string Rpc => $"https://sepolia.infura.io/v3/287318045c6e455ab34b81d6bcd7a65f";
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
                    //Debug
                    //services.Debug().UseJsonRpcWallet(new JsonRpcWalletConfig { AccountIndex = 2 });
                    services.UseChainlinkLootboxService(new LootboxServiceConfig
                    {
                        ContractAddress = "0xF899Bc1f149030c3912f1Cb473A7b2db0ed9cE5f",
                        ContractAbi = ContractAbi
                    });
                    services.AddSingleton<Erc1155MetaDataReader>();
                })
                .LaunchAsync();

            lootBoxScene.Configure(
                web3.Chainlink().Lootboxes(),
                web3.ContractBuilder,
                web3.ServiceProvider.GetRequiredService<Erc1155MetaDataReader>());

            lootBoxScene.Launch();
        }
    }
}