using Nethereum.ABI.FunctionEncoding.Attributes;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall3.Dto
{
    /// <summary>
    /// Returns the block coinbase.
    /// </summary>
    public partial class GetCurrentBlockCoinbaseOutputDto : GetCurrentBlockCoinbaseOutputDtoBase
    {
    }

    [FunctionOutput]
    public class GetCurrentBlockCoinbaseOutputDtoBase : IFunctionOutputDTO
    {
        [Parameter("address", "coinbase", 1)]
        public virtual string Coinbase { get; set; }
    }
}