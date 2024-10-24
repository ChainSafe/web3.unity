using System.Threading.Tasks;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Environment;
using ChainSafe.Gaming.Web3.Evm.Wallet;

public class AnvilProvider : WalletProvider
{
    public AnvilProvider(Web3Environment environment, IChainConfig chainConfig) : base(environment, chainConfig)
    {
    }

    public override async Task<string> Connect()
    {
        string[] accounts = await Request<string[]>("eth_accounts");
        
        return accounts[0];
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
