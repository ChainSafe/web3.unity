namespace ChainSafe.Gaming.Marketplace.Interfaces
{
    /// <summary>
    /// Marketplace auth interface.
    /// </summary>
    public interface IMarketplaceAuth
    {
        void RefreshExpiredToken();
    }
}