using ChainSafe.Gaming.EmbeddedWallet;
using Nethereum.Util;
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
            
            timestampText.text = transaction.Timestamp.ToString("HH:mm:ss");
            
            // TODO look up how to get action like transfer and mint
            
            amountText.text = response.Value != null ? transaction.ValueString : string.Empty;
            
            // TODO copy button on hash
            
            hashText.text = response.Hash;
            
            explorerButton.onClick.AddListener(() =>
            {
                Application.OpenURL(transaction.BlockExplorerUrl);
            });
        }
    }
}
