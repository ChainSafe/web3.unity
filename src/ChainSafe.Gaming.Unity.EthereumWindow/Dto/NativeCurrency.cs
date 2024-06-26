using Newtonsoft.Json;

namespace ChainSafe.Gaming.Unity.EthereumWindow.Dto
{
    public struct NativeCurrency
    {
        public NativeCurrency(string symbol)
        {
            Name = symbol;
            Symbol = symbol;
            Decimals = 18;
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("decimals")]
        public int Decimals { get; set; }
    }
}