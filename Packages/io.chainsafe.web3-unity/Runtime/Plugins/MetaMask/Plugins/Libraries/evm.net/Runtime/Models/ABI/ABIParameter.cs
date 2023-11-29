using Newtonsoft.Json;

namespace evm.net.Models.ABI
{
    public class ABIParameter
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("type")]
        public string TypeName { get; set; }
        
        [JsonProperty("components", DefaultValueHandling = DefaultValueHandling.Ignore, Required = Required.DisallowNull)]
        public ABIParameter[] Components { get; set; }
        
        [JsonProperty("internalType")]
        public string InternalType { get; set; }
        
        [JsonProperty("indexed", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Indexed { get; set; }
    }
}