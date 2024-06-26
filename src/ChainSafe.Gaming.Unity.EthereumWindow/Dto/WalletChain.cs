using Newtonsoft.Json;

namespace ChainSafe.Gaming.Unity.EthereumWindow.Dto
{
    /// <summary>
    /// Chain Data for switching and adding new chain to Ethereum Window (browser) wallet.
    /// </summary>
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