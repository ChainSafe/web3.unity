using System;
using System.Collections.Generic;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.GamingSdk.Web3Auth;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Web3Auth wallet GUI main class to manage initialization.
/// </summary>
public class Web3AuthWalletGUI : MonoBehaviour
{
    #region Fields

    [SerializeField] private Text autoConfirmTxLabel;
    [SerializeField] private GameObject walletIconContainer;
    [SerializeField] private List<GameObject> primaryBackgroundObjects;
    [SerializeField] private List<GameObject> menuBackgroundObjects;
    [SerializeField] private List<GameObject> primaryTextObjects;
    [SerializeField] private List<GameObject> secondaryTextObjects;
    [SerializeField] private List<GameObject> displayLineObjects;
    [SerializeField] private List<GameObject> borderButtonObjects;

    #endregion

    #region Properties

    private bool DisplayWalletIcon { get; set; }
    private bool AutoPopUpWalletOnTx { get; set; }
    private bool AutoConfirmTransactions { get; set; }
    private Sprite WalletIcon { get; set; }
    private Sprite WalletLogo { get; set; }
    private Color PrimaryBackgroundColour { get; set; }
    private Color MenuBackgroundColour { get; set; }
    private Color PrimaryTextColour { get; set; }
    private Color SecondaryTextColour { get; set; }
    private Color BorderButtonColour { get; set; }
    private TMP_FontAsset DisplayFont { get; set; }

    #endregion

    #region Methods

    /// <summary>
    /// Method to initialize parameters after prefab creation
    /// </summary>
    /// <param name="config">Web3Auth wallet configuration</param>
    public void Initialize(Web3AuthWalletConfig config)
    {
        DontDestroyOnLoad(gameObject);
        DisplayWalletIcon = config.DisplayWalletIcon;
        AutoPopUpWalletOnTx = config.AutoPopUpWalletOnTx;
        AutoConfirmTransactions = config.AutoConfirmTransactions;
        WalletIcon = config.WalletIcon;
        WalletLogo = config.WalletLogo;
        DisplayFont = config.DisplayFont;
        PrimaryBackgroundColour = config.PrimaryBackgroundColour;
        MenuBackgroundColour = config.MenuBackgroundColour;
        PrimaryTextColour = config.PrimaryTextColour;
        SecondaryTextColour = config.SecondaryTextColour;
        BorderButtonColour = config.BorderButtonColour;
        SetCustomConfig();
    }

    /// <summary>
    /// Sets custom config from the login scene.
    /// </summary>
    private void SetCustomConfig()
    {
        var txConfigArgs = new TxManagerConfigEventArgs(AutoPopUpWalletOnTx, AutoConfirmTransactions, DisplayFont, SecondaryTextColour);
        Web3AuthEventManager.RaiseConfigureTxManager(txConfigArgs);
        var guiConfigArgs = new GuiManagerConfigEventArgs(DisplayWalletIcon, WalletIcon, WalletLogo);
        Web3AuthEventManager.RaiseConfigureGuiManager(guiConfigArgs);
        walletIconContainer.SetActive(DisplayWalletIcon);
        SetCustomColours();
    }

    /// <summary>
    /// Sets custom colours for menu and text objects.
    /// </summary>
    private void SetCustomColours()
    {
        var objectsAndColours = new List<(List<GameObject> objects, Color color)>
        {
            (primaryBackgroundObjects, PrimaryBackgroundColour),
            (menuBackgroundObjects, MenuBackgroundColour),
            (primaryTextObjects, PrimaryTextColour),
            (secondaryTextObjects, SecondaryTextColour)
        };

        foreach (var (objects, colour) in objectsAndColours)
        {
            foreach (var item in objects)
            {
                var imageRenderer = item.GetComponent<Image>();
                if (imageRenderer != null)
                {
                    imageRenderer.color = colour;
                    var imageBorder = item.GetComponent<Outline>();
                    if (imageBorder != null)
                    {
                        imageBorder.effectColor = SecondaryTextColour;
                    }
                }
                var textMeshPro = item.GetComponent<TextMeshProUGUI>();
                if (textMeshPro != null)
                {
                    textMeshPro.font = DisplayFont;
                    textMeshPro.color = colour;
                }
            }
        }
        autoConfirmTxLabel.color = SecondaryTextColour;
        SetButtonsAndLines();
    }

    /// <summary>
    /// Sets border buttons & menu lines.
    /// </summary>
    private void SetButtonsAndLines()
    {
        var objectsAndColours = new List<(List<GameObject> objects, Color color)>
        {
            (borderButtonObjects, BorderButtonColour),
            (displayLineObjects, BorderButtonColour)
        };
        foreach (var (objects, colour) in objectsAndColours)
        {
            foreach (var item in objects)
            {
                var imageRenderer = item.GetComponent<Image>();
                if (imageRenderer != null)
                {
                    imageRenderer.color = colour;
                    var imageBorder = item.GetComponent<Outline>();
                    if (imageBorder != null)
                    {
                        imageBorder.effectColor = SecondaryTextColour;
                    }
                }
            }
        }
    }

    #endregion

    #region ConfigClass

    [Serializable]
    public class Web3AuthWalletConfig
    {
        public bool DisplayWalletIcon;
        public bool AutoPopUpWalletOnTx;
        public bool AutoConfirmTransactions;
        public Sprite WalletIcon;
        public Sprite WalletLogo;
        public TMP_FontAsset DisplayFont;
        public Color PrimaryBackgroundColour;
        public Color MenuBackgroundColour;
        public Color PrimaryTextColour;
        public Color SecondaryTextColour;
        public Color BorderButtonColour;
    }

    #endregion
}
