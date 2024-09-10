namespace ChainSafe.Gaming.Marketplace
{
    using System;

    public static class MarketplaceItemStatusExtensions
    {
        public static MarketplaceItemStatus FromString(string rawValue)
        {
            return rawValue switch
            {
                "sold" => MarketplaceItemStatus.Sold,
                "listed" => MarketplaceItemStatus.Listed,
                "canceled" => MarketplaceItemStatus.Canceled,
                _ => throw new ArgumentOutOfRangeException(nameof(rawValue))
            };
        }

        public static string ToRequestParameter(this MarketplaceItemStatus value)
        {
            return value switch
            {
                MarketplaceItemStatus.Sold => "sold",
                MarketplaceItemStatus.Listed => "listed",
                MarketplaceItemStatus.Canceled => "canceled",
                _ => throw new ArgumentOutOfRangeException(nameof(value))
            };
        }
    }
}