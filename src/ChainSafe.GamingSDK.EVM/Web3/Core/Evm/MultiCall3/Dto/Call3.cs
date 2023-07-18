using Nethereum.ABI.FunctionEncoding.Attributes;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall3.Dto
{
    /// <summary>
    /// Standard Call3 return object.
    /// </summary>
    public partial class Call3 : Call3Base
    {
    }

    public class Call3Base
    {
        [Parameter("address", "target", 1)]
        public virtual string Target { get; set; }

        [Parameter("bool", "allowFailure", 2)]
        public virtual bool AllowFailure { get; set; }

        [Parameter("bytes", "callData", 3)]
        public virtual byte[] CallData { get; set; }
    }
}