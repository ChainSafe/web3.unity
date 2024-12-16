using Newtonsoft.Json;

namespace ChainSafe.Gaming.Reown.AppKit
{
    public class EthSignMessageAppKit
    {
        public EthSignMessageAppKit(params object[] x)
        {
            Message = x[0].ToString();
        }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}