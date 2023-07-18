using System.Collections.Generic;
using ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall3;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall3.Dto
{
    /// <summary>
    /// Backwards-compatible with MultiCall2.
    /// Aggregate calls without requiring success.
    /// </summary>
    public partial class TryAggregateFunction : TryAggregateFunctionBase
    {
    }

    [Function("tryAggregate", typeof(TryAggregateOutputDto))]
    public class TryAggregateFunctionBase : FunctionMessage
    {
        [Parameter("bool", "requireSuccess", 1)]
        public virtual bool RequireSuccess { get; set; }

        [Parameter("tuple[]", "calls", 2)]
        public virtual List<Call> Calls { get; set; }
    }
}