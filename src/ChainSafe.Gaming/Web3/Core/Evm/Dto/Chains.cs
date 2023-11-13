using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Environment;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.Evm.Network
{
    public class Chains
    {
        public class Chain
        {
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }

            [JsonProperty(PropertyName = "chain")]
            public string ChainSymbol { get; set; }

            [JsonProperty(PropertyName = "rpc")]
            public string[] RPC { get; set; }

            [JsonProperty(PropertyName = "nativeCurrency")]
            public NativeCurrency NativeCurrencyInfo { get; set; }

            [JsonProperty(PropertyName = "infoURL")]
            public string InfoUrl { get; set; }

            [JsonProperty(PropertyName = "chainId")]
            public ulong ChainId { get; set; }

            [JsonProperty(PropertyName = "networkId")]
            public ulong NetworkId { get; set; }

            [JsonProperty(PropertyName = "ens")]
            public ENS Ens { get; set; }

            [JsonProperty(PropertyName = "explorers")]
            public Explorer[] Explorers { get; set; }

            public class ENS
            {
                [JsonProperty(PropertyName = "registry")]
                public string Registry { get; set; }
            }

            public class Explorer
            {
                [JsonProperty(PropertyName = "name")]
                public string Name { get; set; }

                [JsonProperty(PropertyName = "url")]
                public string Url { get; set; }
            }

            public class NativeCurrency
            {
                [JsonProperty(PropertyName = "chain")]
                public string Name { get; set; }

                [JsonProperty(PropertyName = "symbol")]
                public string Symbol { get; set; }

                [JsonProperty(PropertyName = "decimals")]
                public ulong Decimals { get; set; }
            }
        }
    }
}