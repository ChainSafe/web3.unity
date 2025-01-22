using Newtonsoft.Json;

namespace ChainSafe.Gaming.Reown.AppKit
{
    /// <summary>
    /// Unlike the original EthSignMessage, this class is used only for webGL when we are sending the signMessage request trough wagmi.
    /// </summary>
    public class EthSignMessageWagmi
    {
        public EthSignMessageWagmi(params object[] x)
        {
            Message = x[0].ToString();
        }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}