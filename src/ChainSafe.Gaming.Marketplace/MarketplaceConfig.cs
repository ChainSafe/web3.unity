﻿namespace ChainSafe.Gaming.Marketplace
{
    public struct MarketplaceConfig : IMarketplaceConfig
    {
        private string? endpointOverride;

        public static MarketplaceConfig Default => new MarketplaceConfig
        {
            EndpointOverride = "https://api.gaming.chainsafe.io",
        };

        public string? EndpointOverride
        {
            readonly get => endpointOverride;
            set => endpointOverride = value;
        }
    }
}