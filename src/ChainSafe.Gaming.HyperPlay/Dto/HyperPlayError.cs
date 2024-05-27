using Newtonsoft.Json;

namespace ChainSafe.Gaming.HyperPlay.Dto
{
    /// <summary>
    /// Error model for HyperPlay RPC responses.
    /// </summary>
    public struct HyperPlayError
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}