using System;
using Newtonsoft.Json;

namespace Reown.Sign.Nethereum.Model
{
    [Serializable]
    public class MetaMaskError
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}