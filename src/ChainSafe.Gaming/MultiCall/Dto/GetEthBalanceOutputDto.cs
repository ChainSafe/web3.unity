using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace ChainSafe.Gaming.MultiCall.Dto
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