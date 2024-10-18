using System.Collections.Generic;
using ChainSafe.Gaming.Reown.Models;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.Reown.Wallets
{
    public class WalletRegistryResponse
    {
        [JsonProperty("count")] public int Count { get; set; }

        [JsonProperty("data")] public List<WalletModel> Data { get; set; }
    }
}