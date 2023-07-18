using Nethereum.ABI.FunctionEncoding.Attributes;
using Org.BouncyCastle.Math;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall3.Dto
{
    /// <summary>
    /// Call3 with value return object.
    /// </summary>
    public partial class Call3Value : Call3ValueBase
    {
    }

    public class Call3ValueBase
    {
        [Parameter("address", "target", 1)]
        public virtual string Target { get; set; }

        [Parameter("bool", "allowFailure", 2)]
        public virtual bool AllowFailure { get; set; }

        [Parameter("uint256", "value", 3)]
        public virtual BigInteger Value { get; set; }

        [Parameter("bytes", "callData", 4)]
        public virtual byte[] CallData { get; set; }
    }
}