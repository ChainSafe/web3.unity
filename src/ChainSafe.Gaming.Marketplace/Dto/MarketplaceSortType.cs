using System;

namespace ChainSafe.Gaming.Marketplace.Dto
{
    public enum MarketplaceSortType
    {
        None,
        ListedAt,
        Price
    }

    public static class MarketplaceSortTypeExtensions
    {
        public static string ToRequestParameter(this MarketplaceSortType type)
        {
            switch (type)
            {
                case MarketplaceSortType.ListedAt:
                    return "listed_at";
                case MarketplaceSortType.Price:
                    return "price";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}