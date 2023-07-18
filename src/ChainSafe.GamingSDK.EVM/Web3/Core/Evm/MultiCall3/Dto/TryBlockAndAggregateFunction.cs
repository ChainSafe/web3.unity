using System.Collections.Generic;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall3.Dto
{
    /// <summary>
    /// Backwards-compatible with MultiCall2.
    /// Aggregate calls and allow failures using tryAggregate.
    /// </summary>
    public partial class TryBlockAndAggregateFunction : TryBlockAndAggregateFunctionBase
    {
    }

    [Function("tryBlockAndAggregate", typeof(TryBlockAndAggregateOutputDto))]
    public class TryBlockAndAggregateFunctionBase : FunctionMessage
    {
        [Parameter("bool", "requireSuccess", 1)]
        public virtual bool RequireSuccess { get; set; }

        [Parameter("tuple[]", "calls", 2)]
        public virtual List<Call> Calls { get; set; }
    }
}