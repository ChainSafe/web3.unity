using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace ChainSafe.Gaming.MultiCall.Dto
{
    /// <summary>
    /// Returns the block hash for the given block number.
    /// </summary>
    public partial class GetBlockHashFunction : GetBlockHashFunctionBase
    {
    }

    [Function("getBlockHash", "bytes32")]
    public class GetBlockHashFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "blockNumber", 1)]
        public virtual BigInteger BlockNumber { get; set; }
    }
}