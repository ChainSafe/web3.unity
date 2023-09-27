namespace ChainSafe.Gaming.Chainlink.Lootboxes
{
    using System.Numerics;

    public class Erc1155Reward
    {
        public string ContractAddress { get; set; }

        public BigInteger TokenId { get; set; }

        public BigInteger Amount { get; set; }
    }
}