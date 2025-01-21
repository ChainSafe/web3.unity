using ChainSafe.Gaming.EmbeddedWallet;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;
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
        
        private IEmbeddedWalletConfig _config;
        
        private EmbeddedWalletRequestHandler _requestHandler;
        
        private EmbeddedWalletRequestDisplay _embeddedWalletRequest;
        
        private TransactionHistoryDisplay _transactionHistory;
        
        public void Initialize(CWeb3 web3)
        {
            // Transaction
            _embeddedWalletRequest = GetComponentInChildren<EmbeddedWalletRequestDisplay>(true);
            
            _transactionHistory = GetComponentInChildren<TransactionHistoryDisplay>(true);
            
            _config = web3.ServiceProvider.GetService<IEmbeddedWalletConfig>();
            
            _requestHandler = web3.ServiceProvider.GetService<EmbeddedWalletRequestHandler>();

            _requestHandler.RequestQueued += Request;
            
            _embeddedWalletRequest.Initialize(_requestHandler);
            
            _transactionHistory.Initialize(_requestHandler);
            
            showButton.onClick.AddListener(Show);
            
            hideButton.onClick.AddListener(Hide);
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
