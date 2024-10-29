using System.Threading.Tasks;
using ChainSafe.Gaming.InProcessSigner;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Environment;
using ChainSafe.Gaming.Web3.Evm.Wallet;
using Nethereum.RPC.Accounts;
using Nethereum.Web3.Accounts;

public class AnvilProvider : WalletProvider, IAccountProvider
{
    public IAccount Account { get; private set; }
    
    public AnvilProvider(Web3Environment environment, IChainConfig chainConfig) : base(environment, chainConfig)
    {
    }

    public override Task<string> Connect()
    {
        Account = new Account("0xac0974bec39a17e36ba4a6b4d238ff944bacb478cbed5efcae784d7bf4f2ff80");

        Account.TransactionManager.Client = this;

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
