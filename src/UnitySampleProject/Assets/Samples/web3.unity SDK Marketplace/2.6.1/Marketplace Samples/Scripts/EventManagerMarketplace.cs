using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace ChainSafe.Gaming.Marketplace
{
    /// <summary>
    /// Manages marketplace events.
    /// </summary>
    public static class EventManagerMarketplace
    {
        #region Events
        public static event EventHandler<MarketplaceGUIConfigEventArgs> ConfigureMarketplaceGuiManager;
        public static event EventHandler<MarketplaceBrowserConfigEventArgs> ConfigureMarketplaceBrowserManager;
        public static event EventHandler<CollectionBrowserConfigEventArgs> ConfigureCollectionBrowserManager;
        public static event EventHandler<MarketplaceCreateConfigEventArgs> ConfigureMarketplaceCreateManager;
        public static event EventHandler<CollectionCreateConfigEventArgs> ConfigureCollectionCreateManager;
        public static event EventHandler<ListNftToMarketplaceConfigEventArgs> ConfigureListNftToMarketplaceManager;
        public static event EventHandler<ListNftToMarketplaceTxEventArgs> ConfigureListNftToMarketplaceTxManager;
        public static event Action ToggleSelectionMenu;
        public static event Action ToggleCollectionsMenu;
        public static event Action ToggleCreateCollectionMenu;
        public static event Action ToggleMintNftToCollectionMenu;
        public static event Action ToggleSelectedCollection;
        public static event Action UploadCollectionImage;
        public static event Action UploadNftImageToCollection;
        public static event Action ToggleMarketplacesMenu;
        public static event Action ToggleCreateMarketplaceMenu;
        public static event Action ToggleListNftToMarketplaceMenu;
        public static event Action OpenSelectedMarketplace;
        public static event Action CloseSelectedMarketplace;
        public static event Action OpenSelectedCollection;
        public static event Action CloseSelectedCollection;
        public static event Action UploadMarketplaceImage;
        public static event Action ListNftToMarketplace;
        public static event Action CreateMarketplace;
        public static event Action LogoutMarketplace;

        #endregion

        #region Methods

        public static void RaiseLoginMarketplace()
        {
            ToggleMarketplacesMenu?.Invoke();
        }
        public static void RaiseLogoutMarketplace()
        {
            ResetBearerTokens();
            LogoutMarketplace?.Invoke();
        }

        /// <summary>
        /// Configure auth system manager.
        /// </summary>
        /// <param name="args">Input args.</param>
        public static void RaiseConfigureAuthSystemManager(MarketplaceAuthSystemConfigEventArgs args)
        {
            ConfigureAuthSystemManager?.Invoke(null, args);
        }
        
        /// <summary>
        /// Configure list nft to marketplace manager.
        /// </summary>
        /// <param name="args">Input args.</param>
        public static void RaiseConfigureListNftToMarketplaceManager(ListNftToMarketplaceConfigEventArgs args)
        {
            ConfigureListNftToMarketplaceManager?.Invoke(null, args);
        }
        
        /// <summary>
        /// Configure list nft to marketplace tx manager.
        /// </summary>
        /// <param name="args">Input args.</param>
        public static void RaiseListNftToMarketplaceTxManager(ListNftToMarketplaceTxEventArgs args)
        {
            ConfigureListNftToMarketplaceTxManager?.Invoke(null, args);
        }
        
        /// <summary>
        /// Configure GUI manager.
        /// </summary>
        /// <param name="args">Input args.</param>
        public static void RaiseConfigureMarketplaceGuiManager(MarketplaceGUIConfigEventArgs args)
        {
            ConfigureMarketplaceGuiManager?.Invoke(null, args);
        }

        /// <summary>
        /// Configure marketplace browser manager.
        /// </summary>
        /// <param name="args">Input args.</param>
        public static void RaiseConfigureMarketplaceBrowserManager(MarketplaceBrowserConfigEventArgs args)
        {
            ConfigureMarketplaceBrowserManager?.Invoke(null, args);
        }

        /// <summary>
        /// Configure collection browser manager.
        /// </summary>
        /// <param name="args">Input args.</param>
        public static void RaiseConfigureCollectionBrowserManager(CollectionBrowserConfigEventArgs args)
        {
            ConfigureCollectionBrowserManager?.Invoke(null, args);
        }
        
        /// <summary>
        /// Configure marketplace create manager.
        /// </summary>
        /// <param name="args">Input args.</param>
        public static void RaiseConfigureMarketplaceCreateManager(MarketplaceCreateConfigEventArgs args)
        {
            ConfigureMarketplaceCreateManager?.Invoke(null, args);
        }
        
        /// <summary>
        /// Configure collection create manager.
        /// </summary>
        /// <param name="args">Input args.</param>
        public static void RaiseConfigureCollectionCreateManager(CollectionCreateConfigEventArgs args)
        {
            ConfigureCollectionCreateManager?.Invoke(null, args);
        }

        #endregion

        #region Configuration Classes

        /// <summary>
        /// Configuration class for the Marketplace GUI Manager.
        /// </summary>
        public class MarketplaceGUIConfigEventArgs : EventArgs
        {
            #region Properties

            public static TMP_FontAsset DisplayFont { get; private set; }
            public static Color PrimaryBackgroundColour { get; private set; }
            public static Color MenuBackgroundColour { get; private set; }
            public static Color PrimaryTextColour { get; private set; }
            public static Color SecondaryTextColour { get; private set; }
            public static Color BorderButtonColour { get; private set; }

            #endregion

            #region Methods

            public MarketplaceGUIConfigEventArgs(TMP_FontAsset displayFont, Color primaryBackgroundColour, Color menuBackgroundColour, Color primaryTextColour, Color secondaryTextColour, Color borderButtonColour)
            {
                DisplayFont = displayFont;
                PrimaryBackgroundColour = primaryBackgroundColour;
                MenuBackgroundColour = menuBackgroundColour;
                PrimaryTextColour = primaryTextColour;
                SecondaryTextColour = secondaryTextColour;
                BorderButtonColour = borderButtonColour;
            }

            #endregion
        }

        /// <summary>
        /// Configuration class for the Marketplace Browser Manager.
        /// </summary>
        public class MarketplaceBrowserConfigEventArgs : EventArgs
        {
            #region Properties

            public TMP_FontAsset DisplayFont { get; private set; }
            public Color SecondaryTextColour { get; private set; }
            public string BearerToken { get; private set; }

            #endregion

            #region Methods

            public MarketplaceBrowserConfigEventArgs(TMP_FontAsset displayFont, Color secondaryTextColour, string bearerToken)
            {
                DisplayFont = displayFont;
                SecondaryTextColour = secondaryTextColour;
                BearerToken = bearerToken;
            }

            #endregion
        }

        /// <summary>
        /// Configuration class for the Marketplace Browser Manager.
        /// </summary>
        public class CollectionBrowserConfigEventArgs : EventArgs
        {
            #region Properties

            private string BearerToken { get; set; }

            private DateTime BearerTokenExpires { get; set; }

            private string RefreshToken { get; set; }

            private DateTime RefreshTokenExpires { get; set; }

            #endregion

            #region Methods

            public MarketplaceAuthSystemManagerConfigEventArgs(string bearerToken, DateTime bearerTokenExpires, string refreshToken, DateTime refreshTokenExpires)
            {
                BearerToken = bearerToken;
            }

            #endregion
        }

        #endregion
    }
}
