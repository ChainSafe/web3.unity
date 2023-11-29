using Newtonsoft.Json;

namespace evm.net.Models.ABI
{
    public class ABIFunction : ABIDef
    {
        [JsonProperty("outputs")]
        public ABIParameter[] Outputs { get; set; }
        
        [JsonProperty("stateMutability")]
        public ABIStateMutability StateMutability { get; set; }
    }
}