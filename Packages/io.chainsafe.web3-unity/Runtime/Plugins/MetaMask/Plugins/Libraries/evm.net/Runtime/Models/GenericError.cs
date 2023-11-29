using Newtonsoft.Json;

namespace evm.net.Models
{
    public class GenericError
    {
        [JsonProperty("code")]
        public long Code { get; set; }
        
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}