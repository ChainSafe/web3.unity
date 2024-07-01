using System;

namespace ChainSafe.Gaming.Marketplace
{
    /// <summary>
    /// Marketplace auth interface.
    /// </summary>
    public interface IMarketplaceAuth
    {
        string BearerToken { get; }
        DateTime BearerTokenExpires { get; }
        string RefreshToken { get; }
        DateTime RefreshTokenExpires { get; }
        
        void InitializeConfig(object sender, EventManagerMarketplace.MarketplaceAuthSystemConfigEventArgs marketplaceAuthSystemManagerConfigEventArgs);
    }
}