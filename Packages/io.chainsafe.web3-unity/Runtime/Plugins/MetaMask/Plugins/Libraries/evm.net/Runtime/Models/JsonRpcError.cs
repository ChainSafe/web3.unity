using Newtonsoft.Json;

namespace evm.net.Models
{
    public class JsonRpcError : JsonRpcBase
    {
        [JsonProperty("error")]
        public GenericError Error { get; protected set; }
    }
}