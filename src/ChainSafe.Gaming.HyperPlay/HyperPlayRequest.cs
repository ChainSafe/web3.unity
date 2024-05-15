using Newtonsoft.Json;

namespace ChainSafe.Gaming.HyperPlay
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

    public struct Request
    {
        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("params", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public object[] Params { get; set; }
    }

    public struct Chain
    {
        [JsonProperty("chainId")]
        public string ChainId { get; set; }
    }

    /// <summary>
    /// Error model for HyperPlay RPC responses.
    /// </summary>
    public struct HyperPlayError
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}