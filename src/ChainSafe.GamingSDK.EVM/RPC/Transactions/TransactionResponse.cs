using System;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;

namespace Web3Unity.Scripts.Library.Ethers.Transactions
{
    public class TransactionResponse : Transaction
    {
        /// <summary>
        ///     DATA, 32 Bytes - hash of the transaction.
        /// </summary>
        [JsonProperty(PropertyName = "hash")]
        public string Hash { get; set; }

        // /// <summary>
        // ///     QUANTITY - integer of the transactions index position in the block. null when its pending.
        // /// </summary>
        //
        // [JsonProperty(PropertyName = "transactionIndex")]
        // public HexBigInteger TransactionIndex { get; set; }

        /// <summary>
        ///     DATA, 32 Bytes - hash of the block where this transaction was in. null when its pending.
        /// </summary>
        [JsonProperty(PropertyName = "blockHash")]
        public string BlockHash { get; set; }

        /// <summary>
        ///     QUANTITY - block number where this transaction was in. null when its pending.
        /// </summary>
        [JsonProperty(PropertyName = "blockNumber")]
        public HexBigInteger BlockNumber { get; set; }

        /// <summary>
        ///     QUANTITY - timestamp when this transaction was in. null when its pending.
        /// </summary>
        [JsonProperty(PropertyName = "timestamp")]
        public HexBigInteger Timestamp { get; set; }

        // TODO: raw
        public ulong? Confirmations { get; set; }

        public Func<Task<TransactionReceipt>> Wait { get; set; }

        public Func<uint, uint, Task<TransactionReceipt>> WaitParams { get; set; }
    }
}