using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Numerics;
using ChainSafe.Gaming.Evm.Contracts.BuiltIn;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using Scripts.EVM.Token;
using UnityEngine.UI;

public class Web3AuthWalletGUITokenManager : MonoBehaviour
{
    #region Fields
    
    [SerializeField] private GameObject customTokenPlaceHolder;
    [SerializeField] private GameObject customTokenDisplay;
    [SerializeField] private GameObject transferTokensContainer;
    [SerializeField] private GameObject addCustomTokensContainer;
    [SerializeField] private TMP_Dropdown selectedTokenToTransfer;
    [SerializeField] private TMP_InputField customTokenAddressInput;
    [SerializeField] private TMP_InputField customTokenSymbolInput;
    [SerializeField] private TMP_InputField transferTokensWalletInput;
    [SerializeField] private TMP_InputField transferTokensAmountInput;
    [SerializeField] private TextMeshProUGUI customTokenAmountText;
    [SerializeField] private TextMeshProUGUI customTokenSymbolText;
    [SerializeField] private TextMeshProUGUI nativeTokenSymbolText;
    [SerializeField] private TextMeshProUGUI nativeTokenAmountText;
    [SerializeField] private Button addTokensMenuButton;
    [SerializeField] private Button closeAddTokensMenuButton;
    [SerializeField] private Button addTokenButton;
    [SerializeField] private Button transferTokensMenuButton;
    [SerializeField] private Button closeTransferTokensButton;
    [SerializeField] private Button transferTokensButton;
    private string customTokenContract;
    
    #endregion

    #region Methods
    

    private void Awake()
    {
        addTokenButton.onClick.AddListener(AddToken);
        addTokensMenuButton.onClick.AddListener(ToggleAddTokensMenuButton);
        closeAddTokensMenuButton.onClick.AddListener(ToggleAddTokensMenuButton);
        transferTokensMenuButton.onClick.AddListener(ToggleTransferTokensMenuButton);
        closeTransferTokensButton.onClick.AddListener(ToggleTransferTokensMenuButton);
        transferTokensButton.onClick.AddListener(TransferTokens);
        SetTokens();
    }

    private void SetTokens()
    {
        if (File.Exists(Path.Combine(Application.persistentDataPath, "customToken.txt")))
        {
            var customTokenData = File.ReadAllText(Path.Combine(Application.persistentDataPath, "customToken.txt"));
            var data = customTokenData.Split(",");
            customTokenContract = data[0];
            customTokenSymbolText.text = data[1].ToUpper();
            customTokenPlaceHolder.SetActive(false);
            customTokenDisplay.SetActive(true);
        }
        else
        {
            customTokenDisplay.SetActive(false);
        }
        // Set native token
        nativeTokenSymbolText.text = Web3Accessor.Web3.ChainConfig.Symbol.ToUpper();
    }
    
    private void ToggleAddTokensMenuButton()
    {
        addCustomTokensContainer.SetActive(!addCustomTokensContainer.activeSelf);
    }

    private void AddToken()
    {
        if (!File.Exists(Path.Combine(Application.persistentDataPath, "customToken.txt")))
        {
            File.WriteAllText(Path.Combine(Application.persistentDataPath, "customToken.txt"), $"{customTokenAddressInput.text},{customTokenSymbolInput.text}");
        }
        customTokenSymbolText.text = customTokenSymbolInput.text.ToUpper();
        customTokenAddressInput.text = string.Empty;
        customTokenSymbolInput.text = string.Empty;
        customTokenDisplay.SetActive(true);
    }
    
    private void ToggleTransferTokensMenuButton()
    {
        transferTokensContainer.SetActive(!transferTokensContainer.activeSelf);
    }
    
    private async void TransferTokens()
    {
        // Set token options
        if (selectedTokenToTransfer.options == null)
        {
            selectedTokenToTransfer.options = new List<TMP_Dropdown.OptionData>(2);
            selectedTokenToTransfer.options[0].text = nativeTokenSymbolText.text;
            selectedTokenToTransfer.options[1].text = customTokenSymbolText.text;
        }
        
        // Token transfers
        switch (selectedTokenToTransfer.value)
        {
            case 0:
                var amountInWei0 = BigInteger.Parse(transferTokensAmountInput.text) * (BigInteger)Math.Pow(10, 18);
                var response0 = await Evm.SendTransaction(Web3Accessor.Web3, transferTokensWalletInput.text, amountInWei0);
                SampleOutputUtil.PrintResult(response0, nameof(Evm), nameof(Evm.SendTransaction));
                break;
            case 1:
                var amountInWei1 = BigInteger.Parse(transferTokensAmountInput.text) * (BigInteger)Math.Pow(10, 18);
                var response1 = await Web3Accessor.Web3.Erc20.Transfer(customTokenContract, transferTokensWalletInput.text, amountInWei1);
                var output = SampleOutputUtil.BuildOutputValue(response1);
                SampleOutputUtil.PrintResult(output, "ERC-20", nameof(Erc20Service.Transfer));
                break;
            default:
                throw new Web3Exception("Token can't be found");
        }
    }
    
    #endregion
}
