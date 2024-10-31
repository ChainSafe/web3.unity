using System.Threading.Tasks;
using ChainSafe.Gaming.InProcessSigner;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Environment;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using Nethereum.RPC.Accounts;
using Nethereum.Web3.Accounts;

namespace ChainSafe.Gaming.Unity.Tests
{
    /// <summary>
    /// Anvil account provider.
    /// </summary>
    public class AnvilProvider : WalletProvider, IAccountProvider
    {
        public IAccount Account { get; private set; }

        public AnvilProvider(Web3Environment environment, IChainConfig chainConfig) : base(environment, chainConfig)
        {
            // Initialize account via private key
            Account = new Account("0xac0974bec39a17e36ba4a6b4d238ff944bacb478cbed5efcae784d7bf4f2ff80");

            Account.TransactionManager.Client = this;
        }

        public override Task<string> Connect()
        {
            return Task.FromResult(Account.Address);
        }

        public override Task Disconnect()
        {
            return Task.CompletedTask;
        }

        public override Task<T> Request<T>(string method, params object[] parameters)
        {
            return Perform<T>(method, parameters);
        }
    }
}
