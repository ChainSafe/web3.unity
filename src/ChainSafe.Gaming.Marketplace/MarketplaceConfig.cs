namespace ChainSafe.Gaming.Marketplace
{
    public struct MarketplaceConfig : IMarketplaceConfig
    {
        public static MarketplaceConfig Default => new MarketplaceConfig
        {
            EndpointOverride = "https://api.gaming.chainsafe.io",
        };

        public string? EndpointOverride { get; set; }
    }
}