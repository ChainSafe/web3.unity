using Newtonsoft.Json;

namespace ChainSafe.Gaming.Unity.EthereumWindow.Dto
{
    /// <summary>
    /// Native Currency Data for switching and adding new chain to Ethereum Window (browser) wallet.
    /// </summary>
    public struct NativeCurrency
    {
        public NativeCurrency(string name, string symbol, int decimals)
        {
            Name = name;
            Symbol = symbol;
            Decimals = decimals;
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("decimals")]
        public int Decimals { get; set; }
    }
}