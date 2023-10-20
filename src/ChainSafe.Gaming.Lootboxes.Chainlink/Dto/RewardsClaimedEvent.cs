namespace ChainSafe.Gaming.Lootboxes.Chainlink
{
    using System.Numerics;
    using Nethereum.ABI.FunctionEncoding.Attributes;

    [Event("RewardsClaimed")]
    public class RewardsClaimedEvent : IEventDTO
    {
        [Parameter("address", "opener", 1, false)]
        public string OpenerAddress { get; set; }

        [Parameter("address", "token", 2, false)]
        public string TokenAddress { get; set; }

        [Parameter("uint256", "tokenId", 3, false)]
        public BigInteger TokenId { get; set; }

        [Parameter("uint256", "amount", 4, false)]
        public BigInteger Amount { get; set; }
    }
}