using System.Collections.Generic;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall.Dto
{
    /// <summary>
    /// Calls the Aggregate3 function with value.
    /// </summary>
    public partial class Aggregate3ValueFunction : Aggregate3ValueFunctionBase
    {
    }

    [Function("aggregate3Value", typeof(Aggregate3ValueOutputDto))]
    public class Aggregate3ValueFunctionBase : FunctionMessage
    {
        [Parameter("tuple[]", "calls", 1)]
        public virtual List<Call3Value> Calls { get; set; }
    }
}