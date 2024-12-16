using UnityEngine;
using TMPro;
using System.IO;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.GamingSdk.Web3Auth;
using UnityEngine.UI;

public class Web3AuthCustomNftManager : MonoBehaviour
{
    #region Fields
        
    [SerializeField] private GameObject customNftPlaceHolder;
    [SerializeField] private GameObject customNftDisplayParent;
    [SerializeField] private GameObject customNftDisplay;
    [SerializeField] private GameObject addCustomNftMenu;
    [SerializeField] private TMP_InputField customNftAddressInput;
    [SerializeField] private TMP_InputField customNftSymbolInput;
    [SerializeField] private TMP_InputField customNftIdInput;
    [SerializeField] private TextMeshProUGUI customNftAmountText;
    [SerializeField] private TextMeshProUGUI customNftSymbolText;
    [SerializeField] private Button addNftsMenuButton;
    [SerializeField] private Button closeAddNftMenuButton;
    [SerializeField] private Button addNftButton;
    private string customNftContract;

    #endregion

    #region Methods

    /// <summary>
    /// Initializes objects.
    /// </summary>
    private void Awake()
    {
        addNftButton.onClick.AddListener(AddNft);
        addNftsMenuButton.onClick.AddListener(ToggleAddNftMenuButton);
        closeAddNftMenuButton.onClick.AddListener(ToggleAddNftMenuButton);
        SetNfts();
    }
    
    /// <summary>
    /// Sets custom nft displays.
    /// </summary>
    private async void SetNfts()
    {
        if (File.Exists(Path.Combine(Application.persistentDataPath, "customNft.txt")))
        {
            var customTokenData = File.ReadAllText(Path.Combine(Application.persistentDataPath, "customNft.txt"));
            var data = customTokenData.Split(",");
            customNftPlaceHolder.SetActive(false);
            customNftContract = data[0];
            customNftSymbolText.text = data[1].ToUpper();
            var balance = await Web3Unity.Web3.Erc1155.GetBalanceOf(customNftContract, data[2]);
            customNftAmountText.text = balance.ToString();
            customNftDisplay.SetActive(true);
        }
        else
        {
            customNftDisplay.SetActive(false);
        }
    }
    
    /// <summary>
    /// Toggles the add nft menu.
    /// </summary>
    private void ToggleAddNftMenuButton()
    {
        addCustomNftMenu.SetActive(!addCustomNftMenu.activeSelf);
    }

    /// <summary>
    /// Adds a custom nft to the wallet.
    /// </summary>
    private void AddNft()
    {
        if (customNftAddressInput.text.Length != 42) return;
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "customNft.txt"), $"{customNftAddressInput.text},{customNftSymbolInput.text},{customNftIdInput.text}");
        customNftSymbolText.text = customNftSymbolInput.text.ToUpper();
        ToggleAddNftMenuButton();
        customNftPlaceHolder.SetActive(false);
        customNftDisplay.SetActive(true);
        customNftAddressInput.text = string.Empty;
        customNftSymbolInput.text = string.Empty;
        customNftIdInput.text = string.Empty;
        SetNfts();
    }

    /// <summary>
    /// Subscribes to events.
    /// </summary>
    private void OnEnable()
    {
        Web3AuthEventManager.SetTokens += SetNfts;
    }

    /// <summary>
    /// Unsubscribes from events.
    /// </summary>
    private void OnDisable()
    {
        Web3AuthEventManager.SetTokens -= SetNfts;
    }

    #endregion
}
