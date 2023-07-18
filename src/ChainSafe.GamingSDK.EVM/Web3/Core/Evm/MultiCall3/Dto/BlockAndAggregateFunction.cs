using System.Collections.Generic;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall3.Dto
{
    /// <summary>
    /// Backwards-compatible with MultiCall2.
    /// Aggregate calls and allow failures using tryAggregate.
    /// </summary>
    /// <returns>
    /// blockNumber The block number where the calls were executed.
    /// blockHash The hash of the block where the calls were executed.
    /// returnData An array of Result structs.
    /// </returns>
    public partial class BlockAndAggregateFunction : BlockAndAggregateFunctionBase
    {
    }

    [Function("blockAndAggregate", typeof(BlockAndAggregateOutputDto))]
    public class BlockAndAggregateFunctionBase : FunctionMessage
    {
        [Parameter("tuple[]", "calls", 1)]
        public virtual List<Call> Calls { get; set; }
    }
}