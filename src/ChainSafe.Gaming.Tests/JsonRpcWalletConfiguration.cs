using System;

<<<<<<<< HEAD:src/ChainSafe.Gaming.Debugging/JsonRpcWalletConfig.cs
namespace ChainSafe.Gaming.Debugging
========
namespace ChainSafe.Gaming.Wallets
>>>>>>>> main:src/ChainSafe.Gaming.Tests/JsonRpcWalletConfiguration.cs
{
    [Serializable]
    public class JsonRpcWalletConfig
    {
        public int AccountIndex { get; set; }

        public string AddressOverride { get; set; }
    }
}