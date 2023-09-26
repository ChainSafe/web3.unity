using System;
using System.Collections.Generic;
using ChainSafe.Gaming.Web3.Core.Evm;
using Nethereum.ABI.EIP712;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.Wallets.WalletConnect.Methods
{
    [Serializable]
    public struct TypedDataRequest<TTypedData>
    {
        public TypedDataRequest(Dictionary<string, MemberDescription[]> types, SerializableDomain domain, TTypedData message)
        {
            Types = types;

            PrimaryType = typeof(TTypedData).Name;

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
        public TTypedData Message { get; private set; }
    }
}