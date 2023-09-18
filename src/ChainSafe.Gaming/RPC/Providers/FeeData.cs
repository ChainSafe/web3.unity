using System.Numerics;

namespace ChainSafe.Gaming.Evm.Providers
{
    public class FeeData
    {
        public BigInteger BaseFeePerGas { get; set; }

        public BigInteger MaxFeePerGas { get; set; }

        public BigInteger MaxPriorityFeePerGas { get; set; }

        public BigInteger GasPrice { get; set; }
    }
}