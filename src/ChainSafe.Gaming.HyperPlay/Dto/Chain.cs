using Newtonsoft.Json;

namespace ChainSafe.Gaming.HyperPlay.Dto
{
    public struct Chain
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

    public struct NativeCurrency
    {
        public NativeCurrency(string symbol)
        {
            Name = symbol;
            Symbol = symbol;
            Decimals = 18;
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("decimals")]
        public int Decimals { get; set; }
    }
}