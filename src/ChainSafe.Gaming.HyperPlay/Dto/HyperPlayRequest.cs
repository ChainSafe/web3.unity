using Newtonsoft.Json;

namespace ChainSafe.Gaming.HyperPlay.Dto
{
    /// <summary>
    /// Request model for HyperPlay RPC requests.
    /// </summary>
    public struct HyperPlayRequest
    {
        [JsonProperty("request")]
        public Request Request { get; set; }

        [JsonProperty("chain")]
        public Chain Chain { get; set; }
    }
}