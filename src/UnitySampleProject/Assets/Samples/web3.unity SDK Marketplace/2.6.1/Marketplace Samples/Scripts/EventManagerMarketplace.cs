using System;
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
        
        public static event EventHandler<MarketplaceAuthSystemManagerConfigEventArgs> ConfigureAuthSystemManager;
        public static event EventHandler<MarketplaceGUIConfigEventArgs> ConfigureMarketplaceGuiManager;
        public static event EventHandler<MarketplaceBrowserConfigEventArgs> ConfigureMarketplaceBrowserManager;
        public static event EventHandler<CollectionBrowserConfigEventArgs> ConfigureCollectionBrowserManager;
        public static event EventHandler<MarketplaceCreateConfigEventArgs> ConfigureMarketplaceCreateManager;
        public static event EventHandler<CollectionCreateConfigEventArgs> ConfigureCollectionCreateManager;
        public static event Action ToggleMarketplacesMenu;
        public static event Action ToggleCreateMarketplaceMenu;
        public static event Action ToggleCollectionsMenu;
        public static event Action ToggleCreateCollectionMenu;
        public static event Action ToggleSelectionMenu;
        public static event Action ToggleMintNftToCollectionMenu;
        public static event Action UploadMarketplaceImage;
        public static event Action UploadCollectionImage;
        public static event Action UploadNftImageToCollection;
        public static event Action ListNftToMarketplace;
        public static event Action ToggleListNftToMarketplaceMenu;
        
        #endregion
        
        #region Methods
        
        /// <summary>
        /// Invokes ToggleMarketplacesMenu.
        /// </summary>
        public static void RaiseToggleMarketplacesMenu()
        {
            ToggleMarketplacesMenu?.Invoke();
        }
        
        /// <summary>
        /// Invokes ToggleCreateMarketplaceMenu.
        /// </summary>
        public static void RaiseToggleCreateMarketplaceMenu()
        {
            ToggleCreateMarketplaceMenu?.Invoke();
        }
        
        /// <summary>
        /// Invokes ToggleCollectionsMenu.
        /// </summary>
        public static void RaiseToggleCollectionsMenu()
        {
            ToggleCollectionsMenu?.Invoke();
        }
        
        /// <summary>
        /// Invokes ToggleCreateCollectionMenu.
        /// </summary>
        public static void RaiseToggleCreateCollectionMenu()
        {
            ToggleCreateCollectionMenu?.Invoke();
        }
        
        /// <summary>
        /// Invokes ToggleMintNftToSelectionMenu.
        /// </summary>
        public static void RaiseToggleMintNftToSelectionMenu()
        {
            ToggleMintNftToCollectionMenu?.Invoke();
        }
        
        /// <summary>
        /// Invokes ToggleSelectionMenu.
        /// </summary>
        public static void RaiseToggleSelectionMenu()
        {
            ToggleSelectionMenu?.Invoke();
        }
        
        /// <summary>
        /// Invokes UploadMarketplaceImage.
        /// </summary>
        public static void RaiseUploadMarketplaceImage()
        {
            UploadMarketplaceImage?.Invoke();
        }
        
        /// <summary>
        /// Invokes UploadCollectionImage.
        /// </summary>
        public static void RaiseUploadCollectionImage()
        {
            UploadCollectionImage?.Invoke();
        }
        
        /// <summary>
        /// Invokes UploadNftToCollectionImage.
        /// </summary>
        public static void RaiseUploadNftImageToCollection()
        {
            UploadNftImageToCollection?.Invoke();
        }

        /// <summary>
        /// Invokes ToggleListNftToMarketplaceMenu.
        /// </summary>
        public static void RaiseToggleListNftToMarketplaceMenu()
        {
            ToggleListNftToMarketplaceMenu?.Invoke();
        }
        
        /// <summary>
        /// Invokes ListNftToMarketplace.
        /// </summary>
        public static void RaiseListNftToMarketplace()
        {
            ListNftToMarketplace?.Invoke();
        }
        
        /// <summary>
        /// Configure auth system manager.
        /// </summary>
        /// <param name="args">Input args.</param>
        public static void RaiseConfigureAuthSystemManager(MarketplaceAuthSystemManagerConfigEventArgs args)
        {
            ConfigureAuthSystemManager?.Invoke(null, args);
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
        /// Configuration class for the Marketplace Auth System Manager.
        /// </summary>
        public class MarketplaceAuthSystemManagerConfigEventArgs : EventArgs
        {
            #region Properties
            
            public string BearerToken { get; private set; }

            public DateTime BearerTokenExpires { get; private set; }
            
            public string RefreshToken { get; private set; }

            public DateTime RefreshTokenExpires { get; set; }

            #endregion
    
            #region Methods
    
            public MarketplaceAuthSystemManagerConfigEventArgs(string bearerToken, DateTime bearerTokenExpires, string refreshToken, DateTime refreshTokenExpires)
            {
                BearerToken = bearerToken;
                BearerTokenExpires = bearerTokenExpires;
                RefreshToken = refreshToken;
                RefreshTokenExpires = refreshTokenExpires;
            }
            
            #endregion
        }
            
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
            
            public TMP_FontAsset DisplayFont { get; private set; }
            public Color SecondaryTextColour { get; private set; }
            public string BearerToken { get; private set; }
    
            #endregion
    
            #region Methods
    
            public CollectionBrowserConfigEventArgs(TMP_FontAsset displayFont, Color secondaryTextColour, string bearerToken)
            {
                DisplayFont = displayFont;
                SecondaryTextColour = secondaryTextColour;
                BearerToken = bearerToken;
            }
            
            #endregion
        }
        
        /// <summary>
        /// Configuration class for the Marketplace Create Manager.
        /// </summary>
        public class MarketplaceCreateConfigEventArgs : EventArgs
        {
            #region Properties
            
            public string BearerToken { get; private set; }
    
            #endregion
    
            #region Methods
    
            public MarketplaceCreateConfigEventArgs(string bearerToken)
            {
                BearerToken = bearerToken;
            }
            
            #endregion
        }
        
        /// <summary>
        /// Configuration class for the Marketplace Create Manager.
        /// </summary>
        public class CollectionCreateConfigEventArgs : EventArgs
        {
            #region Properties
            
            public string BearerToken { get; private set; }
    
            #endregion
    
            #region Methods
    
            public CollectionCreateConfigEventArgs(string bearerToken)
            {
                BearerToken = bearerToken;
            }
            
            #endregion
        }
        
        #endregion
    }
}
