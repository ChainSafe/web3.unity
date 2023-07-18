using System.Collections.Generic;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall.Dto
{
    /// <summary>
    /// Parses return data from the Aggregate3 with value field populated.
    /// </summary>
    public partial class Aggregate3ValueOutputDto : Aggregate3ValueOutputDtoBase
    {
    }

    [FunctionOutput]
    public class Aggregate3ValueOutputDtoBase : IFunctionOutputDTO
    {
        [Parameter("tuple[]", "returnData", 1)]
        public virtual List<Result> ReturnData { get; set; }
    }
}