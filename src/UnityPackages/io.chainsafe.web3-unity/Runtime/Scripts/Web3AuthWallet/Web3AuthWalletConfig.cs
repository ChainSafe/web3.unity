using System;

namespace ChainSafe.GamingSDK.EVM.Web3AuthWallet
{
    [Serializable]
    public class Web3AuthWalletConfig
    {
        public string? PrivateKey { get; set; } = string.Empty;

        public string Host { get; } = "https://api.gaming.chainsafe.io/evm";
    }
}