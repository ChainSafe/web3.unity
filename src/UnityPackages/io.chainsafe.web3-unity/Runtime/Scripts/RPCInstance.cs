using Web3Unity.Scripts.Library.Ethers.Providers;

public sealed class RPC
{
    private static RPC instance = null;
    private static JsonRpcProvider provider;

    public static RPC GetInstance
    {
        get
        {
            instance ??= new RPC();
            return instance;
        }
    }

    private RPC()
    {
        provider = ProviderMigration.NewJsonRpcProviderAsync().Result;
    }

    public JsonRpcProvider Provider()
    {
        return provider;
    }
}
