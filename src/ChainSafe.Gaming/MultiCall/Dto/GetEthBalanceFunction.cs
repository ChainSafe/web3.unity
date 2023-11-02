using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace ChainSafe.Gaming.MultiCall.Dto
{
    /// <summary>
    /// Returns the (ETH) balance of a given address.
    /// </summary>
    public partial class GetEthBalanceFunction : GetEthBalanceFunctionBase
    {
    }

    [Function("getEthBalance", "uint256")]
    public class GetEthBalanceFunctionBase : FunctionMessage
    {
        [Parameter("address", "addr", 1)]
        public virtual string Addr { get; set; }
    }
}