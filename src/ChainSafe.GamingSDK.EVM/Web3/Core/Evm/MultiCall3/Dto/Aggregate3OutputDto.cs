using System.Collections.Generic;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall3.Dto
{
    /// <summary>
    /// Parses return data from the Aggregate function.
    /// </summary>
    /// <returns>
    /// returnData An array of Result structs.
    /// </returns>
    public partial class Aggregate3OutputDto : Aggregate3OutputDtoBase
    {
    }

    [FunctionOutput]
    public class Aggregate3OutputDtoBase : IFunctionOutputDTO
    {
        [Parameter("tuple[]", "returnData", 1)]
        public virtual List<Result> ReturnData { get; set; }
    }
}