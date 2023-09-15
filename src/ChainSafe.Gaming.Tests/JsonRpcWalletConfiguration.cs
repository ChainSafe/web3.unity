using System;

namespace Chainsafe.Gaming.Wallets
{
    [Serializable]
    public class JsonRpcWalletConfiguration
    {
        public int AccountIndex { get; set; }

        public string AddressOverride { get; set; }
    }
}