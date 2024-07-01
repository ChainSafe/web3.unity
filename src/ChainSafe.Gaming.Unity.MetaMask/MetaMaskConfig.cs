using ChainSafe.Gaming.Web3.Evm.Wallet;

namespace ChainSafe.Gaming.Unity.MetaMask
{
    /// <summary>
    /// Concrete implementation of <see cref="IWalletProviderConfig"/> for connecting to MetaMask wallet.
    /// </summary>
    public class MetaMaskConfig : IWalletProviderConfig
    {
        public string SignMessageRpcMethodName => "personal_sign";

        public string SignTypedMessageRpcMethodName => "eth_signTypedData_v4";
    }
}