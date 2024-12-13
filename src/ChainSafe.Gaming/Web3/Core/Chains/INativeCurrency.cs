namespace ChainSafe.Gaming.Web3.Core.Chains
{
    public interface INativeCurrency
    {
        string Name { get; set; }

        string Symbol { get; set; }

        int Decimals { get; set; }
    }
}