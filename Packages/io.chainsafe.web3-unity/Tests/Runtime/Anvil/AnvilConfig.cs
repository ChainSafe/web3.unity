using ChainSafe.Gaming.Web3.Evm.Wallet;

public class AnvilConfig : IWalletProviderConfig
{
    public string SignMessageRpcMethodName => "eth_sign";

    public string SignTypedMessageRpcMethodName => "eth_signTypedData_v4";
}
