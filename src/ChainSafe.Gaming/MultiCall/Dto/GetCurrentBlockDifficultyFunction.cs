using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace ChainSafe.Gaming.MultiCall.Dto
{
    /// <summary>
    /// Returns the block difficulty.
    /// </summary>
    public partial class GetCurrentBlockDifficultyFunction : GetCurrentBlockDifficultyFunctionBase
    {
    }

    [Function("getCurrentBlockDifficulty", "uint256")]
    public class GetCurrentBlockDifficultyFunctionBase : FunctionMessage
    {
    }
}