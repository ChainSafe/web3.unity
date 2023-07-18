using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall3.Dto
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