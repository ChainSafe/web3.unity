using System;
using Reown.AppKit.Unity.Model;

namespace Reown.AppKit.Unity
{
    [Serializable]
    public class AppKitConfig
    {
        public string[] includedWalletIds;
        public string[] excludedWalletIds;

        public ushort connectViewWalletsCountMobile = 3;
        public ushort connectViewWalletsCountDesktop = 2;

        public bool enableAnalytics = true;

        public bool enableEmail = true; // Currently supported only in WebGL
        public bool enableOnramp = true; // Currently supported only in WebGL
        public bool enableCoinbaseWallet = true; // Currently supported only in WebGL

        public SiweConfig siweConfig;

        public Chain[] supportedChains =
        {
            ChainConstants.Chains.Ethereum,
            ChainConstants.Chains.Arbitrum,
            ChainConstants.Chains.Polygon,
            ChainConstants.Chains.Avalanche,
            ChainConstants.Chains.Optimism,
            ChainConstants.Chains.Base,
            ChainConstants.Chains.Celo,
            ChainConstants.Chains.Ronin
        };

        public Wallet[] customWallets;

        public Metadata metadata;
        public string projectId;

        public AppKitConfig()
        {
            // Exclude Coinbase Wallet on native platforms because it doesn't use WalletConnect protocol
            // On WebGL the official Coinbase Wallet library is used
#if !UNITY_WEBGL
            ExcludeCoinbaseWallet();
#endif
        }
        
        public AppKitConfig(string projectId, Metadata metadata) : this()
        {
            this.projectId = projectId;
            this.metadata = metadata;
        }

#if !UNITY_WEBGL
        private void ExcludeCoinbaseWallet()
        {
            const string cbWalletId = "fd20dc426fb37566d803205b19bbc1d4096b248ac04548e3cfb6b3a38bd033aa";

            if (excludedWalletIds == null || excludedWalletIds.Length == 0)
            {
                excludedWalletIds = new[] { cbWalletId };
            }
            else if (Array.IndexOf(excludedWalletIds, cbWalletId) == -1)
            {
                var newWalletIds = new string[excludedWalletIds.Length + 1];
                excludedWalletIds.CopyTo(newWalletIds, 0);
                newWalletIds[^1] = cbWalletId;
                excludedWalletIds = newWalletIds;
            }
        }
#endif
    }

    public class Metadata
    {
        public readonly string Name;
        public readonly string Description;
        public readonly string Url;
        public readonly string IconUrl;
        public readonly RedirectData Redirect;

        public Metadata(string name, string description, string url, string iconUrl, RedirectData redirect = null)
        {
            Name = name;
            Description = description;
            Url = url;
            IconUrl = iconUrl;
            Redirect = redirect ?? new RedirectData();
        }

        public static implicit operator Core.Metadata(Metadata metadata)
        {
            return new Core.Metadata
            {
                Name = metadata.Name,
                Description = metadata.Description,
                Url = metadata.Url,
                Icons = new[] { metadata.IconUrl },
                Redirect = new Core.Models.RedirectData
                {
                    Native = metadata.Redirect.Native,
                    Universal = metadata.Redirect.Universal
                }
            };
        }
    }
    
    public class RedirectData
    {
        public string Native = string.Empty;
        public string Universal = string.Empty;
    }
}