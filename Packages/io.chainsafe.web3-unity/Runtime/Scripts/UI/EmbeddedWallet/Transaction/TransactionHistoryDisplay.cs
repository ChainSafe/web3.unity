using System.Collections;
using System.Collections.Generic;
using ChainSafe.Gaming.EmbeddedWallet;
using UnityEngine;

namespace ChainSafe.Gaming
{
    public class TransactionHistoryDisplay : MonoBehaviour
    {
        [SerializeField] private Transform container;
        [SerializeField] private TransactionRowDisplay transactionRowPrefab;
        
        public void Initialize(EmbeddedWalletRequestHandler requestHandler)
        {
            requestHandler.RequestConfirmed += RequestConfirmed;
        }

        private void RequestConfirmed(IEmbeddedWalletRequest request)
        {
            switch (request)
            {
                case EmbeddedWalletTransaction transaction:
                    TransactionConfirmed(transaction);
                    break;
            }
        }
        
        private void TransactionConfirmed(EmbeddedWalletTransaction transaction)
        {
            var transactionRow = Instantiate(transactionRowPrefab, container);
            
            // 1 is right below the header row
            transactionRow.transform.SetSiblingIndex(1);
            
            transactionRow.Attach(transaction);
        }
    }
}
