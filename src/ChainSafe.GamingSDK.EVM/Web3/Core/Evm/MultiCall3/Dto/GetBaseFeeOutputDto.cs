using Nethereum.ABI.FunctionEncoding.Attributes;
using Org.BouncyCastle.Math;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall3.Dto
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