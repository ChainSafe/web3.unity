using Newtonsoft.Json;

namespace ChainSafe.Gaming.HyperPlay.Dto
{
    public struct Request
    {
        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("params", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public object[] Params { get; set; }
    }
}