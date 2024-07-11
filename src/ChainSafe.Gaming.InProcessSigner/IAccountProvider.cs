using Nethereum.RPC.Accounts;

namespace ChainSafe.Gaming.InProcessSigner
{
    /// <summary>
    /// Provides the current connected account.
    /// </summary>
    public interface IAccountProvider
    {
        /// <summary>
        /// Current connected account.
        /// </summary>
        public IAccount Account { get; }
    }
}