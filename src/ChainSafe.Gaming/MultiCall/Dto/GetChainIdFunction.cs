using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace ChainSafe.Gaming.MultiCall.Dto
{
    /// <summary>
    /// Returns the chain id.
    /// </summary>
    public partial class GetChainIdFunction : GetChainIdFunctionBase
    {
    }

    [Function("getChainId", "uint256")]
    public class GetChainIdFunctionBase : FunctionMessage
    {
    }
}