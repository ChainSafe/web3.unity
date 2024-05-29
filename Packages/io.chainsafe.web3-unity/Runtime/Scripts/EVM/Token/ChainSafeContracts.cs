using System.Collections.Generic;

namespace Scripts.EVM.Token
{
    public class ChainSafeContracts
    {
        public const string Erc20 = "0x358969310231363CBEcFEFe47323139569D8a88b";
        public const string Erc721 = "0x4f75BB7bdd6f7A0fD32f1b3A94dfF409F5a3F1CC";
        public const string Erc1155 = "0xAA2EbE78aa788d13AfFaaefD38C93333bbC4d51e";
        public const string ArrayTotal = "0x9839293240C535d8009920390b4D3DA256d31177";
        public static readonly Dictionary<string, string> MarketplaceContracts = new Dictionary<string, string>
        {
            // Avalanche mainnet
            { "43114", "0xfd9ba4301d277c8593280b54113900f4022e9d43" },
            // Avalanche testnet
            { "43113", "0x471232C9f11019045BA6Cf0183469e637ad4aB74" },
            // Binance mainnet
            { "56", "0xfd9ba4301d277c8593280b54113900f4022e9d43" },
            // Binance testnet
            { "97", "0x471232C9f11019045BA6Cf0183469e637ad4aB74" },
            // Eth mainnet
            { "1", "0xfd9ba4301d277c8593280b54113900f4022e9d43" },
            // Sepolia testnet
            { "11155111", "0x471232C9f11019045BA6Cf0183469e637ad4aB74" },
            // Polygon mainnet
            { "137", "0xfd9ba4301d277c8593280b54113900f4022e9d43" },
            // Mumbai testnet
            { "80001", "0x471232C9f11019045BA6Cf0183469e637ad4aB74" }
        };
    }
}