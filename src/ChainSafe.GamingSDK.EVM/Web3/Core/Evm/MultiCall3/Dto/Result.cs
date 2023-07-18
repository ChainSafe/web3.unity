using Nethereum.ABI.FunctionEncoding.Attributes;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall3.Dto
{
    /// <summary>
    /// MultiCall3 return object.
    /// </summary>
    public partial class Result : ResultBase
    {
    }

    public class ResultBase
    {
        [Parameter("bool", "success", 1)]
        public virtual bool Success { get; set; }

        [Parameter("bytes", "returnData", 2)]
        public virtual byte[] ReturnData { get; set; }
    }
}