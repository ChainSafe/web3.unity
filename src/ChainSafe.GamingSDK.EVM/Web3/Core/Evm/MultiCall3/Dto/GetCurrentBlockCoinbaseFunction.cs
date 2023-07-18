using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall3.Dto
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