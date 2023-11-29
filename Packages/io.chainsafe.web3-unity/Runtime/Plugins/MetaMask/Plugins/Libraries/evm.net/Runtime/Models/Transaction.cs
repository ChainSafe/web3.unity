using System;
using System.Numerics;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace evm.net.Models
{
    public class Transaction
    {
        [JsonIgnore]
        internal IProvider _provider;
        
        [JsonProperty("blockHash")]
        public string BlockHash { get; private set; }
        
        [JsonProperty("blockNumber")]
        public BigInteger BlockNumber { get; private set; }
        
        [JsonProperty("from")]
        public string From { get; private set; }
        
        [JsonProperty("gas")]
        public BigInteger Gas { get; private set; }
        
        [JsonProperty("gasPrice")]
        public BigInteger GasPrice { get; private set; }
        
        [JsonProperty("hash")]
        public string Hash { get; private set; }
        
        [JsonProperty("input")]
        public string Input { get; private set; }
        
        [JsonProperty("nonce")]
        public BigInteger Nonce { get; private set; }
        
        [JsonProperty("to")]
        public string To { get; private set; }
        
        [JsonProperty("transactionIndex")]
        public BigInteger TransactionIndex { get; private set; }
        
        [JsonProperty("value")]
        public BigInteger Value { get; private set; }
        
        [JsonProperty("v")]
        public BigInteger V { get; private set; }
        
        [JsonProperty("r")]
        public BigInteger R { get; private set; }
        
        [JsonProperty("s")]
        public BigInteger S { get; private set; }

        public static async Task<Transaction> FromHash(string hash, IProvider provider)
        {
            object[] requestParameters = new object[]
            {
                hash.StartsWith("0x") ? hash : $"0x{hash}"
            };
            
            try
            {
                // try to use generic function
                var receipt = await provider.Request<Transaction>("eth_getTransactionReceipt", requestParameters);

                receipt._provider = provider;
                return receipt;
            }
            catch (NotImplementedException e)
            {
                // try with legacy provider
                var rawResult = provider.Request("eth_getTransactionReceipt", requestParameters);

                // Legacy MetaMask SDK provider
                if (rawResult.GetType() == typeof(Task<JsonElement>))
                {
                    var receiptTask = (Task<JsonElement>)rawResult;
                    var receipt = await receiptTask;

                    return receipt.Deserialize<Transaction>();
                }

                return (Transaction)rawResult;
            }
        }

        public async Task<TransactionReceipt> FetchReceipt()
        {
            object[] requestParameters = new object[]
            {
                Hash.StartsWith("0x") ? Hash : $"0x{Hash}"
            };
            
            try
            {
                // try to use generic function
                var receipt = await _provider.Request<TransactionReceipt>("eth_getTransactionReceipt", requestParameters);

                receipt._provider = _provider;
                receipt.TransactionData = this;
                return receipt;
            }
            catch (NotImplementedException e)
            {
                // try with legacy provider
                var rawResult = _provider.Request("eth_getTransactionReceipt", requestParameters);
                
                // Legacy MetaMask SDK provider
                if (rawResult.GetType() == typeof(Task<JsonElement>))
                {
                    var receiptTask = (Task<JsonElement>)rawResult;
                    var receipt = await receiptTask;

                    return receipt.Deserialize<TransactionReceipt>();
                }

                return (TransactionReceipt)rawResult;
            }
        }
    }
}