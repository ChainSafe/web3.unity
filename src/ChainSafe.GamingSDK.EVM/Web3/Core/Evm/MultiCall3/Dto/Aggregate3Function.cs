using System.Collections.Generic;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall3.Dto
{
    /// <summary>
    /// Aggregate calls, ensuring each returns success if required.
    /// </summary>
    /// <returns>
    /// returnData An array of Result structs.
    /// </returns>
    public partial class Aggregate3Function : Aggregate3FunctionBase
    {
    }

    [Function("aggregate3", typeof(Aggregate3OutputDto))]
    public class Aggregate3FunctionBase : FunctionMessage
    {
        [Parameter("tuple[]", "calls")]
        public virtual List<Call3> Calls { get; set; }
    }
}