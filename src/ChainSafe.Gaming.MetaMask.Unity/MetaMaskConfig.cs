using ChainSafe.Gaming.Web3.Evm.Wallet;

namespace ChainSafe.Gaming.MetaMask.Unity
{
    public class MetaMaskConfig : IWalletProviderConfig
    {
        public string SignMessageRpcMethodName => "personal_sign";

        public string SignTypedMessageRpcMethodName => "eth_signTypedData_v4";
    }
}