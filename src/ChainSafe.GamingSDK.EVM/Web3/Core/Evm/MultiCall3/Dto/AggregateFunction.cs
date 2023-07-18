using System.Collections.Generic;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall3.Dto
{
    /// <summary>
    /// Backwards-compatible call aggregation with MultiCall.
    /// calls An array of Call structs.
    /// </summary>
    /// <returns>
    /// blockNumber The block number where the calls were executed.
    /// returnData An array of bytes containing the responses.
    /// </returns>
    public partial class AggregateFunction : AggregateFunctionBase
    {
    }

    [Function("aggregate", typeof(AggregateOutputDto))]
    public class AggregateFunctionBase : FunctionMessage
    {
        [Parameter("tuple[]", "calls", 1)]
        public virtual List<Call> Calls { get; set; }
    }
}