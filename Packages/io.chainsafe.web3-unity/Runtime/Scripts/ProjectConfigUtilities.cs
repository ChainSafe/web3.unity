using System.Collections;
using System.Collections.Generic;
using System.IO;
using ChainSafe.Gaming.Web3;
using UnityEngine;

namespace ChainSafe.Gaming.UnityPackage
{
    public static class ProjectConfigUtilities
    {
        private const string AssetName = "ProjectConfigData";

        public static ProjectConfigScriptableObject Load()
        {
            var projectConfig = Resources.Load<ProjectConfigScriptableObject>(AssetName);
            return projectConfig ? projectConfig : null;
        }

        public static ProjectConfigScriptableObject Create(string projectId, string chainId, string chain, string network,
            string symbol, string rpc, string blockExplorerUrl, bool enableAnalytics, string ws = "")
        {
            var projectConfig = ScriptableObject.CreateInstance<ProjectConfigScriptableObject>();

            projectConfig.ProjectId = projectId;
            projectConfig.ChainId = chainId;
            projectConfig.Chain = chain;
            projectConfig.Network = network;
            projectConfig.Symbol = symbol;
            projectConfig.Rpc = rpc;
            projectConfig.Ws = ws;
            projectConfig.BlockExplorerUrl = blockExplorerUrl;
            projectConfig.EnableAnalytics = enableAnalytics;

            return projectConfig;
        }

        public static IChainConfig BuildLocalhostConfig(string port = "8545", string chainId = "31337",
            string chain = "Anvil", string symbol = "ETH", string network = "GoChain Testnet")
        {
            return new LocalhostChainConfig(chainId, symbol, chain, network, port);
        }

#if UNITY_EDITOR
        public static ProjectConfigScriptableObject CreateOrLoad()
        {
            var projectConfig = Load();

            if (projectConfig == null)
            {
                string assetDirectory = Path.Combine(Application.dataPath, nameof(Resources));

                if (!Directory.Exists(assetDirectory))
                {
                    Directory.CreateDirectory(assetDirectory);
                }

                projectConfig = ScriptableObject.CreateInstance<ProjectConfigScriptableObject>();
                UnityEditor.AssetDatabase.CreateAsset(projectConfig,
                    Path.Combine("Assets", nameof(Resources), $"{AssetName}.asset"));
            }

            return projectConfig;
        }

        public static void Save(ProjectConfigScriptableObject projectConfig)
        {
            UnityEditor.EditorUtility.SetDirty(projectConfig);
            UnityEditor.AssetDatabase.SaveAssets();
        }
#endif
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