using System.Collections.Generic;
using System.IO;
using ChainSafe.Gaming.Web3;
using UnityEngine;

namespace ChainSafe.Gaming.UnityPackage
{
    public static class ProjectConfigUtilities
    {
        private const string AssetName = "Web3Config";

        public static Web3ConfigAsset Load()
        {
            if (Web3Unity.TestMode)
            {
                bool isWindowsEditor = Application.platform == RuntimePlatform.WindowsEditor;
                
                return Create("3dc3e125-71c4-4511-a367-e981a6a94371",
                    "11155111",
                    "Anvil", "Sepolia", "Seth", isWindowsEditor ? "http://127.0.0.1:8545" : "http://172.17.0.1:8545",
                    "https://sepolia.etherscan.io/", false, isWindowsEditor ? "ws://127.0.0.1:8545" : "ws://172.17.0.1:8545");
            }
            
            var projectConfig = Resources.Load<Web3ConfigAsset>(AssetName);
            return projectConfig ? projectConfig : null;
        }

        public static Web3ConfigAsset Create(string projectId, string chainId, string chain, string network,
            string symbol, string rpc, string blockExplorerUrl, bool enableAnalytics, string ws = "")
        {
            var projectConfig = ScriptableObject.CreateInstance<Web3ConfigAsset>();

            projectConfig.ProjectId = projectId;
            projectConfig.EnableAnalytics = enableAnalytics;
            projectConfig.ChainConfigs = new List<ChainConfigEntry>
            {
                new()
                {
                    ChainId = chainId,
                    Chain = chain,
                    Network = network,
                    Symbol = symbol,
                    Rpc = rpc,
                    Ws = ws,
                    BlockExplorerUrl = blockExplorerUrl,
                }
            };

            return projectConfig;
        }

#if UNITY_EDITOR
        public static Web3ConfigAsset CreateOrLoad()
        {
            var projectConfig = Load();

            if (projectConfig == null)
            {
                string assetDirectory = Path.Combine(Application.dataPath, nameof(Resources));

                if (!Directory.Exists(assetDirectory))
                {
                    Directory.CreateDirectory(assetDirectory);
                }

                projectConfig = ScriptableObject.CreateInstance<Web3ConfigAsset>();
                UnityEditor.AssetDatabase.CreateAsset(projectConfig,
                    Path.Combine("Assets", nameof(Resources), $"{AssetName}.asset"));
            }

            return projectConfig;
        }

        public static void Save(Web3ConfigAsset web3Config)
        {
            UnityEditor.EditorUtility.SetDirty(web3Config);
            UnityEditor.AssetDatabase.SaveAssets();
        }
#endif

        public static IChainConfig BuildLocalhostConfig(string port = "8545", string chainId = "31337",
            string chain = "Anvil", string symbol = "ETH", string network = "GoChain Testnet")
        {
            return new LocalhostChainConfig(chainId, symbol, chain, network, port);
        }

        private class LocalhostChainConfig : IChainConfig
        {
            public LocalhostChainConfig(string chainId, string symbol, string chain, string network, string port)
            {
                var localhostEndPoint = $"127.0.0.1:{port}";

                ChainId = chainId;
                Symbol = symbol;
                Chain = chain;
                Network = network;
                Rpc = $"http://{localhostEndPoint}";
                Ws = $"ws://{localhostEndPoint}";
                BlockExplorerUrl = $"http://{localhostEndPoint}";
            }

            public string ChainId { get; }
            public string Symbol { get; }
            public string Chain { get; }
            public string Network { get; }
            public string Rpc { get; }
            public string Ipc => null;
            public string Ws { get; }
            public string BlockExplorerUrl { get; }
        }
    }
}