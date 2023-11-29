using Newtonsoft.Json;

namespace evm.net.Models.ABI
{
    public class FunctionAbiOutput
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}