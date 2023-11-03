using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace ChainSafe.Gaming.MultiCall.Dto
{
    /// <summary>
    /// Returns the block difficulty.
    /// </summary>
    public partial class GetCurrentBlockDifficultyOutputDto : GetCurrentBlockDifficultyOutputDtoBase
    {
    }

    [FunctionOutput]
    public class GetCurrentBlockDifficultyOutputDtoBase : IFunctionOutputDTO
    {
        [Parameter("uint256", "difficulty", 1)]
        public virtual BigInteger Difficulty { get; set; }
    }
}