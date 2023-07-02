namespace Prefabs.Web3AuthWallet.Interfaces
{
    public interface ITransactionConfig
    {
        string GetProjectId();

        string GetChain();

        string GetNetwork();

        string GetRpc();
    }
}