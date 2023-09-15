using System;

namespace ChainSafe.Gaming.Debugging
{
    [Serializable]
    public class JsonRpcWalletConfig
    {
        public int AccountIndex { get; set; }

        public string AddressOverride { get; set; }
    }
}