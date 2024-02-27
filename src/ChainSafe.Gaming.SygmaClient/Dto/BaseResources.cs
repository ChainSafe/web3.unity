using Newtonsoft.Json;

namespace ChainSafe.Gaming.SygmaClient.Dto
{
    public class BaseResources
    {
        [JsonProperty(PropertyName = "resourceId")]
        public string ResourceId { get; set; }

        [JsonProperty(PropertyName = "type")]
        public ResourceType Type { get; set; }

        [JsonProperty(PropertyName = "native")]
        public bool? Native { get; set; }

        [JsonProperty(PropertyName = "burnable")]
        public bool? Burnable { get; set; }

        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "decimals")]
        public int? Decimals { get; set; }
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class EvmResource : BaseResources
    {
        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }
    }
}