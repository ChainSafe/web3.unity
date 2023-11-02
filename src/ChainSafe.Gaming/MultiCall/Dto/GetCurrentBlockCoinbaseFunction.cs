using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace ChainSafe.Gaming.MultiCall.Dto
{
    /// <summary>
    /// Returns the block coinbase.
    /// </summary>
    public partial class GetCurrentBlockCoinbaseFunction : GetCurrentBlockCoinbaseFunctionBase
    {
    }

    [Function("getCurrentBlockCoinbase", "address")]
    public class GetCurrentBlockCoinbaseFunctionBase : FunctionMessage
    {
    }
}