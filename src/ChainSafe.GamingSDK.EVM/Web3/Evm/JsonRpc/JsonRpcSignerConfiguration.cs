using System;

namespace Web3Unity.Scripts.Library.Ethers.Providers
{
    [Serializable]
    public class JsonRpcSignerConfiguration
    {
        public int AccountIndex { get; set; }

        public string AddressOverride { get; set; }
    }
}