using ChainSafe.Gaming.Web3.Evm.Wallet;

namespace ChainSafe.Gaming.HyperPlay
{
    /// <summary>
    /// Config for a HyperPlay connection.
    /// </summary>
    public interface IHyperPlayConfig : IWalletProviderConfig
    {
        /// <summary>
        /// Url for connecting to HyperPlay desktop client.
        /// </summary>
        public string Url => "http://localhost:9680/rpc";

        /// <summary>
        /// Remember a connected session.
        /// </summary>
        public bool RememberSession { get; }
    }
}