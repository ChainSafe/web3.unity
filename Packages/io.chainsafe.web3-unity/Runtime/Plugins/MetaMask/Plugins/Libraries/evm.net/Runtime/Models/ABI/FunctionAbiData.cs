using Newtonsoft.Json;

namespace evm.net.Models.ABI
{
    public class FunctionAbiData
    {
        [JsonProperty("constant")]
        public bool Constant { get; set; }
        
        [JsonProperty("inputs")]
        public FunctionAbiInput[] Inputs { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("outputs")]
        public FunctionAbiOutput[] Outputs { get; set; }
        
        [JsonProperty("payable")]
        public bool Payable { get; set; }
        
        [JsonProperty("stateMutability")]
        public string StateMutability { get; set; }
        
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("anonymous")]
        public bool Anonymous { get; set; }
    }
}