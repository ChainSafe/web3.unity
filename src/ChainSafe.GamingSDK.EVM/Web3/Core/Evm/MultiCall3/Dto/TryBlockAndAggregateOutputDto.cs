using System.Collections.Generic;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Org.BouncyCastle.Math;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall3.Dto
{
    /// <summary>
    /// Backwards-compatible with MultiCall2.
    /// Aggregate calls and allow failures using tryAggregate.
    /// </summary>
    public partial class TryBlockAndAggregateOutputDto : TryBlockAndAggregateOutputDtoBase
    {
    }

    [FunctionOutput]
    public class TryBlockAndAggregateOutputDtoBase : IFunctionOutputDTO
    {
        [Parameter("uint256", "blockNumber", 1)]
        public virtual BigInteger BlockNumber { get; set; }

        [Parameter("bytes32", "blockHash", 2)]
        public virtual byte[] BlockHash { get; set; }

        [Parameter("tuple[]", "returnData", 3)]
        public virtual List<Result> ReturnData { get; set; }
    }
}