using System.Collections.Generic;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.UnityPackage.Model
{
    public struct NativeCurrency
    {
        [JsonProperty(PropertyName = "symbol")]        
        public string Symbol { get; set; }
        
        [JsonProperty(PropertyName = "decimals")]  
        public int Decimals { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }

    public struct Explorer
    {
        public string name { get; set; }
        public string url { get; set; }
        public string standard { get; set; }
    }

    public struct Root
    {
        public string name { get; set; }
        public string chain { get; set; }
        public List<string> rpc { get; set; }
        public NativeCurrency nativeCurrency { get; set; }
        public object chainId { get; set; }
        public List<Explorer> explorers { get; set; }
        public bool allowCustomValues { get; set; }
    }
}