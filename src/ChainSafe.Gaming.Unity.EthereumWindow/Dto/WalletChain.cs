using Newtonsoft.Json;

namespace ChainSafe.Gaming.Unity.EthereumWindow.Dto
{
    public struct WalletChain
    {
        [JsonProperty("chainId")]
        public string ChainId { get; set; }

        [JsonProperty("chainName", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("rpcUrls", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] RpcUrls { get; set; }

        [JsonProperty("nativeCurrency", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public NativeCurrency NativeCurrency { get; set; }

        [JsonProperty("blockExplorerUrls", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] BlockExplorerUrls { get; set; }

        [JsonProperty("iconUrls", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] IconUrls { get; set; }
    }
}