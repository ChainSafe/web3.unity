using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall3.Dto
{
    /// <summary>
    /// Returns the block number.
    /// </summary>
    public partial class GetBlockNumberFunction : GetBlockNumberFunctionBase
    {
    }

    [Function("getBlockNumber", "uint256")]
    public class GetBlockNumberFunctionBase : FunctionMessage
    {
    }
}