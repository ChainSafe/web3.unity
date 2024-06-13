using System;
using ChainSafe.Gaming.InProcessSigner;
using ChainSafe.Gaming.InProcessTransactionExecutor;
using ChainSafe.Gaming.InProcessTransactionExecutor.Unity;
using ChainSafe.Gaming.UnityPackage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Web3AuthWalletGUI : MonoBehaviour
{

    #region Fields
    
    [SerializeField] private TextMeshProUGUI incomingTxActionText;
    [SerializeField] private TextMeshProUGUI incomingTxHashText;
    [SerializeField] private Button acceptRequestButton;
    [SerializeField] private Button rejectRequestButton;
    private Web3AuthWalletGUITxHistoryManager txHistoryManager;

    #endregion

    #region Methods

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        txHistoryManager = GetComponent<Web3AuthWalletGUITxHistoryManager>();
        acceptRequestButton.onClick.AddListener(AcceptRequest);
        rejectRequestButton.onClick.AddListener(RejectRequest);
    }
    
    public void AcceptRequest()
    {
        //var signer = (InProcessSigner)Web3Accessor.Web3.ServiceProvider.GetService(typeof(InProcessSigner));
        //var executor = (InProcessTransactionExecutor)Web3Accessor.Web3.ServiceProvider.GetService(typeof(InProcessTransactionExecutor));
        
        // TODO update vars below with tx data
        // Get transaction data
        var txTime = DateTime.Now.ToString("hh:mm: tt");
        var txAction = "";
        var txAmount = "";
        var txHash = "";
        
        txHistoryManager.AddTransaction(txTime, txAction, txAmount, txHash);
        txHistoryManager.ResetTransactionDisplay();
    }
    
    private void RejectRequest()
    {
        txHistoryManager.ResetTransactionDisplay();
    }
    
    #endregion
}
