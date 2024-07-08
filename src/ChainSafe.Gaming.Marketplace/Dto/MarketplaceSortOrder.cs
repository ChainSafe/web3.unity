// <copyright file="MarketplaceSortOrder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ChainSafe.Gaming.Marketplace.Dto
{
    using System;

    public enum MarketplaceSortOrder
    {
        Ascending,
        Descending,
    }

    public static class MarketplaceSortOrderExtensions
    {
        public static string ToRequestParameter(this MarketplaceSortOrder type)
        {
            switch (type)
            {
                case MarketplaceSortOrder.Ascending:
                    return "asc";
                case MarketplaceSortOrder.Descending:
                    return "desc";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}