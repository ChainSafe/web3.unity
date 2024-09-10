using System;
using TMPro;
using UnityEngine;

namespace ChainSafe.GamingSdk.Web3Auth
{
    /// <summary>
    /// Web3Auth event manager handles non data sensitive events.
    /// </summary>
    public static class Web3AuthEventManager
    {
        #region Events

        public static event EventHandler<TxManagerConfigEventArgs> ConfigureTxManager;

        public static event EventHandler<GuiManagerConfigEventArgs> ConfigureGuiManager;

        public static event Action SetTokens;

        public static event Action ToggleWallet;

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
            SetTokens?.Invoke();
        }

        /// <summary>
        /// Invokes toggle wallet.
        /// </summary>
        public static void RaiseToggleWallet()
        {
            ToggleWallet?.Invoke();
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

        public bool AutoPopUpWalletOnTx { get; private set; }
        public bool AutoConfirmTransactions { get; private set; }

        public TMP_FontAsset DisplayFont { get; private set; }

        public Color SecondaryTextColour { get; private set; }

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

        public bool DisplayWalletIcon { get; private set; }
        public Sprite WalletIcon { get; private set; }
        public Sprite WalletLogo { get; private set; }

        #endregion

        #region Methods

        public GuiManagerConfigEventArgs(bool displayWalletIcon, Sprite walletIcon, Sprite walletLogo)
        {
            DisplayWalletIcon = displayWalletIcon;
            WalletIcon = walletIcon;
            WalletLogo = walletLogo;
        }

        #endregion
    }

    #endregion
}