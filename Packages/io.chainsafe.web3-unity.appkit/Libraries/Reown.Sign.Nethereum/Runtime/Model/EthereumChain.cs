using System;
using System.Numerics;
using Newtonsoft.Json;
using Reown.Core.Common.Utils;

namespace Reown.Sign.Nethereum.Model
{
    [Serializable]
    public class EthereumChain
    {
        [JsonProperty("chainId")]
        public string chainIdHex;

        [JsonProperty("chainName")]
        public string name;

        [JsonProperty("nativeCurrency")]
        public Currency nativeCurrency;

        [JsonProperty("rpcUrls")]
        public string[] rpcUrls;

        [JsonProperty("blockExplorerUrls", NullValueHandling = NullValueHandling.Ignore)]
        public string[] blockExplorerUrls;

        [JsonIgnore]
        public string chainIdDecimal;

        [Preserve]
        public EthereumChain()
        {
        }

        public EthereumChain(string chainIdDecimal, string name, in Currency nativeCurrency, string[] rpcUrls, string[] blockExplorerUrls = null)
        {
            this.chainIdDecimal = chainIdDecimal;
            chainIdHex = BigInteger.Parse(chainIdDecimal).ToHex(true);
            this.name = name;
            this.nativeCurrency = nativeCurrency;
            this.rpcUrls = rpcUrls;
            this.blockExplorerUrls = blockExplorerUrls;
        }
    }

    [Serializable]
    public readonly struct Currency
    {
        public readonly string name;
        public readonly string symbol;
        public readonly int decimals;

        public Currency(string name, string symbol, int decimals)
        {
            this.name = name;
            this.symbol = symbol;
            this.decimals = decimals;
        }
    }
}