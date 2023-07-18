using Nethereum.ABI.FunctionEncoding.Attributes;
using Org.BouncyCastle.Math;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall3.Dto
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