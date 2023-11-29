using Newtonsoft.Json;

namespace evm.net.Models.ABI
{
    public class ContractArtifact
    {
        [JsonProperty("contractName")]
        public string ContractName { get; set; }
        
        [JsonProperty("abi")]
        public ContractABI ABI { get; set; }
        
        [JsonProperty("bytecode")]
        public string Bytecode { get; set; }
    }
}