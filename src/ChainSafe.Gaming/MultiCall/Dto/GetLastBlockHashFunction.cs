using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace ChainSafe.Gaming.MultiCall.Dto
{
    /// <summary>
    /// Returns the block hash of the last block.
    /// </summary>
    public partial class GetLastBlockHashFunction : GetLastBlockHashFunctionBase
    {
    }

    [Function("getLastBlockHash", "bytes32")]
    public class GetLastBlockHashFunctionBase : FunctionMessage
    {
    }
}