using ChainSafe.Gaming.EmbeddedWallet;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChainSafe.Gaming
{
    public class TransactionRowDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timestampText;
        [SerializeField] private TextMeshProUGUI actionText;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private TextMeshProUGUI hashText;
        [SerializeField] private Button explorerButton;
        
        // Attach the transaction to the UI
        public void Attach(EmbeddedWalletTransaction transaction)
        {
            var response = transaction.Response.Task.Result;
            
            timestampText.text = transaction.Timestamp.ToShortTimeString();
            
            // TODO look up how to get action like transfer and mint
            
            amountText.text = response.Value != null ? response.Value.ToString() : response.Data;
            
            hashText.text = response.Hash;
            
            explorerButton.onClick.AddListener(() =>
            {
                // TODO open a block explorer with the transaction hash
            });
        }
    }
}
