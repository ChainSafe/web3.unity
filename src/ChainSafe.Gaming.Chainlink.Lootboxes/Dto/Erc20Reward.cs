namespace ChainSafe.Gaming.Chainlink.Lootboxes
{
    using System.Numerics;

    public class Erc20Reward
    {
        public string ContractAddress { get; set; }

        /// <summary>
        /// Gets or sets raw amount of tokens. Divide this by ERC20.decimals() before showing to user.
        /// </summary>
        public BigInteger AmountRaw { get; set; }
    }
}