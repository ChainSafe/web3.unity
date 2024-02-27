using System.Collections.Generic;
using ChainSafe.Gaming.SygmaClient.Types;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.SygmaClient.Dto
{
    public class RawConfig
    {
        [JsonProperty(PropertyName = "domains")]
        public List<EvmConfig> Domains { get; set; }
    }
}