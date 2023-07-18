using Nethereum.ABI.FunctionEncoding.Attributes;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall3.Dto
{
    /// <summary>
    /// Standard MultiCall call object, for backwards compatibility.
    /// </summary>
    public partial class Call : CallBase
    {
    }

    public class CallBase
    {
        [Parameter("address", "target", 1)]
        public virtual string Target { get; set; }

        [Parameter("bytes", "callData", 2)]
        public virtual byte[] CallData { get; set; }
    }
}