using System;
using System.Collections.Generic;
using ChainSafe.Gaming.Web3.Core.Evm;
using Nethereum.ABI.EIP712;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.WalletConnect.Models
{
    /// <summary>
    /// Typed data model for signing typed data using Wallet Connect JsonRPC, see https://docs.walletconnect.com/advanced/rpc-reference/ethereum-rpc#eth_signtypeddata.
    /// </summary>
    /// <typeparam name="TStruct">Type of Data to be signed.</typeparam>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public struct TypedDataModel<TStruct>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypedDataModel{TStruct}"/> struct.
        /// </summary>
        /// <param name="domain">Domain for Typed Data that's serializable by json.net.</param>
        /// <param name="message">Typed data to be signed.</param>
        public TypedDataModel(SerializableDomain domain, TStruct message)
        {
            Types = MemberDescriptionFactory.GetTypesMemberDescription(typeof(SerializableDomain), typeof(TStruct));

            PrimaryType = typeof(TStruct).Name;

            if (StructAttribute.IsStructType(message))
            {
                PrimaryType = StructAttribute.GetAttribute(message).Name;
            }

            Domain = domain;

            Message = message;
        }

        /// <summary>
        /// Member types for Wallet Connect used in json RPC params.
        /// </summary>
        [JsonProperty("types")]
        public Dictionary<string, MemberDescription[]> Types { get; private set; }

        /// <summary>
        /// Primary type used for Wallet Connect in json RPC params.
        /// </summary>
        [JsonProperty("primaryType")]
        public string PrimaryType { get; private set; }

        /// <summary>
        /// Domain that's serializable by json.net.
        /// </summary>
        [JsonProperty("domain")]
        public SerializableDomain Domain { get; private set; }

        /// <summary>
        /// Data to be signed.
        /// </summary>
        [JsonProperty("message")]
        public TStruct Message { get; private set; }
    }
}