namespace ChainSafe.Gaming.Web3.Evm.Wallet
{
    public interface IWalletProviderConfig
    {
        public string SignMessageRpcMethodName { get; }

        public string SignTypedMessageRpcMethodName { get; }
    }
}