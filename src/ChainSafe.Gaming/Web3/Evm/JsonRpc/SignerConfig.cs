using System;

namespace ChainSafe.Gaming.Web3.Evm.JsonRpc
{
    [Serializable]
    public class SignerConfig
    {
        public int AccountIndex { get; set; }

        public string AddressOverride { get; set; }
    }
}