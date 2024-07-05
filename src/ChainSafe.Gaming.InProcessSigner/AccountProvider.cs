using Nethereum.Web3.Accounts;

namespace ChainSafe.Gaming.InProcessSigner
{
    /// <summary>
    /// Provides the current connected account.
    /// </summary>
    public class AccountProvider
    {
        /// <summary>
        /// Current connected account.
        /// </summary>
        public Account Account { get; private set; } = null!;

        /// <summary>
        /// Initialized provider with the current connected account.
        /// </summary>
        /// <param name="account">Current connected account.</param>
        public void Initialize(Account account)
        {
            Account = account;
        }
    }
}