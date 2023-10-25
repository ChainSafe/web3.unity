using System.Numerics;

namespace ChainSafe.Gaming.Evm.Providers
{
    /// <summary>
    /// Represents fee relevant data in the Ethereum network.
    /// </summary>
    public class FeeData
    {
        /// <summary>
        /// Gets or sets the base fee per gas.
        /// </summary>
        public BigInteger BaseFeePerGas { get; set; }

        /// <summary>
        /// Gets or sets the maximum fee per gas that the sender is willing to pay.
        /// </summary>
        public BigInteger MaxFeePerGas { get; set; }

        /// <summary>
        /// Gets or sets the maximum fee per gas to be given to the miner.
        /// </summary>
        public BigInteger MaxPriorityFeePerGas { get; set; }

        /// <summary>
        /// Gets or sets the price of gas.
        /// </summary>
        public BigInteger GasPrice { get; set; }
    }
}