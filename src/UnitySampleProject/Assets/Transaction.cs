using System.Collections.Generic;
using System.Numerics;
using Newtonsoft.Json;
using UnityEngine.Scripting;
using WalletConnectSharp.Common.Utils;
using WalletConnectSharp.Network.Models;

public class Transaction
{
    [JsonProperty("from")]
    public string From { get; set; }
        
    [JsonProperty("to")]
    public string To { get; set; }
        
    [JsonProperty("gas", NullValueHandling = NullValueHandling.Ignore)]
    public string Gas { get; set; }
        
    [JsonProperty("gasPrice", NullValueHandling = NullValueHandling.Ignore)]
    public string GasPrice { get; set; }
        
    [JsonProperty("value")]
    public string Value { get; set; }

    [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
    public string Data { get; set; } = "0x";
}

[RpcMethod("eth_sendTransaction"), RpcRequestOptions(Clock.ONE_MINUTE, 99997)]
public class EthSendTransaction : List<Transaction>
{
    public EthSendTransaction(params Transaction[] transactions) : base(transactions)
    {
    }

    [Preserve] //Needed for JSON.NET serialization
    public EthSendTransaction()
    {
    }
}

[RpcMethod("personal_sign"), RpcRequestOptions(Clock.ONE_MINUTE, 99998)]
public class EthSignMessage : List<string>
{
    public EthSignMessage(string message, string address) : base(new string[]{message, address})
    {
        
    }

    [Preserve] //Needed for JSON.NET serialization
    public EthSignMessage()
    {
        
    }
}
