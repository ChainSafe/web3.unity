using System.Numerics;
using Newtonsoft.Json;

namespace ChainSafe.Gaming
{
    public struct CustomToken
    {
        [JsonProperty("address")]
        public string Address { get; private set; }

        [JsonProperty("symbol")]
        public string Symbol { get; private set; }

        [JsonIgnore]
        public BigInteger Balance { get; private set; }
        
        [JsonIgnore]
        public BigInteger Decimals { get; private set; }
        
        public CustomToken(string address, string symbol, BigInteger decimals)
        {
            Address = address;

            Symbol = symbol;

            Balance = 0;

            Decimals = decimals;
        }

        public void SetBalance(BigInteger balance)
        {
            Balance = balance;
        }
    }
}