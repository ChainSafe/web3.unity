using System;
using Newtonsoft.Json;

namespace Reown.AppKit.Unity.Model.BlockchainApi
{
    [Serializable]
    public sealed class GetIdentityResponse
    {
        public string Name { get; }
        public string Avatar { get; }
        
        [JsonConstructor]
        public GetIdentityResponse(string name, string avatar)
        {
            Name = name;
            Avatar = avatar;
        }
    }

    [Serializable]
    public sealed class GetBalanceResponse
    {
        public Balance[] Balances { get; }
        
        [JsonConstructor]
        public GetBalanceResponse(Balance[] balances)
        {
            Balances = balances;
        }
    }

    [Serializable]
    public struct Balance
    {
        [JsonProperty("name")]
        public string name;
        
        [JsonProperty("symbol")]
        public string symbol;
        
        [JsonProperty("chainId")]
        public string chainId;
        [JsonProperty("address")]
        public string address;
        
        [JsonProperty("value")]
        public string value;
        
        [JsonProperty("price")]
        public string price;
        
        [JsonProperty("quantity")]
        public Quantity quantity;
        
        [JsonProperty("iconUrl")]
        public string iconUrl;
    }
    
    [Serializable]
    public struct Quantity
    {
        [JsonProperty("decimals")]
        public string decimals;
        
        [JsonProperty("numeric")]
        public string numeric;
    }
}