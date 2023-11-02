using Nethereum.ABI.FunctionEncoding.Attributes;

namespace ChainSafe.Gaming.MultiCall.Dto
{
    /// <summary>
    /// Returns the block hash of the last block.
    /// </summary>
    public partial class GetLastBlockHashOutputDto : GetLastBlockHashOutputDtoBase
    {
    }

    [FunctionOutput]
    public class GetLastBlockHashOutputDtoBase : IFunctionOutputDTO
    {
        [Parameter("bytes32", "blockHash", 1)]
        public virtual byte[] BlockHash { get; set; }
    }
}