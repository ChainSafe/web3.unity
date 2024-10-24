using ChainSafe.Gaming.Web3.Evm.Wallet;

public class AnvilConfig : IWalletProviderConfig
{
    public string SignMessageRpcMethodName => "personal_sign";

    public string SignTypedMessageRpcMethodName => "eth_signTypedData_v4";
}
