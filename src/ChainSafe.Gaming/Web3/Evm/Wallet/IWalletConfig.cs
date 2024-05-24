namespace ChainSafe.Gaming.Web3.Evm.Wallet
{
    public interface IWalletConfig
    {
        public string SignMessageRpcMethodName { get; }

        public string SignTypedMessageRpcMethodName { get; }
    }
}