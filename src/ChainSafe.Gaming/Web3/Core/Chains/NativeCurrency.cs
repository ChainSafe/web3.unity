using ChainSafe.Gaming.Web3.Core.Chains;

namespace ChainSafe.Gaming.Web3
{
    /// <summary>
    /// This class is necessary in order to properly create a request when adding the new network to reown.
    /// </summary>
    [System.Serializable]
    public class NativeCurrency : INativeCurrency
    {
        public string Name { get; set; }

        public string Symbol { get; set; }

        public int Decimals { get; set; }
    }
}