using System.Collections.Generic;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall3.Dto
{
    /// <summary>
    /// Backwards-compatible with MultiCall2.
    /// Aggregate calls without requiring success.
    /// </summary>
    public partial class TryAggregateOutputDto : TryAggregateOutputDtoBase
    {
    }

    [FunctionOutput]
    public class TryAggregateOutputDtoBase : IFunctionOutputDTO
    {
        [Parameter("tuple[]", "returnData", 1)]
        public virtual List<Result> ReturnData { get; set; }
    }
}