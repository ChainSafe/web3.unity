using Newtonsoft.Json;

namespace ChainSafe.Gaming.HyperPlay.Dto
{
    public struct Chain
    {
        [JsonProperty("chainId")]
        public string ChainId { get; set; }
    }
}