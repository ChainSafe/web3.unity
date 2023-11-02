using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace ChainSafe.Gaming.MultiCall.Dto
{
    /// <summary>
    ///  Gets the base fee of the given block.
    ///  Can revert if the BASEFEE opcode is not implemented by the given chain.
    /// </summary>
    public partial class GetBaseFeeOutputDto : GetBaseFeeOutputDtoBase
    {
    }

    [FunctionOutput]
    public class GetBaseFeeOutputDtoBase : IFunctionOutputDTO
    {
        [Parameter("uint256", "basefee", 1)]
        public virtual BigInteger BaseFee { get; set; }
    }
}