using ChainSafe.Gaming.Web3.Evm.Wallet;

namespace ChainSafe.Gaming.Unity.Tests
{
    /// <summary>
    /// Just a stub config used for Foundry-Anvil
    /// </summary>
    public class AnvilConfig : IWalletProviderConfig
    {
        // Not personal_sign because Foundry-Anvil doesn't support that RPC method as listed here https://book.getfoundry.sh/reference/anvil/#supported-rpc-methods
        public string SignMessageRpcMethodName => "eth_sign";

        public string SignTypedMessageRpcMethodName => "eth_signTypedData_v4";
    }
}
