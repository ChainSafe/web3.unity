using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace ChainSafe.Gaming.MultiCall.Dto
{
    /// <summary>
    /// Returns the chain id.
    /// </summary>
    public partial class GetChainIdOutputDto : GetChainIdOutputDtoBase
    {
    }

    [FunctionOutput]
    public class GetChainIdOutputDtoBase : IFunctionOutputDTO
    {
        [Parameter("uint256", "chainid", 1)]
        public virtual BigInteger ChainId { get; set; }
    }
}