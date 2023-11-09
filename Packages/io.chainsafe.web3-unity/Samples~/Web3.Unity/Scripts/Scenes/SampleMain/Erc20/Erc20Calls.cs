﻿using System.Numerics;
using ChainSafe.Gaming.UnityPackage;
using Scripts.EVM.Token;
using UnityEngine;

/// <summary>
/// ERC20 calls used in the sample scene
/// </summary>
public class Erc20Calls : MonoBehaviour
{
    #region Fields

    #region Balance Of

    private string contractBalanceOf = "0x3E0C0447e47d49195fbE329265E330643eB42e6f";
    private string accountBalanceOf = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";

    #endregion
    
    #region Custom Balance Of

    private string contractCustomBalanceOf = "0x99D555E4dAf4f7e103893AD075CFC605fB8e3544";
    private string AbiCustomBalanceOf = "[ { \"inputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"constructor\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"_owner\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"_spender\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"Approval\", \"type\": \"event\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"_spender\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"approve\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"success\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"_spender\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"decreaseAllowance\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"_owner\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"_spender\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"DecreaseAllowance\", \"type\": \"event\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"_owner\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"decreaseMapping\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"_owner\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"DecreaseMapping\", \"type\": \"event\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"_spender\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"increaseAllowance\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"_owner\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"_spender\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"IncreaseAllowance\", \"type\": \"event\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"_owner\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"increaseMapping\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"_owner\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"IncreaseMapping\", \"type\": \"event\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"_owner\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"mint\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"success\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"_owner\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"Mint\", \"type\": \"event\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"_to\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"transfer\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"success\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"_from\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"_to\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"Transfer\", \"type\": \"event\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"_from\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"_to\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"transferFrom\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"success\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"_decimal\", \"outputs\": [ { \"internalType\": \"uint8\", \"name\": \"\", \"type\": \"uint8\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"_name\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"_symbol\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"_totalSupply\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"_owner\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"_spender\", \"type\": \"address\" } ], \"name\": \"allowance\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"remaining\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"_owner\", \"type\": \"address\" } ], \"name\": \"balanceOf\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"balance\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"decimals\", \"outputs\": [ { \"internalType\": \"uint8\", \"name\": \"\", \"type\": \"uint8\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"earnSupply\", \"outputs\": [ { \"internalType\": \"int256\", \"name\": \"\", \"type\": \"int256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"name\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"symbol\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"totalSupply\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" } ], \"name\": \"uintMapping\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" } ]";
    
    #endregion
    
    #region Native Balance Of

    private string accountNativeBalanceOf = "0xaBed4239E4855E120fDA34aDBEABDd2911626BA1";

    #endregion
    
    #region Token Info

    private string contractToken = "0x3E0C0447e47d49195fbE329265E330643eB42e6f";

    #endregion
    
    #region Mint
    
    private const string contractMint = "0x714d32fA722461A2c8F0b4EB98ff5cFF8F908Df2";
    private const string toAccountMint = "0xdD4c825203f97984e7867F11eeCc813A036089D1";
    private BigInteger amountMint = 1000000000000000000;

    #endregion
    
    #region Transfer

    private string contractTransfer = "0xc778417e063141139fce010982780140aa0cd5ab";
    private string toAccountTransfer = "0xdD4c825203f97984e7867F11eeCc813A036089D1";
    private string amountTransfer = "1000000000000000"; // todo to double representing one unit of currency

    #endregion
    
    #endregion
    
    /// <summary>
    /// Balance Of ERC20 Address
    /// </summary>
    public async void BalanceOf()
    {
        var balance = await Erc20.BalanceOf(Web3Accessor.Web3, contractBalanceOf, accountBalanceOf);
        SampleOutputUtil.PrintResult(balance.ToString(), nameof(Erc20), nameof(Erc20.BalanceOf));
    }
    
    /// <summary>
    /// Custom ERC20 token balance of an address
    /// </summary>
    public async void CustomTokenBalanceOf()
    {
        var result = await Erc20.CustomTokenBalance(Web3Accessor.Web3, AbiCustomBalanceOf, contractCustomBalanceOf);
        SampleOutputUtil.PrintResult(result, nameof(Erc20), nameof(Erc20.CustomTokenBalance));
    }
    
    /// <summary>
    /// Native ERC20 balance of an Address
    /// </summary>
    public async void NativeBalanceOf()
    {
        var result = await Erc20.NativeBalanceOf(Web3Accessor.Web3, accountNativeBalanceOf);
        SampleOutputUtil.PrintResult(result.ToString(), nameof(Erc20), nameof(Erc20.NativeBalanceOf));
    }
    
    /// <summary>
    /// Name of ERC20 Token
    /// </summary>
    public async void Name()
    {
        var result = await Erc20.Name(Web3Accessor.Web3, contractToken);
        SampleOutputUtil.PrintResult(result, nameof(Erc20), nameof(Erc20.Name));
    }
    
    /// <summary>
    /// Symbol of ERC20 Token
    /// </summary>
    public async void Symbol()
    {
        var result = await Erc20.Symbol(Web3Accessor.Web3, contractToken);
        SampleOutputUtil.PrintResult(result, nameof(Erc20), nameof(Erc20.Symbol));
    }
    
    /// <summary>
    /// Decimals of ERC20 Token
    /// </summary>
    public async void Decimals()
    {
        var decimals = await Erc20.Decimals(Web3Accessor.Web3, contractToken);
        SampleOutputUtil.PrintResult(decimals.ToString(), nameof(Erc20), nameof(Erc20.Decimals));
    }
    
    /// <summary>
    /// Total Supply of ERC20 Token
    /// </summary>
    public async void TotalSupply()
    {
        var result = await Erc20.TotalSupply(Web3Accessor.Web3, contractToken);
        SampleOutputUtil.PrintResult(result.ToString(), nameof(Erc20), nameof(Erc20.TotalSupply));
    }
    
    /// <summary>
    /// Mints ERC20 Tokens to an address
    /// </summary>
    public async void MintErc20()
    {
        string toAccount = await Web3Accessor.Web3.Signer.GetAddress();
        var response = await Erc20.MintErc20(Web3Accessor.Web3, contractMint, toAccount, amountMint);
        var output = SampleOutputUtil.BuildOutputValue(response);
        SampleOutputUtil.PrintResult(output, nameof(Erc20), nameof(Erc20.MintErc20));
    }
    
    /// <summary>
    /// Transfers ERC20 Tokens to an address
    /// </summary>
    public async void TransferErc20()
    {
        var response = await Erc20.TransferErc20(Web3Accessor.Web3, contractTransfer, toAccountTransfer, amountTransfer);
        var output = SampleOutputUtil.BuildOutputValue(response);
        SampleOutputUtil.PrintResult(output, nameof(Erc20), nameof(Erc20.TransferErc20));
    }
}