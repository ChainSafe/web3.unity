namespace ChainSafe.Gaming.HyperPlay
{
    /// <summary>
    /// Config for a HyperPlay connection.
    /// </summary>
    public class HyperPlayConfig : IHyperPlayConfig
    {
        public string SignMessageRpcMethodName => "personal_sign";

        public string SignTypedMessageRpcMethodName => "eth_signTypedData_v4";

        /// <summary>
        /// Remember the HyperPlay session.
        /// Like remember me for login.
        /// </summary>
        public bool RememberSession { get; set; }
    }
}