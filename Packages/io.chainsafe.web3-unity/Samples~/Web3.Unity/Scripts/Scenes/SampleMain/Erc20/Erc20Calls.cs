using System.Numerics;
using ChainSafe.Gaming.Evm.Contracts.BuiltIn;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.UnityPackage;
using Scripts.EVM.Token;
using UnityEngine;

/// <summary>
/// ERC20 calls used in the sample scene
/// </summary>
public class Erc20Calls : MonoBehaviour
{
    #region Fields
    [Header("Change the fields below for testing purposes")]

    #region Balance Of
    
    [Header("Balance Of Call")]
    [SerializeField] private string accountBalanceOf = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";

    #endregion

    #region Mint
    
    [Header("Mint Call")]
    [SerializeField] private BigInteger amountMint = 1000000000000000000;

    #endregion

    #region Transfer
    
    [Header("Transfer Call")]
    [SerializeField] private string toAccount = "0xdD4c825203f97984e7867F11eeCc813A036089D1";
    [SerializeField] private BigInteger amountTransfer = 1000000000000000;

    #endregion

    #endregion

    /// <summary>
    /// Balance Of ERC20 Address
    /// </summary>
    public async void BalanceOf()
    {
        var balance = await Web3Accessor.Web3.Erc20.GetBalanceOf(ChainSafeContracts.Erc20, accountBalanceOf);
        SampleOutputUtil.PrintResult(balance.ToString(), "ERC-20", nameof(Erc20Service.GetBalanceOf));
    }

    /// <summary>
    /// Native ERC20 balance of an Address
    /// </summary>
    public async void NativeBalanceOf()
    {
        var balance = await Web3Accessor.Web3.RpcProvider.GetBalance(accountBalanceOf);
        SampleOutputUtil.PrintResult(balance.ToString(), "ERC-20", nameof(NativeBalanceOf));
    }

    /// <summary>
    /// Name of ERC20 Token
    /// </summary>
    public async void Name()
    {
        var name = await Web3Accessor.Web3.Erc20.GetName(ChainSafeContracts.Erc20);
        SampleOutputUtil.PrintResult(name, "ERC-20", nameof(Erc20Service.GetName));
    }

    /// <summary>
    /// Symbol of ERC20 Token
    /// </summary>
    public async void Symbol()
    {
        var symbol = await Web3Accessor.Web3.Erc20.GetSymbol(ChainSafeContracts.Erc20);
        SampleOutputUtil.PrintResult(symbol, "ERC-20", nameof(Erc20Service.GetSymbol));
    }

    /// <summary>
    /// Decimals of ERC20 Token
    /// </summary>
    public async void Decimals()
    {
        var decimals = await Web3Accessor.Web3.Erc20.GetDecimals(ChainSafeContracts.Erc20);
        SampleOutputUtil.PrintResult(decimals.ToString(), "ERC-20", nameof(Erc20Service.GetDecimals));
    }

    /// <summary>
    /// Total Supply of ERC20 Token
    /// </summary>
    public async void TotalSupply()
    {
        var totalSupply = await Web3Accessor.Web3.Erc20.GetTotalSupply(ChainSafeContracts.Erc20);
        SampleOutputUtil.PrintResult(totalSupply.ToString(), "ERC-20", nameof(Erc20Service.GetTotalSupply));
    }

    /// <summary>
    /// Mints ERC20 Tokens to an address
    /// </summary>
    public async void MintErc20()
    {
        var mintResponse = await Web3Accessor.Web3.Erc20.Mint(ChainSafeContracts.Erc20, amountMint, toAccount);
        var output = SampleOutputUtil.BuildOutputValue(mintResponse);
        SampleOutputUtil.PrintResult(output, "ERC-20", nameof(Erc20Service.Mint));
    }

    /// <summary>
    /// Transfers ERC20 Tokens to an address
    /// </summary>
    public async void TransferErc20()
    {
        var mintResponse = await Web3Accessor.Web3.Erc20.Transfer(ChainSafeContracts.Erc20, toAccount, amountTransfer);
        var output = SampleOutputUtil.BuildOutputValue(mintResponse);
        SampleOutputUtil.PrintResult(output, "ERC-20", nameof(Erc20Service.Transfer));
    }
}