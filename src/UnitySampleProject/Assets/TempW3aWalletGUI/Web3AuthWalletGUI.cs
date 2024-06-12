using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Web3AuthWalletGUI : MonoBehaviour
{

    #region Fields
    
    [SerializeField] private TextMeshProUGUI walletAddressText;
    [SerializeField] private TextMeshProUGUI privateKeyText;
    [SerializeField] private TextMeshProUGUI incomingTxActionText;
    [SerializeField] private TextMeshProUGUI incomingTxHashText;
    [SerializeField] private TextMeshProUGUI customTokenAmountText;
    [SerializeField] private TextMeshProUGUI customTokenSymbolText;
    [SerializeField] private TMP_InputField customTokenAddressInput;
    [SerializeField] private TMP_InputField customTokenSymbolInput;
    [SerializeField] private TextMeshProUGUI nativeTokenAmountText;
    [SerializeField] private TextMeshProUGUI nativeTokenSymbolText;
    [SerializeField] private TMP_InputField transferTokensWalletInput;
    [SerializeField] private TMP_InputField transferTokensAmountInput;
    [SerializeField] private TextMeshProUGUI txHistoryTimeText;
    [SerializeField] private TextMeshProUGUI txHistoryActionText;
    [SerializeField] private TextMeshProUGUI txHistoryAmountText;
    [SerializeField] private TextMeshProUGUI txHistoryTxHashText;
    [SerializeField] private Button txHistoryOpenBlockExplorerButton;
    [SerializeField] private Button openWalletButton;
    [SerializeField] private Button closeWalletButton;
    [SerializeField] private Button copyWalletAddressButton;
    [SerializeField] private Button openPrivateKeyMenuButton;
    [SerializeField] private Button closePrivateKeyMenuButton;
    [SerializeField] private Button copyPrivateKeyButton;
    [SerializeField] private Button addTokensMenuButton;
    [SerializeField] private Button addTokenButton;
    [SerializeField] private Button closeAddTokensMenuButton;
    [SerializeField] private Button transferTokensMenuButton;
    [SerializeField] private Button transferTokensButton;
    [SerializeField] private Button closeTransferTokensButton;
    [SerializeField] private Button acceptRequestButton;
    [SerializeField] private Button rejectRequestButton;
    [SerializeField] private GameObject openWalletGUIContainer;
    [SerializeField] private GameObject walletGUIContainer;
    [SerializeField] private GameObject privateKeyContainer;
    [SerializeField] private GameObject transferTokensContainer;
    [SerializeField] private GameObject addCustomTokensContainer;
    [SerializeField] private GameObject customTokenPlaceHolder;
    [SerializeField] private GameObject customTokenDisplay;
    [SerializeField] private GameObject incomingTxPlaceHolder;
    [SerializeField] private GameObject incomingTxDisplay;
    [SerializeField] private GameObject txHistoryPlaceHolder;
    [SerializeField] private GameObject txHistoryDisplay;

    #endregion

    #region Methods

    private void Awake()
    {
        openWalletButton.onClick.AddListener(ToggleWalletButton);
        closeWalletButton.onClick.AddListener(ToggleWalletButton);
        copyWalletAddressButton.onClick.AddListener(CopyWalletAddressButton);
        openPrivateKeyMenuButton.onClick.AddListener(TogglePrivateKeyMenuButton);
        closePrivateKeyMenuButton.onClick.AddListener(TogglePrivateKeyMenuButton);
        copyPrivateKeyButton.onClick.AddListener(CopyPrivateKeyButton);
        addTokensMenuButton.onClick.AddListener(ToggleAddTokensMenuButton);
        closeAddTokensMenuButton.onClick.AddListener(ToggleAddTokensMenuButton);
        addTokenButton.onClick.AddListener(AddTokenButton);
        transferTokensMenuButton.onClick.AddListener(ToggleTransferTokensMenuButton);
        closeTransferTokensButton.onClick.AddListener(ToggleTransferTokensMenuButton);
        transferTokensButton.onClick.AddListener(TransferTokens);
        acceptRequestButton.onClick.AddListener(AcceptRequest);
        rejectRequestButton.onClick.AddListener(RejectRequest);
        txHistoryOpenBlockExplorerButton.onClick.AddListener(OpenBlockExplorer);
    }

    private void ToggleWalletButton()
    {
        if (walletGUIContainer.activeSelf)
        {
            walletGUIContainer.SetActive(false);
            openWalletGUIContainer.SetActive(true);
        }
        else
        {
            walletGUIContainer.SetActive(true);
            openWalletGUIContainer.SetActive(false);
        }
    }

    private void CopyWalletAddressButton()
    {
        GUIUtility.systemCopyBuffer = walletAddressText.text;
    }
    
    private void TogglePrivateKeyMenuButton()
    {
        privateKeyContainer.SetActive(!privateKeyContainer.activeSelf);
    }

    private void CopyPrivateKeyButton()
    {
        GUIUtility.systemCopyBuffer = privateKeyText.text;
    }
    
    private void ToggleAddTokensMenuButton()
    {
        addCustomTokensContainer.SetActive(!addCustomTokensContainer.activeSelf);
    }
    
    private void AddTokenButton()
    {
        // TODO
    }
    
    private void ToggleTransferTokensMenuButton()
    {
        transferTokensContainer.SetActive(!transferTokensContainer.activeSelf);
    }
    
    private void TransferTokens()
    {
        // TODO
    }
    
    private void AcceptRequest()
    {
        // TODO
    }
    
    private void RejectRequest()
    {
        // TODO
    }
    
    private void OpenBlockExplorer()
    {
        Application.OpenURL($"https://sepolia.etherscan.io/tx/{txHistoryTxHashText.text}");
    }
    
    private void Update()
    {
        // Check for shift + tab press to allow opening and closing of wallet GUI
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleWalletButton();
        }
    }
    
    #endregion
}
