using System;

namespace ChainSafe.Gaming.Wallets
{
    [Serializable]
    public class JsonRpcWalletConfig
    {
        public int AccountIndex { get; set; }

        public string AddressOverride { get; set; }
    }
}