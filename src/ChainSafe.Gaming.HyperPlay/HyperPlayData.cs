using Newtonsoft.Json;

namespace ChainSafe.Gaming.HyperPlay
{
    public class HyperPlayData : IHyperPlayData
    {
        [JsonIgnore]
        public string StoragePath => "hyperplay-data.json";

        [JsonIgnore]
        public bool LoadOnInitialize => true;

        public bool RememberSession { get; set; }

        public string SavedAccount { get; set; }
    }
}