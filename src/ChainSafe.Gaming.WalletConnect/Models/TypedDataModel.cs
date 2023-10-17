using System;
using System.Collections.Generic;
using ChainSafe.Gaming.Web3.Core.Evm;
using Nethereum.ABI.EIP712;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.WalletConnect.Models
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public struct TypedDataModel<TStruct>
    {
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

        [JsonProperty("types")]
        public Dictionary<string, MemberDescription[]> Types { get; private set; }

        [JsonProperty("primaryType")]
        public string PrimaryType { get; private set; }

        [JsonProperty("domain")]
        public SerializableDomain Domain { get; private set; }

        [JsonProperty("message")]
        public TStruct Message { get; private set; }
    }
}