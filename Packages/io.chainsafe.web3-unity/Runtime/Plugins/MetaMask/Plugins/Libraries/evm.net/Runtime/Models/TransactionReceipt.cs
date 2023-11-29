using System.Numerics;
using Newtonsoft.Json;

namespace evm.net.Models
{
    public class TransactionReceipt
    {
        [JsonIgnore]
        internal IProvider _provider;
        
        [JsonProperty("transactionHash")]
        public string TransactionHash { get; private set; }
        
        [JsonProperty("transactionIndex")]
        public BigInteger TransactionIndex { get; private set; }
        
        [JsonProperty("blockHash")]
        public string BlockHash { get; private set; }
        
        [JsonProperty("blockNumber")]
        public BigInteger BlockNumber { get; private set; }
        
        [JsonProperty("from")]
        public string From { get; private set; }
                
        [JsonProperty("to")]
        public string To { get; private set; }
        
        [JsonProperty("cumulativeGasUsed")]
        public BigInteger CumulativeGasUsed { get; private set; }
        
        [JsonProperty("effectiveGasPrice ")]
        public BigInteger EffectiveGasPrice  { get; private set; }
        
        [JsonProperty("gasUsed")]
        public BigInteger GasUsed { get; private set; }
        
        [JsonProperty("contractAddress")]
        public string ContractAddress { get; private set; }
        
        [JsonProperty("logs")]
        public object[] Logs { get; private set; }
        
        [JsonProperty("logsBloom")]
        public string LogBloom { get; private set; }
        
        [JsonProperty("type")]
        public BigInteger Type { get; private set; }
        
        [JsonProperty("status")]
        public BigInteger Status { get; private set; }
        
        [JsonIgnore]
        public Transaction TransactionData { get; internal set; }
    }
}