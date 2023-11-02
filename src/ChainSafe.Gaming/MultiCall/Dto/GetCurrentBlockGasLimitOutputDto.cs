using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace ChainSafe.Gaming.MultiCall.Dto
{
    /// <summary>
    /// Returns the block gas limit.
    /// </summary>
    public partial class GetCurrentBlockGasLimitOutputDto : GetCurrentBlockGasLimitOutputDtoBase
    {
    }

    [FunctionOutput]
    public class GetCurrentBlockGasLimitOutputDtoBase : IFunctionOutputDTO
    {
        [Parameter("uint256", "gaslimit", 1)]
        public virtual BigInteger GasLimit { get; set; }
    }
}