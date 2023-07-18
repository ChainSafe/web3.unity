using Nethereum.ABI.FunctionEncoding.Attributes;
using Org.BouncyCastle.Math;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall3.Dto
{
    /// <summary>
    /// Returns the (ETH) balance of a given address.
    /// </summary>
    public partial class GetEthBalanceOutputDto : GetEthBalanceOutputDtoBase
    {
    }

    [FunctionOutput]
    public class GetEthBalanceOutputDtoBase : IFunctionOutputDTO
    {
        [Parameter("uint256", "balance", 1)]
        public virtual BigInteger Balance { get; set; }
    }
}