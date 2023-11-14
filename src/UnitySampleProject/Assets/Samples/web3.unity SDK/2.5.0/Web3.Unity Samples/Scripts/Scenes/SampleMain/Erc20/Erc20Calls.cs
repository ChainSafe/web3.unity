using System.Numerics;
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
    
    private string accountBalanceOf = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";

    #endregion
    
    #region Mint
    
    private BigInteger amountMint = 1000000000000000000;

    #endregion
    
    #region Transfer

    private const string toAccount = "0xdD4c825203f97984e7867F11eeCc813A036089D1";
    private BigInteger amountTransfer = 1000000000000000;

    #endregion
    
    #endregion
    
    /// <summary>
    /// Balance Of ERC20 Address
    /// </summary>
    public async void BalanceOf()
    {
        SampleFeedback.Instance?.Activate();
        var balance = await Erc20.BalanceOf(Web3Accessor.Web3, Contracts.Erc20, accountBalanceOf);
        SampleOutputUtil.PrintResult(balance.ToString(), nameof(Erc20), nameof(Erc20.BalanceOf));
        SampleFeedback.Instance?.Deactivate();
    }
    
    /// <summary>
    /// Custom ERC20 token balance of an address
    /// </summary>
    public async void CustomTokenBalanceOf()
    {
        var result = await Erc20.CustomTokenBalance(Web3Accessor.Web3, ABI.Erc20, Contracts.Erc20);
        SampleOutputUtil.PrintResult(result.ToString(), nameof(Erc20), nameof(Erc20.CustomTokenBalance));
    }
    
    /// <summary>
    /// Native ERC20 balance of an Address
    /// </summary>
    public async void NativeBalanceOf()
    {
        var result = await Erc20.NativeBalanceOf(Web3Accessor.Web3, accountBalanceOf);
        SampleOutputUtil.PrintResult(result.ToString(), nameof(Erc20), nameof(Erc20.NativeBalanceOf));
    }
    
    /// <summary>
    /// Name of ERC20 Token
    /// </summary>
    public async void Name()
    {
        var result = await Erc20.Name(Web3Accessor.Web3, Contracts.Erc20);
        SampleOutputUtil.PrintResult(result, nameof(Erc20), nameof(Erc20.Name));
    }
    
    /// <summary>
    /// Symbol of ERC20 Token
    /// </summary>
    public async void Symbol()
    {
        var result = await Erc20.Symbol(Web3Accessor.Web3, Contracts.Erc20);
        SampleOutputUtil.PrintResult(result, nameof(Erc20), nameof(Erc20.Symbol));
    }
    
    /// <summary>
    /// Decimals of ERC20 Token
    /// </summary>
    public async void Decimals()
    {
        var decimals = await Erc20.Decimals(Web3Accessor.Web3, Contracts.Erc20);
        SampleOutputUtil.PrintResult(decimals.ToString(), nameof(Erc20), nameof(Erc20.Decimals));
    }
    
    /// <summary>
    /// Total Supply of ERC20 Token
    /// </summary>
    public async void TotalSupply()
    {
        var result = await Erc20.TotalSupply(Web3Accessor.Web3, Contracts.Erc20);
        SampleOutputUtil.PrintResult(result.ToString(), nameof(Erc20), nameof(Erc20.TotalSupply));
    }
    
    /// <summary>
    /// Mints ERC20 Tokens to an address
    /// </summary>
    public async void MintErc20()
    {
        string toAccount = await Web3Accessor.Web3.Signer.GetAddress();
        var response = await Erc20.MintErc20(Web3Accessor.Web3, Contracts.Erc20, toAccount, amountMint);
        var output = SampleOutputUtil.BuildOutputValue(response);
        SampleOutputUtil.PrintResult(output, nameof(Erc20), nameof(Erc20.MintErc20));
    }
    
    /// <summary>
    /// Transfers ERC20 Tokens to an address
    /// </summary>
    public async void TransferErc20()
    {
        var response = await Erc20.TransferErc20(Web3Accessor.Web3, Contracts.Erc20, toAccount, amountTransfer);
        var output = SampleOutputUtil.BuildOutputValue(response);
        SampleOutputUtil.PrintResult(output, nameof(Erc20), nameof(Erc20.TransferErc20));
    }
}