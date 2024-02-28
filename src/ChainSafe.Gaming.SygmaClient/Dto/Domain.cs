using ChainSafe.Gaming.SygmaClient.Dto;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.SygmaClient.Types
{
    public enum Environment
    {
        Local,
        Devnet,
        Testnet,
        Mainnet,
    }

    public class Domain
    {
        [JsonProperty(PropertyName = "id")]
        public uint Id { get; set; }

        [JsonProperty(PropertyName = "chainId")]
        public uint ChainId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "network")]
        public Network Type { get; set; }
    }
}