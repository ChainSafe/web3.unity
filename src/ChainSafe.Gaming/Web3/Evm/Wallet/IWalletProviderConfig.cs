namespace ChainSafe.Gaming.Web3.Evm.Wallet
{
    /// <summary>
    /// Wallet provider config for connecting to a wallet.
    /// </summary>
    public interface IWalletProviderConfig
    {
        /// <summary>
        /// Sign message RPC method name, usually "personal_sign".
        /// </summary>
        public string SignMessageRpcMethodName { get; }

        /// <summary>
        /// Sign Typed message RPC method name, usually "eth_signTypedData".
        /// </summary>
        public string SignTypedMessageRpcMethodName { get; }
    }
}