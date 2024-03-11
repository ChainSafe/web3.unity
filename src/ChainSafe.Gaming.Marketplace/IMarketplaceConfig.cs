namespace ChainSafe.Gaming.Marketplace
{
    public interface IMarketplaceConfig
    {
        string EndpointOverride { get; set; }

        string? MarketplaceContractAbi { get; set; }
    }
}