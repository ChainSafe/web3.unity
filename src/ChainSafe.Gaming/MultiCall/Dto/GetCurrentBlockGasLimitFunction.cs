using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace ChainSafe.Gaming.MultiCall.Dto
{
    /// <summary>
    /// Returns the block gas limit.
    /// </summary>
    public partial class GetCurrentBlockGasLimitFunction : GetCurrentBlockGasLimitFunctionBase
    {
    }

    [Function("getCurrentBlockGasLimit", "uint256")]
    public class GetCurrentBlockGasLimitFunctionBase : FunctionMessage
    {
    }
}