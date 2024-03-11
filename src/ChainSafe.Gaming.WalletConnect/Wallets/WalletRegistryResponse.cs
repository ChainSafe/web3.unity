using System.Collections.Generic;
using ChainSafe.Gaming.WalletConnect.Models;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.WalletConnect.Wallets
{
    public class WalletRegistryResponse
    {
        [JsonProperty("listings")]
        public Dictionary<string, WalletModel> Listings { get; set; }
    }
}