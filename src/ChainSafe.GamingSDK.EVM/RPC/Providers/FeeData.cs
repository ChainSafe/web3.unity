using System.Numerics;

namespace Web3Unity.Scripts.Library.Ethers.Providers
{
    public class FeeData
    {
        public BigInteger MaxFeePerGas { get; set; }

        public BigInteger MaxPriorityFeePerGas { get; set; }

        public BigInteger GasPrice { get; set; }
    }
}