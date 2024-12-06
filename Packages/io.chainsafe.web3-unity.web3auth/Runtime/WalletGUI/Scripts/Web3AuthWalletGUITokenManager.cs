using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using ChainSafe.GamingSdk.Web3Auth;
using UnityEngine.UI;

/// <summary>
/// Web3Auth waller GUI token manager to handle token displays and logic.
/// </summary>
public class Web3AuthWalletGUITokenManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private GameObject customTokenPlaceHolder;
    [SerializeField] private GameObject customNftPlaceHolder;
    [SerializeField] private GameObject customTokenDisplayErc20Parent;
    [SerializeField] private GameObject customNftDisplayParent;
    [SerializeField] private GameObject customTokenDisplayErc20;
    [SerializeField] private GameObject customNftDisplay;
    [SerializeField] private GameObject transferTokensContainer;
    [SerializeField] private GameObject addCustomTokensMenu;
    [SerializeField] private GameObject addCustomNftMenu;
    [SerializeField] private TMP_Dropdown selectedTokenToTransfer;
    [SerializeField] private TMP_InputField customTokenAddressInput;
    [SerializeField] private TMP_InputField customTokenSymbolInput;
    [SerializeField] private TMP_InputField customNftAddressInput;
    [SerializeField] private TMP_InputField customNftSymbolInput;
    [SerializeField] private TMP_InputField customNftIdInput;
    [SerializeField] private TMP_InputField transferTokensWalletInput;
    [SerializeField] private TMP_InputField transferTokensAmountInput;
    [SerializeField] private TextMeshProUGUI customNftAmountText;
    [SerializeField] private TextMeshProUGUI customNftSymbolText;
    [SerializeField] private TextMeshProUGUI customTokenAmountText;
    [SerializeField] private TextMeshProUGUI customTokenSymbolText;
    [SerializeField] private TextMeshProUGUI nativeTokenSymbolText;
    [SerializeField] private TextMeshProUGUI nativeTokenAmountText;
    [SerializeField] private Button addTokensMenuButton;
    [SerializeField] private Button addNftsMenuButton;
    [SerializeField] private Button closeAddTokensMenuButton;
    [SerializeField] private Button closeAddNftMenuButton;
    [SerializeField] private Button addNftButton;
    [SerializeField] private Button addTokenButton;
    [SerializeField] private Button transferTokensMenuButton;
    [SerializeField] private Button closeTransferTokensButton;
    [SerializeField] private Button transferTokensButton;
    [SerializeField] private Button toggleCustomTokensButton;
    private Task<string> symbolTask;
    private bool isSymbolTaskRunning;
    private string lastCheckedAddress;
    private string customTokenContract;
    private string customNftContract;

    #endregion

    #region Methods

    /// <summary>
    /// Initializes objects.
    /// </summary>
    private void Awake()
    {
        addTokenButton.onClick.AddListener(AddToken);
        addNftButton.onClick.AddListener(AddNft);
        addNftsMenuButton.onClick.AddListener(ToggleAddNftMenuButton);
        closeAddNftMenuButton.onClick.AddListener(ToggleAddNftMenuButton);
        addTokensMenuButton.onClick.AddListener(ToggleAddTokensMenuButton);
        closeAddTokensMenuButton.onClick.AddListener(ToggleAddTokensMenuButton);
        transferTokensMenuButton.onClick.AddListener(ToggleTransferTokensMenuButton);
        closeTransferTokensButton.onClick.AddListener(ToggleTransferTokensMenuButton);
        transferTokensButton.onClick.AddListener(TransferTokens);
        toggleCustomTokensButton.onClick.AddListener(ToggleCustomTokenMenu);
        SetTokens();
        SetNfts();
    }

    /// <summary>
    /// Sets native & custom token displays.
    /// </summary>
    private async void SetTokens()
    {
        if (File.Exists(Path.Combine(Application.persistentDataPath, "customToken.txt")))
        {
            var customTokenData = File.ReadAllText(Path.Combine(Application.persistentDataPath, "customToken.txt"));
            var data = customTokenData.Split(",");
            customTokenPlaceHolder.SetActive(false);
            customTokenContract = data[0];
            customTokenSymbolText.text = data[1].ToUpper();
            var balance = await Web3Unity.Web3.Erc20.GetBalanceOf(customTokenContract, Web3Unity.Web3.Signer.PublicAddress);
            var customTokenValue = (decimal)balance / (decimal)BigInteger.Pow(10, 18);
            customTokenAmountText.text = customTokenValue.ToString("N18");
            customTokenDisplayErc20.SetActive(true);
        }
        else
        {
            customTokenDisplayErc20.SetActive(false);
        }
        // Set native token
        nativeTokenSymbolText.text = Web3Unity.Web3.ChainConfig.NativeCurrency.Symbol.ToUpper();
        var hexBalance = await Web3Unity.Web3.RpcProvider.GetBalance(Web3Unity.Web3.Signer.PublicAddress);
        var weiBalance = BigInteger.Parse(hexBalance.ToString());
        decimal ethBalance = (decimal)weiBalance / (decimal)Math.Pow(10, 18);
        nativeTokenAmountText.text = ethBalance.ToString("0.#########");
        SetTokenDropdownOptions();
    }
    
    /// <summary>
    /// Sets native & custom nft displays.
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
    /// Toggles the add token menu.
    /// </summary>
    private void ToggleAddTokensMenuButton()
    {
        addCustomTokensMenu.SetActive(!addCustomTokensMenu.activeSelf);
    }
    
    /// <summary>
    /// Toggles the add nft menu.
    /// </summary>
    private void ToggleAddNftMenuButton()
    {
        addCustomNftMenu.SetActive(!addCustomNftMenu.activeSelf);
    }

    /// <summary>
    /// Calls the symbol form the contract address so the user doesn't have to enter it.
    /// </summary>
    private async void GetSymbol()
    {
        if (isSymbolTaskRunning) return;
        if (!addCustomTokensMenu.activeSelf || customTokenAddressInput.text == lastCheckedAddress) return;
        if (customTokenAddressInput.text == null || customTokenAddressInput.text.Length != 42) return;
        isSymbolTaskRunning = true;
        lastCheckedAddress = customTokenAddressInput.text;
        try
        {
            symbolTask = Web3Unity.Web3.Erc20.GetSymbol(customTokenAddressInput.text);
            customTokenSymbolInput.text = await symbolTask;
            addTokenButton.gameObject.SetActive(true);
        }
        catch (Web3Exception e)
        {
            Debug.LogError($"Error fetching symbol: {e.Message}");
        }
        finally
        {
            isSymbolTaskRunning = false;
        }
    }

    /// <summary>
    /// Toggles the custom token type menus.
    /// </summary>
    private void ToggleCustomTokenMenu()
    {
        customTokenDisplayErc20Parent.SetActive(!customTokenDisplayErc20Parent.activeSelf);
        customNftDisplayParent.SetActive(!customNftDisplayParent.activeSelf);
    }

    /// <summary>
    /// Adds a custom token to the wallet.
    /// </summary>
    private void AddToken()
    {
        if (customTokenAddressInput.text.Length != 42) return;
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "customToken.txt"), $"{customTokenAddressInput.text},{customTokenSymbolInput.text}");
        customTokenSymbolText.text = customTokenSymbolInput.text.ToUpper();
        ToggleAddTokensMenuButton();
        customTokenPlaceHolder.SetActive(false);
        customTokenDisplayErc20.SetActive(true);
        customTokenAddressInput.text = string.Empty;
        customTokenSymbolInput.text = string.Empty;
        SetTokens();
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
    /// Sets the dropdown options in the token transfer menu.
    /// </summary>
    private void SetTokenDropdownOptions()
    {
        selectedTokenToTransfer.options = new List<TMP_Dropdown.OptionData>();
        var nativeTokenOption = new TMP_Dropdown.OptionData { text = nativeTokenSymbolText.text };
        var customTokenOption = new TMP_Dropdown.OptionData { text = customTokenSymbolText.text };
        selectedTokenToTransfer.options.Add(nativeTokenOption);
        selectedTokenToTransfer.options.Add(customTokenOption);
        selectedTokenToTransfer.RefreshShownValue();
    }

    /// <summary>
    /// Toggles the transfer tokens menu.
    /// </summary>
    private void ToggleTransferTokensMenuButton()
    {
        transferTokensContainer.SetActive(!transferTokensContainer.activeSelf);
    }

    /// <summary>
    /// Transfers tokens to a wallet address.
    /// </summary>
    /// <exception cref="Web3Exception">Exception is thrown if transfer fails</exception>
    private async void TransferTokens()
    {
        // Token transfers
        switch (selectedTokenToTransfer.value)
        {
            case 0:
                BigInteger amountInWei0 = BigInteger.Parse((decimal.Parse(transferTokensAmountInput.text) * (decimal)Math.Pow(10, 18)).ToString("F0"));
                transferTokensContainer.SetActive(false);
                var response0 = await Web3Unity.Instance.SendTransaction(transferTokensWalletInput.text, amountInWei0);
                Debug.Log($"Transfer Complete! {response0}");
                break;
            case 1:
                BigInteger amountInWei1 = BigInteger.Parse((decimal.Parse(transferTokensAmountInput.text) * (decimal)Math.Pow(10, 18)).ToString("F0"));
                transferTokensContainer.SetActive(false);
                var response1 = await Web3Unity.Web3.Erc20.Transfer(customTokenContract, transferTokensWalletInput.text, amountInWei1);
                Debug.Log($"Transfer Complete! {response1}");
                break;
            default:
                throw new Web3Exception("Transfer failed, please check that the token contract exists & you have enough tokens to complete the transfer");
        }
    }

    /// <summary>
    /// Subscribes to events.
    /// </summary>
    private void OnEnable()
    {
        Web3AuthEventManager.SetTokens += SetTokens;
    }

    /// <summary>
    /// Unsubscribes from events.
    /// </summary>
    private void OnDisable()
    {
        Web3AuthEventManager.SetTokens -= SetTokens;
    }

    /// <summary>
    /// Polls for contract address change to get token symbol.
    /// </summary>
    private void Update()
    {
        GetSymbol();
    }

    #endregion
}
