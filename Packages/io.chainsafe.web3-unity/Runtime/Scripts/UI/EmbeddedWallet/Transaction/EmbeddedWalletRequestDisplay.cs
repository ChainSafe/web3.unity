using System.Collections.Generic;
using ChainSafe.Gaming.EmbeddedWallet;
using Nethereum.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChainSafe.Gaming
{
    public class EmbeddedWalletRequestDisplay : MonoBehaviour
    {
        [SerializeField] private Transform emptyOverlayContainer;
        
        [SerializeField] private Button approveButton;
        
        [SerializeField] private Button declineButton;
        
        [SerializeField] private TextMeshProUGUI headerText;
        
        [SerializeField] private TextMeshProUGUI messageText;

        private bool _awaitingResponse;
        
        private EmbeddedWalletRequestHandler _requestHandler;
        
        private readonly Queue<IEmbeddedWalletRequest> _requestPool = new Queue<IEmbeddedWalletRequest>();
        
        public void Initialize(EmbeddedWalletRequestHandler requestHandler)
        {
            _requestHandler = requestHandler;
            
            approveButton.onClick.AddListener(TransactionApproved);
            
            declineButton.onClick.AddListener(TransactionDeclined);
        }
        
        public void Request(IEmbeddedWalletRequest request)
        {
            if (_awaitingResponse)
            {
                _requestPool.Enqueue(request);
                
                return;
            }

            switch (request)
            {
                case EmbeddedWalletTransaction transaction:
                    RequestTransaction(transaction);
                    break;
            }
            
            _awaitingResponse = true;
        }

        private void RequestTransaction(EmbeddedWalletTransaction transaction)
        {
            var message = transaction.Request.Value != null ? 
                transaction.ValueString : transaction.Request.Data;
            
            ShowModal("Transaction Request", message);
        }
        
        private void TransactionApproved()
        {
            _requestHandler.Approve();

            FinishRequest();
        }
        
        private void TransactionDeclined()
        {
            _requestHandler.Decline();
            
            FinishRequest();
        }

        private void FinishRequest()
        {
            _awaitingResponse = false;
            
            // Check for awaiting transactions
            if (_requestPool.Count != 0)
            {
                Request(_requestPool.Dequeue());
            }
            else
            {
                HideModal();
            }
        }
        
        private void ShowModal(string header, string message)
        {
            headerText.text = header;
            
            messageText.text = message;
            
            emptyOverlayContainer.gameObject.SetActive(false);
        }
        
        private void HideModal()
        {
            emptyOverlayContainer.gameObject.SetActive(true);
        }
    }
}
