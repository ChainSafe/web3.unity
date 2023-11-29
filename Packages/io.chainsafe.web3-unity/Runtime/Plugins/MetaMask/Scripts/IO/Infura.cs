using System;
using System.Collections.Generic;

namespace MetaMask.IO
{
    public static class Infura
    {
        public static Dictionary<long, string> ChainIdToName = new Dictionary<long, string>()
        {
            { 0x1, "mainnet" },
            { 0x5, "goerli" },
            { 0xaa36a7, "sepolia" },
            { 0xe708, "linea-mainnet" },
            { 0xe704, "linea-goerli" },
            { 0x89, "polygon-mainnet" }, // TODO Check with Infura
            { 0xa, "optimism-mainnet" }, // TODO Check with Infura
            { 0xa4b1, "arbitrum-mainnet" }, // TODO Check with Infura
            { 0x2a15c308d, "palm-mainnet" },
            { 0x2a15c3083, "palm-testnet" },
            { 0xa86a, "avalanche-mainnet"},
            { 0xa869, "avalanche-fuji" },
            { 0x4e454152, "aurora-mainnet" },
            { 0x4e454153, "aurora-testnet" },
            { 0xa4ec, "celo-mainnet" },
            { 0xaef3, "celo-alfajores" },
        };

        public static string Url(string projectId, string chainName)
        {
            return $"https://{chainName}.infura.io/v3/{projectId}";
        }

        public static bool IsUrl(string url, bool enforceSSL = true)
        {
            Uri uri = new Uri(url);
            string scheme = uri.GetLeftPart(UriPartial.Scheme);
            string host = uri.GetLeftPart(UriPartial.Authority);

            return host.EndsWith(".infura.io") && (!enforceSSL || scheme == "https://");
        }
    }
}