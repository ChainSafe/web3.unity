using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace ChainSafe.Gaming.MultiCall.Dto
{
    /// <summary>
    /// Gets the base fee of the given block.
    /// Can revert if the BASEFEE opcode is not implemented by the given chain.
    /// </summary>
    public partial class GetBaseFeeFunction : GetBaseFeeFunctionBase
    {
    }

    [Function("getBasefee", "uint256")]
    public class GetBaseFeeFunctionBase : FunctionMessage
    {
    }
}