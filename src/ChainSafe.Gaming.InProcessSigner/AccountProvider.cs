using Nethereum.Web3.Accounts;

namespace ChainSafe.Gaming.InProcessSigner
{
    public class AccountProvider
    {
        public Account Account { get; private set; } = null!;

        public void Initialize(Account account)
        {
            Account = account;
        }
    }
}