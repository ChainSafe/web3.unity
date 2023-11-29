using Newtonsoft.Json;

namespace evm.net.Models
{
    public class JsonRpcResult<TR> : JsonRpcBase
    {
        [JsonProperty("result")]
        public TR Result { get; protected set; }
    }
}