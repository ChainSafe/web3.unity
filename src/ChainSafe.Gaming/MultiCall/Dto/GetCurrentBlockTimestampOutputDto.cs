using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace ChainSafe.Gaming.MultiCall.Dto
{
    /// <summary>
    /// Returns the block timestamp.
    /// </summary>
    public partial class GetCurrentBlockTimestampOutputDto : GetCurrentBlockTimestampOutputDtoBase
    {
    }

    [FunctionOutput]
    public class GetCurrentBlockTimestampOutputDtoBase : IFunctionOutputDTO
    {
        [Parameter("uint256", "timestamp", 1)]
        public virtual BigInteger Timestamp { get; set; }
    }
}