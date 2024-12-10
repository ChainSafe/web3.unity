using System;
using Newtonsoft.Json;

namespace Reown.AppKit.Unity.WebGl.Wagmi
{
    [Serializable]
    public class GetBalanceParameter
    {
        public string address;
    }
    
    [Serializable]
    public class GetBalanceReturnType
    {
        public string value;
    }
    
    [Serializable]
    public class SignMessageParameter
    {
        public string message;
    }

    [Serializable]
    public class VerifyMessageParameters
    {
        public string address;
        public string message;
        public string signature;
    }

    [Serializable]
    public class VerifyTypedDataParameters
    {
        public string address;
        public object message;
        public string signature;
    }

    [Serializable]
    public class GetAccountReturnType
    {
        public string address;
        public string[] addresses;
        public string chainId;
        public bool isConnecting;
        public bool isReconnecting;
        public bool isConnected;
        public bool isDisconnected;
        public string status;
    }

    [Serializable]
    public class SwitchChainParameter
    {
        public int chainId;
        public AddEthereumChainParameter parameter;
    }

    [Serializable]
    public class AddEthereumChainParameter
    {
        public string chainId;
        public string chainName;
        public NativeCurrency nativeCurrency;
        public string[] rpcUrls;
        public string[] blockExplorerUrls;
        public string[] iconUrls;
    }

    [Serializable]
    public class NativeCurrency
    {
        public string name;
        public string symbol;
        public int decimals;
    }

    [Serializable]
    public class SendTransactionParameter
    {
        public string to;
        public string value;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string data;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string gas;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string gasPrice;
    }

    [Serializable]
    public class ReadContractParameter
    {
        public AbiItem[] abi;
        public string address;
        public string functionName;
        public object[] args;
    }
    
    [Serializable]
    public class WriteContractParameter
    {
        public AbiItem[] abi;
        public string address;
        public string functionName;
        public object[] args;
        public string value;
        public string gas;
    }

    [Serializable]
    public struct AbiItem
    {
        public string type;
        public string name;
        public string stateMutability;
        public AbiParam[] inputs;
        public AbiParam[] outputs;
    }

    [Serializable]
    public struct AbiParam
    {
        public string name;
        public string type;
    }
    
    [Serializable]
    public class EstimateGasParameter
    {
        public string to;
        public string value;
        public string data;
    }
}