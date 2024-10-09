using System;
using System.Collections.Generic;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.Evm.Transactions
{
    public class TransactionRequest : ICloneable
    {
        [JsonIgnore]
        public string Id { get; set; }

        /// <summary>
        ///    QUANTITY - The transaction type.
        /// </summary>
        [JsonProperty(PropertyName = "type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public HexBigInteger Type { get; set; }

        /// <summary>
        ///    QUANTITY - The transaction chain id.
        /// </summary>
        [JsonProperty(PropertyName = "chainId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public HexBigInteger ChainId { get; set; }

        /// <summary>
        ///     DATA, 20 Bytes - The address the transaction is send from.
        /// </summary>
        [JsonProperty(PropertyName = "from", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string From { get; set; }

        /// <summary>
        ///     DATA, 20 Bytes - address of the receiver. null when its a contract creation transaction.
        /// </summary>
        [JsonProperty(PropertyName = "to", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string To { get; set; }

        /// <summary>
        ///   QUANTITY - gas provided by the sender.
        /// </summary>
        [JsonProperty(PropertyName = "gas", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public HexBigInteger GasLimit { get; set; }

        /// <summary>
        ///   QUANTITY - gas price provided by the sender in Wei.
        /// </summary>
        [JsonProperty(PropertyName = "gasPrice", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public HexBigInteger GasPrice { get; set; }

        /// <summary>
        ///   QUANTITY - Max Fee Per Gas provided by the sender in Wei.
        /// </summary>
        [JsonProperty(PropertyName = "maxFeePerGas", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public HexBigInteger MaxFeePerGas { get; set; }

        /// <summary>
        ///   QUANTITY - Max Priority Fee Per Gas provided by the sender in Wei.
        /// </summary>
        [JsonProperty(PropertyName = "maxPriorityFeePerGas", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public HexBigInteger MaxPriorityFeePerGas { get; set; }

        /// <summary>
        ///     QUANTITY - value transferred in Wei.
        /// </summary>
        [JsonProperty(PropertyName = "value", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public HexBigInteger Value { get; set; }

        /// <summary>
        ///     DATA - the data send along with the transaction.
        /// </summary>
        [JsonProperty(PropertyName = "data", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Data { get; set; }

        /// <summary>
        ///     QUANTITY - the number of transactions made by the sender prior to this one.
        /// </summary>
        [JsonProperty(PropertyName = "nonce", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public HexBigInteger Nonce { get; set; }

        /// <summary>
        ///   Access list.
        /// </summary>
        [JsonProperty(PropertyName = "accessList", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<AccessList> AccessList { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public TransactionInput ToTransactionInput()
        {
            return new TransactionInput
            {
                From = From,
                To = To,
                Gas = GasLimit,
                GasPrice = GasPrice,
                Value = Value,
                Data = Data,
                Nonce = Nonce,
                AccessList = AccessList,
            };
        }

        public Dictionary<string, object> ToRPCParam()
        {
            var param = new Dictionary<string, object>();

            if (Type != null)
            {
                param.Add("type", Type.HexValue);
            }

            if (ChainId != null)
            {
                param.Add("chainId", ChainId.HexValue);
            }

            if (From != null)
            {
                param.Add("from", From);
            }

            if (To != null)
            {
                param.Add("to", To);
            }

            if (GasLimit != null)
            {
                param.Add("gas", GasLimit.HexValue);
            }

            if (GasPrice != null)
            {
                param.Add("gasPrice", GasPrice.HexValue);
            }

            if (MaxFeePerGas != null)
            {
                param.Add("maxFeePerGas", MaxFeePerGas.HexValue);
            }

            if (MaxPriorityFeePerGas != null)
            {
                param.Add("maxPriorityFeePerGas", MaxPriorityFeePerGas.HexValue);
            }

            if (Value != null)
            {
                param.Add("value", Value.HexValue);
            }

            if (Data != null)
            {
                param.Add("data", Data);
            }

            if (Nonce != null)
            {
                param.Add("nonce", Nonce.HexValue);
            }

            // TODO: AccessList
            /*
            if (AccessList != null)
            {
                param.Add("accessList", AccessList);
            }
            */

            return param;
        }
    }
}