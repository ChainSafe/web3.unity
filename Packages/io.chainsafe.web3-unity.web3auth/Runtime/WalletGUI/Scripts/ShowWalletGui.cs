using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage.Connection;
using ChainSafe.Gaming.Web3;
using TMPro;
using UnityEngine;

namespace ChainSafe.GamingSdk.Web3Auth
{
    public class ShowWalletGui : MonoBehaviour, IWeb3InitializedHandler
    {
        [SerializeField] private Web3AuthWalletGUI web3AuthWalletGUIPrefab;
        [SerializeField] private bool displayWalletIcon;
        [SerializeField] private bool autoConfirmTransactions;
        [SerializeField] private bool autoPopUpWalletOnTx;
        [SerializeField] private Sprite walletIcon;
        [SerializeField] private Sprite walletLogo;
        [SerializeField] public TMP_FontAsset displayFont;
        [SerializeField] private Color primaryBackgroundColour;
        [SerializeField] private Color menuBackgroundColour;
        [SerializeField] private Color primaryTextColour;
        [SerializeField] private Color secondaryTextColour;
        [SerializeField] private Color borderButtonColour;
        
        public Task OnWeb3Initialized(Web3 web3)
        {
            var w3AWalletGuiConfig = new Web3AuthWalletGUI.Web3AuthWalletConfig
            {
                DisplayWalletIcon = displayWalletIcon,
                AutoPopUpWalletOnTx = autoPopUpWalletOnTx,
                AutoConfirmTransactions = autoConfirmTransactions,
                WalletIcon = walletIcon,
                WalletLogo = walletLogo,
                DisplayFont = displayFont,
                PrimaryBackgroundColour = primaryBackgroundColour,
                MenuBackgroundColour = menuBackgroundColour,
                PrimaryTextColour = primaryTextColour,
                SecondaryTextColour = secondaryTextColour,
                BorderButtonColour = borderButtonColour
            };
            
            // TODO pass web3 instance here instead of using web3accessor
            var web3AuthWalletInstance = Instantiate(web3AuthWalletGUIPrefab);
            web3AuthWalletInstance.Initialize(w3AWalletGuiConfig);

            return Task.CompletedTask;
        }
    }
}