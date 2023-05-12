using Web3Unity.Scripts.Library.Ethers.Providers;

public sealed class RPC
{
    private static RPC instance = null;
    private static JsonRpcProvider provider;

    public static RPC GetInstance
    {
        get
        {
            if (instance == null)
                instance = new RPC();
            return instance;
        }
    }

    private RPC()
    {
        provider = ProviderMigration.NewJsonRpcProvider();
    }

    public JsonRpcProvider Provider()
    {
        return provider;
    }
}
