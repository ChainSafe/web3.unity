using ChainSafe.Gaming.UnityPackage;
using Scripts.EVM.Token;
using UnityEngine;

/// <summary>
/// ERC20 calls used in the sample scene
/// </summary>
public class Erc20Calls : MonoBehaviour
{
    // Fields
    private Erc20 erc20;
    
    // Initializes the protocol class
    public void Awake()
    {
        erc20 = new Erc20(Web3Accessor.Web3);
    }
    
    /// <summary>
    /// Balance Of ERC20 Address
    /// </summary>
    public async void BalanceOf()
    {
        string contractAddress = "0x3E0C0447e47d49195fbE329265E330643eB42e6f";
        string account = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";
        var balance = await erc20.BalanceOf(contractAddress, account);
        SampleOutputUtil.PrintResult(balance.ToString(), nameof(Erc20), nameof(Erc20.BalanceOf));
    }
    
    /// <summary>
    /// Custom ERC20 token balance of an address
    /// </summary>
    public async void CustomTokenBalanceOf()
    {
        string contractAddress = "0x99D555E4dAf4f7e103893AD075CFC605fB8e3544";
        string contractAbi = "[ { \"inputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"constructor\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"_owner\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"_spender\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"Approval\", \"type\": \"event\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"_spender\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"approve\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"success\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"_spender\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"decreaseAllowance\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"_owner\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"_spender\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"DecreaseAllowance\", \"type\": \"event\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"_owner\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"decreaseMapping\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"_owner\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"DecreaseMapping\", \"type\": \"event\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"_spender\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"increaseAllowance\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"_owner\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"_spender\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"IncreaseAllowance\", \"type\": \"event\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"_owner\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"increaseMapping\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"_owner\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"IncreaseMapping\", \"type\": \"event\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"_owner\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"mint\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"success\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"_owner\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"Mint\", \"type\": \"event\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"_to\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"transfer\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"success\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"_from\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"_to\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"Transfer\", \"type\": \"event\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"_from\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"_to\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"transferFrom\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"success\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"_decimal\", \"outputs\": [ { \"internalType\": \"uint8\", \"name\": \"\", \"type\": \"uint8\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"_name\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"_symbol\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"_totalSupply\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"_owner\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"_spender\", \"type\": \"address\" } ], \"name\": \"allowance\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"remaining\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"_owner\", \"type\": \"address\" } ], \"name\": \"balanceOf\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"balance\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"decimals\", \"outputs\": [ { \"internalType\": \"uint8\", \"name\": \"\", \"type\": \"uint8\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"earnSupply\", \"outputs\": [ { \"internalType\": \"int256\", \"name\": \"\", \"type\": \"int256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"name\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"symbol\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"totalSupply\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" } ], \"name\": \"uintMapping\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" } ]";
        var result = await erc20.CustomTokenBalance(contractAbi, contractAddress);
        SampleOutputUtil.PrintResult(result, nameof(Erc20), nameof(Erc20.CustomTokenBalance));
    }
    
    /// <summary>
    /// Native ERC20 balance of an Address
    /// </summary>
    public async void NativeBalanceOf()
    {
        string account = "0xaBed4239E4855E120fDA34aDBEABDd2911626BA1";
        var result = await erc20.NativeBalanceOf(account);
        SampleOutputUtil.PrintResult(result.ToString(), nameof(Erc20), nameof(Erc20.NativeBalanceOf));
    }
    
    /// <summary>
    /// Name of ERC20 Token
    /// </summary>
    public async void Name()
    {
        string contractAddress = "0x3E0C0447e47d49195fbE329265E330643eB42e6f";
        var result = await erc20.Name(contractAddress);
        SampleOutputUtil.PrintResult(result, nameof(Erc20), nameof(Erc20.Name));
    }
    
    /// <summary>
    /// Symbol of ERC20 Token
    /// </summary>
    public async void Symbol()
    {
        string contractAddress = "0x3E0C0447e47d49195fbE329265E330643eB42e6f";
        var result = await erc20.Symbol(contractAddress);
        SampleOutputUtil.PrintResult(result, nameof(Erc20), nameof(Erc20.Symbol));
    }
    
    /// <summary>
    /// Decimals of ERC20 Token
    /// </summary>
    public async void Decimals()
    {
        string contractAddress = "0x3E0C0447e47d49195fbE329265E330643eB42e6f";
        var decimals = await erc20.Decimals(contractAddress);
        SampleOutputUtil.PrintResult(decimals.ToString(), nameof(Erc20), nameof(Erc20.Decimals));
    }
    
    /// <summary>
    /// Total Supply of ERC20 Token
    /// </summary>
    public async void TotalSupply()
    {
        string contractAddress = "0x3E0C0447e47d49195fbE329265E330643eB42e6f";
        var result = await erc20.TotalSupply(contractAddress);
        SampleOutputUtil.PrintResult(result.ToString(), nameof(Erc20), nameof(Erc20.TotalSupply));
    }
}