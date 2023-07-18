using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall3.Dto
{
    /// <summary>
    /// Returns the block timestamp.
    /// </summary>
    public partial class GetCurrentBlockTimestampFunction : GetCurrentBlockTimestampFunctionBase
    {
    }

    [Function("getCurrentBlockTimestamp", "uint256")]
    public class GetCurrentBlockTimestampFunctionBase : FunctionMessage
    {
    }
}