using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall3.Dto
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