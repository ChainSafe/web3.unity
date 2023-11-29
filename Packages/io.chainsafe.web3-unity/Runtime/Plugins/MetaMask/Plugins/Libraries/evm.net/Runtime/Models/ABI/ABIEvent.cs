using Newtonsoft.Json;

namespace evm.net.Models.ABI
{
    public class ABIEvent : ABIDef
    {
        [JsonProperty("anonymous")]
        public bool Anonymous { get; set; }
    }
}