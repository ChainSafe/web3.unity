using Nethereum.ABI.FunctionEncoding.Attributes;
using Org.BouncyCastle.Math;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall3.Dto
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