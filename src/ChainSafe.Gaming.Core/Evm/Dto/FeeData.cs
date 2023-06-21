using System.Numerics;

namespace ChainSafe.Gaming.Evm
{
    public class FeeData
    {
        public BigInteger MaxFeePerGas { get; set; }

        public BigInteger MaxPriorityFeePerGas { get; set; }

        public BigInteger GasPrice { get; set; }
    }
}