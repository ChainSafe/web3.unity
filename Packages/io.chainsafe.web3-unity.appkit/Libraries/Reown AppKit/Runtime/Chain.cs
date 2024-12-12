using System;
using System.Collections.Generic;

namespace Reown.AppKit.Unity
{
    [Serializable]
    public class Chain
    {
        public virtual string Name { get; }

        // https://github.com/wevm/viem/blob/main/src/chains/index.ts
        public virtual string ViemName { get; }
        public virtual Currency NativeCurrency { get; }
        public virtual BlockExplorer BlockExplorer { get; }
        public virtual string RpcUrl { get; }
        public virtual bool IsTestnet { get; }
        public virtual string ImageUrl { get; }

        // --- CAIP-2
        public virtual string ChainNamespace { get; }
        public virtual string ChainReference { get; }

        public virtual string ChainId
        {
            get => $"{ChainNamespace}:{ChainReference}";
        }
        // ---

        public Chain(
            string chainNamespace,
            string chainReference,
            string name,
            Currency nativeCurrency,
            BlockExplorer blockExplorer,
            string rpcUrl,
            bool isTestnet,
            string imageUrl,
            string viemName = null)
        {
            ChainNamespace = chainNamespace;
            ChainReference = chainReference;
            Name = name;
            NativeCurrency = nativeCurrency;
            BlockExplorer = blockExplorer;
            RpcUrl = rpcUrl;
            IsTestnet = isTestnet;
            ImageUrl = imageUrl;
            ViemName = viemName;
        }
    }

    [Serializable]
    public readonly struct Currency
    {
        public readonly string name;
        public readonly string symbol;
        public readonly int decimals;

        public Currency(string name, string symbol, int decimals)
        {
            this.name = name;
            this.symbol = symbol;
            this.decimals = decimals;
        }

        public static implicit operator Reown.Sign.Nethereum.Model.Currency(Currency currency)
        {
            return new Reown.Sign.Nethereum.Model.Currency(currency.name, currency.symbol, currency.decimals);
        }
    }

    [Serializable]
    public readonly struct BlockExplorer
    {
        public readonly string name;
        public readonly string url;

        public BlockExplorer(string name, string url)
        {
            this.name = name;
            this.url = url;
        }
    }

    public static class ChainConstants
    {
        internal const string ChainImageUrl = "https://api.web3modal.com/public/getAssetImage";

        public static class Namespaces
        {
            public const string Evm = "eip155";
            public const string Algorand = "algorand";
            public const string Solana = "sol";
        }

        public static class References
        {
            public const string Ethereum = "1";
            public const string EthereumGoerli = "5";
            public const string Optimism = "10";
            public const string Ronin = "2020";
            public const string RoninSaigon = "2021";
            public const string Base = "8453";
            public const string BaseGoerli = "84531";
            public const string Arbitrum = "42161";
            public const string Celo = "42220";
            public const string CeloAlfajores = "44787";
            public const string Solana = "5eykt4UsFv8P8NJdTREpY1vzqKqZKvdp";
            public const string Polygon = "137";
            public const string Avalanche = "43114";
        }

        // https://specs.walletconnect.com/2.0/specs/meta-clients/web3modal/api#known-static-asset-ids
        public static Dictionary<string, string> ImageIds { get; } = new()
        {
            // Ethereum
            { References.Ethereum, "692ed6ba-e569-459a-556a-776476829e00" },
            // Ethereum Goerli
            { References.EthereumGoerli, "692ed6ba-e569-459a-556a-776476829e00" },
            // Optimism
            { References.Optimism, "ab9c186a-c52f-464b-2906-ca59d760a400" },
            // Ronin
            { References.Ronin, "b8101fc0-9c19-4b6f-ec65-f6dfff106e00" },
            // RoninSaigon
            { References.RoninSaigon, "b8101fc0-9c19-4b6f-ec65-f6dfff106e00" },
            // Arbitrum
            { References.Arbitrum, "600a9a04-c1b9-42ca-6785-9b4b6ff85200" },
            // Celo
            { References.Celo, "ab781bbc-ccc6-418d-d32d-789b15da1f00" },
            // Celo Alfajores
            { References.CeloAlfajores, "ab781bbc-ccc6-418d-d32d-789b15da1f00" },
            // Base
            { References.Base, "7289c336-3981-4081-c5f4-efc26ac64a00" },
            // Base Goerli
            { References.BaseGoerli, "7289c336-3981-4081-c5f4-efc26ac64a00" },
            // Polygon
            { References.Polygon, "41d04d42-da3b-4453-8506-668cc0727900" },
            // Avalanche
            { References.Avalanche, "30c46e53-e989-45fb-4549-be3bd4eb3b00" },
            // Solana
            { References.Solana, "a1b58899-f671-4276-6a5e-56ca5bd59700" }
        };

        public static class Chains
        {
            public static readonly Chain Ethereum = new(
                Namespaces.Evm,
                References.Ethereum,
                "Ethereum",
                new Currency("Ether", "ETH", 18),
                new BlockExplorer("Etherscan", "https://etherscan.io"),
                "https://cloudflare-eth.com",
                false,
                $"{ChainImageUrl}/{ImageIds[References.Ethereum]}",
                "mainnet"
            );

