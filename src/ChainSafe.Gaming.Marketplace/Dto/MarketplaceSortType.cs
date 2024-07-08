// <copyright file="MarketplaceSortType.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ChainSafe.Gaming.Marketplace.Dto
{
    using System;

    public enum MarketplaceSortType
    {
        None,
        ListedAt,
        Price,
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