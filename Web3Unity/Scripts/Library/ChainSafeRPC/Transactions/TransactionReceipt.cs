using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Web3Unity.Scripts.Library.Ethers.Transactions
{
    public class TransactionReceipt
    {
        /// <summary>
        ///     DATA, 32 Bytes - hash of the transaction.
        /// </summary>
        [JsonProperty(PropertyName = "transactionHash")]
        public string TransactionHash { get; set; }

        /// <summary>
        ///     QUANTITY - integer of the transactions index position in the block.
        /// </summary>
        [JsonProperty(PropertyName = "transactionIndex")]
        public HexBigInteger TransactionIndex { get; set; }

        /// <summary>
        ///     DATA, 32 Bytes - hash of the block where this transaction was in.
        /// </summary>
        [JsonProperty(PropertyName = "blockHash")]
        public string BlockHash { get; set; }

        /// <summary>
        ///     QUANTITY - block number where this transaction was in.
        /// </summary>
        [JsonProperty(PropertyName = "blockNumber")]
        public HexBigInteger BlockNumber { get; set; }

        /// <summary>
        ///     DATA, 20 Bytes - From address
        /// </summary>
        [JsonProperty(PropertyName = "from")]
        public string From { get; set; }

        /// <summary>
        ///     DATA, 20 Bytes - To address
        /// </summary>
        [JsonProperty(PropertyName = "to")]
        public string To { get; set; }

        /// <summary>
        ///     QUANTITY - The total amount of gas used when this transaction was executed in the block.
        /// </summary>
        [JsonProperty(PropertyName = "cumulativeGasUsed")]
        public HexBigInteger CumulativeGasUsed { get; set; }

        /// <summary>
        ///     QUANTITY - The amount of gas used by this specific transaction alone.
        /// </summary>
        [JsonProperty(PropertyName = "gasUsed")]
        public HexBigInteger GasUsed { get; set; }

        /// <summary>
        /// The actual value per gas deducted from the senders account. Before EIP-1559, this is equal to the transaction's gas price. After, it is equal to baseFeePerGas + min(maxFeePerGas - baseFeePerGas, maxPriorityFeePerGas). Legacy transactions and EIP-2930 transactions are coerced into the EIP-1559 format by setting both maxFeePerGas and maxPriorityFeePerGas as the transaction's gas price.
        /// </summary>
        [JsonProperty(PropertyName = "effectiveGasPrice")]
        public HexBigInteger EffectiveGasPrice { get; set; }

        /// <summary>
        ///     DATA, 20 Bytes - The contract address created, if the transaction was a contract creation, otherwise null.
        /// </summary>
        [JsonProperty(PropertyName = "contractAddress")]
        public string ContractAddress { get; set; }

        /// <summary>
        ///     QUANTITY / BOOLEAN Transaction Success 1, Transaction Failed 0
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public HexBigInteger Status { get; set; }

        /// <summary>
        ///     logs: Array - Array of log objects, which this transaction generated.
        /// </summary>
        [JsonProperty(PropertyName = "logs")]
        public JArray Logs { get; set; }

        /// <summary>
        ///    QUANTITY - The transaction type.
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public HexBigInteger Type { get; set; }

        /// <summary>
        ///     DATA, 256 Bytes - Bloom filter for light clients to quickly retrieve related logs
        /// </summary>
        [JsonProperty(PropertyName = "logsBloom")]
        public string LogsBloom { get; set; }

        /// <summary>
        ///  DATA, 32 Bytes The post-transaction state root. Only specified for transactions included before the Byzantium upgrade. (DEPRECATED)
        /// </summary>
        [JsonProperty(PropertyName = "root")]
        public string Root { get; set; }

        public ulong? Confirmations { get; set; }
    }
}