namespace evm.net
{
    public interface ILegacyProvider
    {
        long ChainId { get; }
        
        string ConnectedAddress { get; }

        object Request(string method, object[] parameters = null);
    }
}