using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ChainSafe.GamingWeb3.Environment;
using Newtonsoft.Json;

namespace Web3Unity.Scripts.Library.Ethers.Network
{
    public class Chains
    {
        private static Dictionary<ulong, Chain> cached = new Dictionary<ulong, Chain>();

        public static async Task<Dictionary<ulong, Chain>> GetChains(Web3Environment environment)
        {
            if (cached.Count > 0)
            {
                return cached;
            }

            var response = await environment.HttpClient.Get<Chain[]>("https://chainid.network/chains.json");

            if (!response.IsSuccess)
            {
                CaptureError(response.Error, environment);
                throw new HttpRequestException(response.Error);
            }

            var chains = response.Response;

            if (chains == null)
            {
                CaptureError("failed to deserialize chains", environment);
                throw new Exception("failed to deserialize chains");
            }

            foreach (var chain in chains)
            {
                cached[chain.ChainId] = chain;
            }

            return cached;
        }

        private static void CaptureError(string error, Web3Environment environment)
        {
            var properties = new Dictionary<string, object>
            {
                { "error", error },
            };

            environment.AnalyticsClient.CaptureEvent("Chains", properties);
        }

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