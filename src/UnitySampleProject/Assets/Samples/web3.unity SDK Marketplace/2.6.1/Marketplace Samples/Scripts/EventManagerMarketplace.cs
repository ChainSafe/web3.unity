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
        
        public static event EventHandler<MarketplaceAuthSystemConfigEventArgs> ConfigureAuthSystemManager;
        public static event EventHandler<MarketplaceGUIConfigEventArgs> ConfigureMarketplaceGuiManager;
        public static event EventHandler<MarketplaceBrowserConfigEventArgs> ConfigureMarketplaceBrowserManager;
        public static event EventHandler<CollectionBrowserConfigEventArgs> ConfigureCollectionBrowserManager;
        public static event EventHandler<MarketplaceCreateConfigEventArgs> ConfigureMarketplaceCreateManager;
        public static event EventHandler<CollectionCreateConfigEventArgs> ConfigureCollectionCreateManager;
        public static event EventHandler<MintCollectionNftConfigEventArgs> ConfigureMintCollectionNftManager;
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
        public static event Action MintNftToCollection;
        public static event Action CreateMarketplace;
        public static event Action LogoutMarketplace;
        
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
        /// Invokes ToggleMintNftToCollectionMenu.
        /// </summary>
        public static void RaiseToggleMintNftToCollectionMenu()
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
        /// Invokes OpenSelectedCollection.
        /// </summary>
        public static void RaiseOpenSelectedCollection()
        {
            OpenSelectedCollection?.Invoke();
        }
        
        /// <summary>
        /// Invokes CloseSelectedCollection.
        /// </summary>
        public static void RaiseCloseSelectedCollection()
        {
            CloseSelectedCollection?.Invoke();
        }
        
        /// <summary>
        /// Invokes OpenSelectedMarketplace.
        /// </summary>
        public static void RaiseOpenSelectedMarketplace()
        {
            OpenSelectedMarketplace?.Invoke();
        }
        
        /// <summary>
        /// Invokes CloseSelectedMarketplace.
        /// </summary>
        public static void RaiseCloseSelectedMarketplace()
        {
            CloseSelectedMarketplace?.Invoke();
        }
        
        public static void RaiseMintNftToCollection()
        {
            MintNftToCollection?.Invoke();
        }
        
        /// <summary>
        /// Invokes ListNftToMarketplace.
        /// </summary>
        public static void RaiseListNftToMarketplace()
        {
            ListNftToMarketplace?.Invoke();
        }
        
        /// <summary>
        /// Closes the selected collection.
        /// </summary>
        public static void RaiseToggleSelectedCollection()
        {
            ToggleSelectedCollection?.Invoke();
        }
        
        /// <summary>
        /// Invokes create marketplace.
        /// </summary>
        public static void RaiseCreateMarketplace()
        {
            CreateMarketplace?.Invoke();
        }
        
        /// <summary>
        /// Logs the user out of the marketplace.
        /// </summary>
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
        
        /// <summary>
        /// Configure marketplace create manager.
        /// </summary>
        /// <param name="args">Input args.</param>
        public static void RaiseMintCollectionNftManager(MintCollectionNftConfigEventArgs args)
        {
            ConfigureMintCollectionNftManager?.Invoke(null, args);
        }
        
        #endregion
        
        /// <summary>
        /// Resets all bearer tokens.
        /// </summary>
        private static void ResetBearerTokens()
        {
            var authEventArgs = new MarketplaceAuthSystemConfigEventArgs(string.Empty, DateTime.Now, string.Empty, DateTime.Now);
            RaiseConfigureAuthSystemManager(authEventArgs);

            var marketplaceBrowserEventArgs = new MarketplaceBrowserConfigEventArgs(MarketplaceGUIConfigEventArgs.DisplayFont, MarketplaceGUIConfigEventArgs.SecondaryTextColour, string.Empty);
            RaiseConfigureMarketplaceBrowserManager(marketplaceBrowserEventArgs);

            var collectionBrowserEventArgs = new CollectionBrowserConfigEventArgs(MarketplaceGUIConfigEventArgs.DisplayFont, MarketplaceGUIConfigEventArgs.SecondaryTextColour, string.Empty);
            RaiseConfigureCollectionBrowserManager(collectionBrowserEventArgs);

            var marketplaceCreateEventArgs = new MarketplaceCreateConfigEventArgs(string.Empty);
            RaiseConfigureMarketplaceCreateManager(marketplaceCreateEventArgs);

            var collectionCreateEventArgs = new CollectionCreateConfigEventArgs(string.Empty);
            RaiseConfigureCollectionCreateManager(collectionCreateEventArgs);
            
            var listNftToMarketplaceCreateEventArgs = new ListNftToMarketplaceConfigEventArgs(string.Empty);
            RaiseConfigureListNftToMarketplaceManager(listNftToMarketplaceCreateEventArgs);
            
            var mintCollectionNftConfigEventArgs = new MintCollectionNftConfigEventArgs(string.Empty, string.Empty, string.Empty);
            RaiseMintCollectionNftManager(mintCollectionNftConfigEventArgs);
        }
        
        #region Configuration Classes
        
        /// <summary>
        /// Configuration class for the Marketplace Auth System Manager.
        /// </summary>
        public class MarketplaceAuthSystemConfigEventArgs : EventArgs
        {
            #region Properties
            
            public string BearerToken { get; private set; }

            public DateTime BearerTokenExpires { get; private set; }
            
            public string RefreshToken { get; private set; }

            public DateTime RefreshTokenExpires { get; set; }

            #endregion
    
            #region Methods
    
            public MarketplaceAuthSystemConfigEventArgs(string bearerToken, DateTime bearerTokenExpires, string refreshToken, DateTime refreshTokenExpires)
            {
                BearerToken = bearerToken;
                BearerTokenExpires = bearerTokenExpires;
                RefreshToken = refreshToken;
                RefreshTokenExpires = refreshTokenExpires;
            }
            
            #endregion
        }
        
        /// <summary>
        /// Transaction value class for the List Nft To Marketplace Manager.
        /// </summary>
        public class ListNftToMarketplaceTxEventArgs : EventArgs
        {
            #region Properties
            
            [CanBeNull] public string CollectionContractToListFrom { get; set; }
            [CanBeNull] public string MarketplaceContractToListTo { get; set; }
            [CanBeNull] public string TokenIdToList { get; set; }
            [CanBeNull] public string Price { get; set; }
            [CanBeNull] public string NftType { get; set; }

            #endregion
    
            #region Methods
            
            /// <summary>
            /// Transaction value class for the List Nft To Marketplace Manager.
            /// </summary>
            public ListNftToMarketplaceTxEventArgs([CanBeNull] string collectionContractToListFrom, [CanBeNull] string marketplaceContractToListTo, [CanBeNull] string tokenIdToList, [CanBeNull] string price, [CanBeNull] string nftType)
            {
                if (collectionContractToListFrom != null)
                {
                    CollectionContractToListFrom = collectionContractToListFrom;
                }

                if (marketplaceContractToListTo != null)
                {
                    MarketplaceContractToListTo = marketplaceContractToListTo;
                }

                if (tokenIdToList != null)
                {
                    TokenIdToList = tokenIdToList;
                }

                if (price != null)
                {
                    Price = price;
                }

                if (nftType != null)
                {
                    NftType = nftType;
                }
            }
            
            #endregion
        }
        
        /// <summary>
        /// Configuration class for the List Nft To Marketplace Manager.
        /// </summary>
        public class ListNftToMarketplaceConfigEventArgs : EventArgs
        {
            #region Properties
            
            public string BearerToken { get; set; }

            #endregion
    
            #region Methods
            
            /// <summary>
            /// Configuration class for the List Nft To Marketplace Manager.
            /// </summary>
            public ListNftToMarketplaceConfigEventArgs(string bearerToken)
            {
                BearerToken = bearerToken;
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
        
        public class MintCollectionNftConfigEventArgs : EventArgs
        {
            #region Properties
            
            public string BearerToken { get; private set; }
            public string CollectionContractToListFrom { get; private set; }
            public string CollectionTypeToListFrom { get; private set; }

            #endregion
    
            #region Methods
    
            public MintCollectionNftConfigEventArgs(string bearerToken, string collectionToListFrom, string collectionTypeToListFrom)
            {
                if (bearerToken != null)
                {
                    BearerToken = bearerToken;
                }
                CollectionContractToListFrom = collectionToListFrom;
                CollectionTypeToListFrom = collectionTypeToListFrom;
            }
            
            #endregion
        }
        
        #endregion
    }
}
