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
        
        public static event EventHandler<MarketplaceGUIConfigEventArgs> ConfigureMarketplaceGuiManager;
        public static event Action LoginMarketplace;
        public static event Action LogoutMarketplace;
        
        #endregion
        
        #region Methods
        
        public static void RaiseLoginMarketplace()
        {
            LoginMarketplace?.Invoke();
        }
        
        public static void RaiseLogoutMarketplace()
        {
            LogoutMarketplace?.Invoke();
        }
        
        /// <summary>
        /// Configure GUI manager.
        /// </summary>
        /// <param name="args"></param>
        public static void RaiseConfigureMarketplaceGuiManager(MarketplaceGUIConfigEventArgs args)
        {
            ConfigureMarketplaceGuiManager?.Invoke(null, args);
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
        
        #endregion
    }
}
