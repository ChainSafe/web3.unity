// <copyright file="Erc1155NftReward.cs" company="Chainsafe">
// Copyright (c) Chainsafe. All rights reserved.
// </copyright>

namespace ChainSafe.Gaming.Chainlink.Lootboxes
{
    using System.Numerics;

    public class Erc1155NftReward
    {
        public string ContractAddress { get; set; }

        public BigInteger TokenId { get; set; }
    }
}