            public static readonly Chain EthereumGoerli = new(
                Namespaces.Evm,
                References.EthereumGoerli,
                "Ethereum Goerli",
                new Currency("Ether", "ETH", 18),
                new BlockExplorer("Etherscan", "https://goerli.etherscan.io"),
                "https://goerli.infura.io/v3/",
                true,
                $"{ChainImageUrl}/{ImageIds[References.EthereumGoerli]}",
                "goerli"
            );

            public static readonly Chain Optimism = new(
                Namespaces.Evm,
                References.Optimism,
                "Optimism",
                new Currency("Ether", "ETH", 18),
                new BlockExplorer("Optimistic Etherscan", "https://optimistic.etherscan.io"),
                "https://mainnet.optimism.io",
                false,
                $"{ChainImageUrl}/{ImageIds[References.Optimism]}",
                "optimism"
            );

            public static readonly Chain Ronin = new(
                Namespaces.Evm,
                References.Ronin,
                "Ronin",
                new Currency("Ronin", "RON", 18),
                new BlockExplorer("Ronin Explorer", "https://app.roninchain.com/"),
                "https://api.roninchain.com/rpc",
                false,
                $"{ChainImageUrl}/{ImageIds[References.Ronin]}",
                "ronin"
            );

            public static readonly Chain RoninSaigon = new(
                Namespaces.Evm,
                References.RoninSaigon,
                "Ronin Saigon",
                new Currency("Ronin", "RON", 18),
                new BlockExplorer("Ronin Explorer", "https://explorer.roninchain.com"),
                "\thttps://api-gateway.skymavis.com/rpc/testnet",
                false,
                $"{ChainImageUrl}/{ImageIds[References.Ronin]}",
                "saigon"
            );

            public static readonly Chain Arbitrum = new(
                Namespaces.Evm,
                References.Arbitrum,
                "Arbitrum",
                new Currency("Ether", "ETH", 18),
                new BlockExplorer("Arbitrum Explorer", "https://arbiscan.io"),
                "https://arb1.arbitrum.io/rpc",
                false,
                $"{ChainImageUrl}/{ImageIds[References.Arbitrum]}",
                "arbitrum"
            );

            public static readonly Chain Celo = new(
                Namespaces.Evm,
                References.Celo,
                "Celo",
                new Currency("Celo", "CELO", 18),
                new BlockExplorer("Celo Explorer", "https://explorer.celo.org"),
                "https://forno.celo.org",
                false,
                $"{ChainImageUrl}/{ImageIds[References.Celo]}",
                "celo"
            );

            public static readonly Chain CeloAlfajores = new(
                Namespaces.Evm,
                References.CeloAlfajores,
                "Celo Alfajores",
                new Currency("Celo", "CELO", 18),
                new BlockExplorer("Celo Explorer", "https://alfajores-blockscout.celo-testnet.org"),
                "https://alfajores-forno.celo-testnet.org",
                true,
                $"{ChainImageUrl}/{ImageIds[References.CeloAlfajores]}",
                "celoAlfajores"
            );

            public static readonly Chain Base = new(
                Namespaces.Evm,
                References.Base,
                "Base",
                new Currency("Ether", "ETH", 18),
                new BlockExplorer("BaseScan", "https://basescan.org/"),
                "https://mainnet.base.org",
                false,
                $"{ChainImageUrl}/{ImageIds[References.Base]}",
                "base"
            );

            public static readonly Chain BaseGoerli = new(
                Namespaces.Evm,
                References.BaseGoerli,
                "Base Goerli",
                new Currency("Ether", "ETH", 18),
                new BlockExplorer("BaseScan", "https://goerli.basescan.org/"),
                "https://goerli.base.org",
                true,
                $"{ChainImageUrl}/{ImageIds[References.BaseGoerli]}",
                "baseGoerli"
            );

            public static readonly Chain Polygon = new(
                Namespaces.Evm,
                "137",
                "Polygon",
                new Currency("Polygon Ecosystem Token", "POL", 18),
                new BlockExplorer("Polygon Explorer", "https://polygonscan.com"),
                "https://polygon-rpc.com",
                false,
                $"{ChainImageUrl}/{ImageIds[References.Polygon]}",
                "polygon"
            );

            public static readonly Chain Avalanche = new(
                Namespaces.Evm,
                References.Avalanche,
                "Avalanche",
                new Currency("AVAX", "AVAX", 18),
                new BlockExplorer("Avalanche Explorer", "https://snowtrace.io/"),
                "https://api.avax.network/ext/bc/C/rpc",
                false,
                $"{ChainImageUrl}/{ImageIds[References.Avalanche]}",
                "avalanche"
            );

            public static readonly Chain Solana = new(
                Namespaces.Solana,
                References.Solana,
                "Solana",
                new Currency("Sol", "SOL", 9),
                new BlockExplorer("Solana Explorer", "https://explorer.solana.com"),
                "https://api.mainnet-beta.solana.com",
                false,
                $"{ChainImageUrl}/{ImageIds[References.Solana]}",
                "solana"
            );

            public static readonly IReadOnlyCollection<Chain> All = new HashSet<Chain>
            {
                Ethereum,
                EthereumGoerli,
                Optimism,
                Ronin,
                RoninSaigon,
                Arbitrum,
                Celo,
                CeloAlfajores,
                Base,
                BaseGoerli,
                Polygon,
                Avalanche,
                Solana
            };
        }
    }
}