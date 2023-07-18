using System.Collections.Generic;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Org.BouncyCastle.Math;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall3.Dto
{
    /// <summary>
    /// Parses return data from the Aggregate function.
    /// </summary>
    public partial class AggregateOutputDto : AggregateOutputDtoBase
    {
    }

    [FunctionOutput]
    public class AggregateOutputDtoBase : IFunctionOutputDTO
    {
        [Parameter("uint256", "blockNumber", 1)]
        public virtual BigInteger BlockNumber { get; set; }

        [Parameter("bytes[]", "returnData", 2)]
        public virtual List<byte[]> ReturnData { get; set; }
    }
}