using Nethereum.ABI.FunctionEncoding.Attributes;
using Org.BouncyCastle.Math;

namespace ChainSafe.Gaming.MultiCall.Dto
{
    /// <summary>
    /// Returns the block number.
    /// </summary>
    public partial class GetBlockNumberOutputDto : GetBlockNumberOutputDtoBase
    {
    }

    [FunctionOutput]
    public class GetBlockNumberOutputDtoBase : IFunctionOutputDTO
    {
        [Parameter("uint256", "blockNumber", 1)]
        public virtual BigInteger BlockNumber { get; set; }
    }
}