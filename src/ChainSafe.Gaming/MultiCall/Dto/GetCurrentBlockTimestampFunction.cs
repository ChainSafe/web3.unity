using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace ChainSafe.Gaming.MultiCall.Dto
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