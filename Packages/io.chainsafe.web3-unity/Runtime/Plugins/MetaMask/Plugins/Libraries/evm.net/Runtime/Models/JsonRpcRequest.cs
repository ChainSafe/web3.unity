using System;
using Newtonsoft.Json;

namespace evm.net.Models
{
    public class JsonRpcRequest : JsonRpcBase
    {
        [JsonProperty("method")]
        public string Method { get; }
        
        [JsonProperty("params")]
        public object[] Parameters { get; }

        public JsonRpcRequest(string method, object[] parameters)
        {
            Method = method;
            Parameters = parameters;

            Id = RandomId(1, long.MaxValue, new Random()).ToString();
        }
    }
}