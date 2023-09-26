using System;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.Wallets.WalletConnect.Methods
{
    [Serializable]
    public struct TypedDataRequest<TTypedData>
    {
        public TypedDataRequest(string types, TTypedData message)
        {
            Types = types;

            Message = message;
        }

        [JsonProperty("types")]
        public string Types { get; private set; }

        [JsonProperty("message")]
        public TTypedData Message { get; private set; }
    }
}