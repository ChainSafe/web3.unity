using Newtonsoft.Json;

namespace evm.net.Models.ABI
{
    public class FunctionAbiInput : FunctionAbiOutput
    {
        [JsonProperty("indexed")]
        public bool Indexed { get; set; }
    }
}