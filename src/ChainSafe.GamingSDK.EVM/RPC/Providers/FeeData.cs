using System.Numerics;

namespace ChainSafe.GamingWeb3.Evm.Providers
{
    public class FeeData
    {
        public BigInteger MaxFeePerGas { get; set; }
        public BigInteger MaxPriorityFeePerGas { get; set; }
        public BigInteger GasPrice { get; set; }
    }
}