using System.Reflection;
using ChainSafe.Gaming.Evm.Utils;

namespace ChainSafe.Gaming.Marketplace
{
    public struct MarketplaceConfig : IMarketplaceConfig
    {
        public static MarketplaceConfig Default => new MarketplaceConfig
        {
            EndpointOverride = "https://api.gaming.chainsafe.io",
            MarketplaceContractAbi = ReadDefaultAbiFromResources(),
        };

        public string? EndpointOverride { get; set; }

        public string? MarketplaceContractAbi { get; set; }

        private static string ReadDefaultAbiFromResources()
        {
            return AbiHelper.ReadAbiFromResources(Assembly.GetExecutingAssembly(), "ChainSafe.Gaming.Marketplace.marketplace-abi.json");
        }
    }
}