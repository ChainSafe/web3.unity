using ChainSafe.Gaming.EmbeddedWallet;
using Microsoft.Extensions.DependencyInjection;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using CWeb3 = ChainSafe.Gaming.Web3.Web3;

namespace ChainSafe.Gaming
{
    public class EmbeddedWalletScreen : MonoBehaviour
    {
        [SerializeField] private Button showButton;
        
        [SerializeField] private Button hideButton;
        
        [SerializeField] private Transform contentContainer;
        
        [SerializeField] private Image requestPendingIcon;
        
        [Space]
        
        [SerializeField] private TextMeshProUGUI addressText;
        
        [SerializeField] private Button copyAddressButton;
        
        private IEmbeddedWalletConfig _config;
        
        private EmbeddedWalletRequestHandler _requestHandler;
        
        private EmbeddedWalletRequestDisplay _embeddedWalletRequest;
        
        private TransactionHistoryDisplay _transactionHistory;
        
        public void Initialize(CWeb3 web3)
        {
            showButton.onClick.AddListener(Show);
            
            hideButton.onClick.AddListener(Hide);

            addressText.text = web3.Signer.PublicAddress;
            
            copyAddressButton.onClick.AddListener(CopyAddress);
            
            // Transaction
            _embeddedWalletRequest = GetComponentInChildren<EmbeddedWalletRequestDisplay>(true);
            
            _transactionHistory = GetComponentInChildren<TransactionHistoryDisplay>(true);
            
            _config = web3.ServiceProvider.GetService<IEmbeddedWalletConfig>();
            
            _requestHandler = web3.ServiceProvider.GetService<EmbeddedWalletRequestHandler>();

            _requestHandler.RequestQueued += Request;
            
            _embeddedWalletRequest.Initialize(_requestHandler);
            
            _transactionHistory.Initialize(_requestHandler);
        }

        private void CopyAddress()
        {
            ClipboardManager.CopyText(addressText.text);
        }

        private void Request(IEmbeddedWalletRequest request)
        {
            Show();

            // requestPendingIcon.enabled = true;
            
            _embeddedWalletRequest.Request(request);
        }

        private void OnConfirmation()
        {
            
        }

        private void Show()
        {
            contentContainer.gameObject.SetActive(true);
            
            showButton.gameObject.SetActive(false);
        }
        
        private void Hide()
        {
            contentContainer.gameObject.SetActive(false);
            
            showButton.gameObject.SetActive(true);
        }

        private void PendingRequest()
        {
            
        }
    }
}
