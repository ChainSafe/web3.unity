using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine.Networking;
using Web3Unity.Scripts.Library.Ethers.InternalEvents;

namespace Web3Unity.Scripts.Library.Ethers.Network
{
    public class Chains
    {
        private static Dictionary<ulong, Chain> cached = new Dictionary<ulong, Chain>();

        public class Chain
        {
            [JsonProperty(PropertyName = "name")] public string Name { get; set; }

            [JsonProperty(PropertyName = "chain")] public string ChainSymbol { get; set; }

            [JsonProperty(PropertyName = "rpc")] public string[] RPC { get; set; }

            public class NativeCurrency
            {
                [JsonProperty(PropertyName = "chain")] public string Name { get; set; }

                [JsonProperty(PropertyName = "symbol")]
                public string Symbol { get; set; }

                [JsonProperty(PropertyName = "decimals")]
                public ulong Decimals { get; set; }
            }

            [JsonProperty(PropertyName = "nativeCurrency")]
            public NativeCurrency NativeCurrencyInfo { get; set; }

            [JsonProperty(PropertyName = "infoURL")]
            public string InfoUrl { get; set; }

            [JsonProperty(PropertyName = "chainId")]
            public ulong ChainId { get; set; }

            [JsonProperty(PropertyName = "networkId")]
            public ulong NetworkId { get; set; }

            public class ENS
            {
                [JsonProperty(PropertyName = "registry")]
                public string Registry { get; set; }
            }

            [JsonProperty(PropertyName = "ens")] public ENS Ens { get; set; }

            public class Explorer
            {
                [JsonProperty(PropertyName = "name")] public string Name { get; set; }

                [JsonProperty(PropertyName = "url")] public string Url { get; set; }
            }

            [JsonProperty(PropertyName = "explorers")]
            public Explorer[] Explorers { get; set; }
        }

        public static async Task<Dictionary<ulong, Chain>> GetChains()
        {
            if (cached.Count > 0)
            {
                return cached;
            }

            var req = UnityWebRequest.Get("https://chainid.network/chains.json");
            await req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                req.Dispose();
                _captureError(req.error);
                throw new Exception(req.error);
            }

            var reader = new JsonTextReader(new StringReader(req.downloadHandler.text));
            var serializer = JsonSerializer.Create();
            var chains = serializer.Deserialize<Chain[]>(reader);

            req.Dispose();

            if (chains == null)
            {
                _captureError("failed to deserialize chains");
                throw new Exception("failed to deserialize chains");
            }

            foreach (var chain in chains)
            {
                cached[chain.ChainId] = chain;
            }

            return cached;
        }

        private static void _captureError(string error)
        {
            var properties = new Dictionary<string, object>
            {
                {"error", error}
            };

            DataHog.Client.Capture("Chains", properties);
        }
    }
}