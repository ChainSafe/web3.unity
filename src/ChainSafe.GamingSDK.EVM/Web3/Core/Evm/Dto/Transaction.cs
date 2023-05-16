using System.Collections.Generic;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json;

namespace Web3Unity.Scripts.Library.Ethers.Transactions
{
    public class Transaction
    {
        /// <summary>
        ///    QUANTITY - The transaction type.
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public HexBigInteger Type { get; set; }

        /// <summary>
        ///    QUANTITY - The transaction chain id.
        /// </summary>
        [JsonProperty(PropertyName = "chainId")]
        public HexBigInteger ChainId { get; set; }

        /// <summary>
        ///     DATA, 20 Bytes - The address the transaction is send from.
        /// </summary>
        [JsonProperty(PropertyName = "from")]
        public string From { get; set; }

        /// <summary>
        ///     DATA, 20 Bytes - address of the receiver. null when its a contract creation transaction.
        /// </summary>
        [JsonProperty(PropertyName = "to")]
        public string To { get; set; }

        /// <summary>
        ///   QUANTITY - gas provided by the sender.
        /// </summary>
        [JsonProperty(PropertyName = "gas")]
        public HexBigInteger GasLimit { get; set; }

        /// <summary>
        ///   QUANTITY - gas price provided by the sender in Wei.
        /// </summary>
        [JsonProperty(PropertyName = "gasPrice")]
        public HexBigInteger GasPrice { get; set; }

        /// <summary>
        ///   QUANTITY - Max Fee Per Gas provided by the sender in Wei.
        /// </summary>
        [JsonProperty(PropertyName = "maxFeePerGas")]
        public HexBigInteger MaxFeePerGas { get; set; }

        /// <summary>
        ///   QUANTITY - Max Priority Fee Per Gas provided by the sender in Wei.
        /// </summary>
        [JsonProperty(PropertyName = "maxPriorityFeePerGas")]
        public HexBigInteger MaxPriorityFeePerGas { get; set; }

        /// <summary>
        ///     QUANTITY - value transferred in Wei.
        /// </summary>
        [JsonProperty(PropertyName = "value")]
        public HexBigInteger Value { get; set; }

        /// <summary>
        ///     DATA - the data send along with the transaction.
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public string Data { get; set; }

        /// <summary>
        ///     QUANTITY - the number of transactions made by the sender prior to this one.
        /// </summary>
        [JsonProperty(PropertyName = "nonce")]
        public HexBigInteger Nonce { get; set; }

        /// <summary>
        ///     QUANTITY - r signature.
        /// </summary>
        [JsonProperty(PropertyName = "r")]
        public string R { get; set; }

        /// <summary>
        ///     QUANTITY - s signature.
        /// </summary>
        [JsonProperty(PropertyName = "s")]
        public string S { get; set; }

        /// <summary>
        ///     QUANTITY - v signature.
        /// </summary>
        [JsonProperty(PropertyName = "v")]
        public string V { get; set; }

        /// <summary>
        ///   Access list.
        /// </summary>
        [JsonProperty(PropertyName = "accessList")]
        public List<AccessList> AccessList { get; set; }
    }
}