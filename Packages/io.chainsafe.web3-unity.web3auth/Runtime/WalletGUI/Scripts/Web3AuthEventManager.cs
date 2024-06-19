using System;
using TMPro;
using UnityEngine;

namespace ChainSafe.GamingSdk.Web3Auth
{
    /// <summary>
    /// Web3Auth event manager handles non data sensitive events.
    /// </summary>
    public class Web3AuthEventManager : MonoBehaviour
    {
        #region Events
        
        public static event EventHandler<TxManagerConfigEventArgs> ConfigureTxManager;
        
        public static event EventHandler<GuiManagerConfigEventArgs> ConfigureGuiManager;
        
        public static event EventHandler IncomingTransaction;
        
        public static event EventHandler SetTokens;
        
        public static event EventHandler ToggleWallet;
        
        #endregion

        #region Methods
        
        /// <summary>
        /// Configures TX Manager.
        /// </summary>
        /// <param name="args"></param>
        public static void RaiseConfigureTxManager(TxManagerConfigEventArgs args)
        {
            ConfigureTxManager?.Invoke(null, args);
        }
        
        /// <summary>
        /// Configure GUI manager.
        /// </summary>
        /// <param name="args"></param>
        public static void RaiseConfigureGuiManager(GuiManagerConfigEventArgs args)
        {
            ConfigureGuiManager?.Invoke(null, args);
        }
        
        /// <summary>
        /// Invokes set tokens.
        /// </summary>
        public static void RaiseSetTokens()
        {
            SetTokens?.Invoke(null, EventArgs.Empty);
        }
        
        /// <summary>
        /// Invokes toggle wallet.
        /// </summary>
        public static void RaiseToggleWallet()
        {
            ToggleWallet?.Invoke(null, EventArgs.Empty);
        }
        
        /// <summary>
        /// Invokes incoming transaction.
        /// </summary>
        public static void RaiseIncomingTransaction()
        {
            IncomingTransaction?.Invoke(null, EventArgs.Empty);
        }
        
        #endregion
    }

    #region Configuration Classes
    
    /// <summary>
    /// Configuration class for the Web3Auth Tx Manager.
    /// </summary>
    public class TxManagerConfigEventArgs : EventArgs
    {
        #region Properties

        public bool AutoPopUpWalletOnTx { get; }
        public bool AutoConfirmTransactions { get; }

        public TMP_FontAsset DisplayFont { get; set; }

        public Color SecondaryTextColour { get; set; }

        #endregion

        #region Methods

        public TxManagerConfigEventArgs(bool autoPopUpWalletOnTx, bool autoConfirmTransactions, TMP_FontAsset displayFont, Color secondaryTextColour)
        {
            AutoPopUpWalletOnTx = autoPopUpWalletOnTx;
            AutoConfirmTransactions = autoConfirmTransactions;
            DisplayFont = displayFont;
            SecondaryTextColour = secondaryTextColour;
        }
        
        #endregion
    }
    
    /// <summary>
    /// Configuration class for the Web3Auth GUI manager.
    /// </summary>
    public class GuiManagerConfigEventArgs : EventArgs
    {
        #region Properties
        
        public bool AutoConfirmTransactions { get; }
        public bool DisplayWalletIcon { get; set; }
        public Sprite WalletIcon { get; set; }
        public Sprite WalletLogo { get; set; }
        
        #endregion

        #region Methods
        
        public GuiManagerConfigEventArgs(bool autoConfirmTransactions, bool displayWalletIcon, Sprite walletIcon, Sprite walletLogo)
        {
            AutoConfirmTransactions = autoConfirmTransactions;
            DisplayWalletIcon = displayWalletIcon;
            WalletIcon = walletIcon;
            WalletLogo = walletLogo;
        }
        
        #endregion
    }
    
    #endregion
}