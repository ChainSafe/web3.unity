using ChainSafe.Gaming.Web3.Evm.Wallet;

namespace ChainSafe.Gaming.HyperPlay
{
    public class HyperPlayConfig : IWalletProviderConfig
    {
        public string SignMessageRpcMethodName => "personal_sign";

        public string SignTypedMessageRpcMethodName => "eth_signTypedData_v3";
    }
}