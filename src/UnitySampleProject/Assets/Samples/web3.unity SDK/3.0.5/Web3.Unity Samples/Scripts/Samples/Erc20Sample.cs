using System;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.UnityPackage;
using Scripts.EVM.Token;
using UnityEngine;

/// <summary>
/// ERC20 calls used in the sample scene
/// </summary>
public class Erc20Sample : MonoBehaviour, ISample
{
    #region Fields

    [field: SerializeField] public string Title { get; private set; }

    [field: SerializeField, TextArea] public string Description { get; private set; }

    public Type[] DependentServiceTypes => Array.Empty<Type>();

    [Header("Change the fields below for testing purposes")]

    #region Balance Of

    [Header("Balance Of Call")]
    [SerializeField]
    private string accountBalanceOf = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";

    #endregion

    #region Mint

    [Header("Mint Call")] private BigInteger valueToSend = 5;
    private BigInteger weiPerEther = BigInteger.Pow(10, 18);

    #endregion

    #region Transfer

    [Header("Transfer Call")]
    [SerializeField]
    private string toAccount = "0xdD4c825203f97984e7867F11eeCc813A036089D1";

    [SerializeField] private BigInteger amountTransfer = 1000000000000000;

    #endregion

    #endregion

    /// <summary>
    /// Balance Of ERC20 Address
    /// </summary>
    public async Task<string> BalanceOf()
    {
        var balance = await Web3Unity.Web3.Erc20.GetBalanceOf(ChainSafeContracts.Erc20, accountBalanceOf);

        return balance.ToString();
    }

    /// <summary>
    /// Native ERC20 balance of an Address
    /// </summary>
    public async Task<string> NativeBalanceOf()
    {
        var balance = await Web3Unity.Web3.RpcProvider.GetBalance(accountBalanceOf);

        return balance.ToString();
    }

    /// <summary>
    /// Name of ERC20 Token
    /// </summary>
    public async Task<string> Name()
    {
        var getName = await Web3Unity.Web3.Erc20.GetName(ChainSafeContracts.Erc20);

        return getName;
    }

    /// <summary>
    /// Symbol of ERC20 Token
    /// </summary>
    public async Task<string> Symbol()
    {
        var symbol = await Web3Unity.Web3.Erc20.GetSymbol(ChainSafeContracts.Erc20);

        return symbol;
    }

    /// <summary>
    /// Decimals of ERC20 Token
    /// </summary>
    public async Task<string> Decimals()
    {
        var decimals = await Web3Unity.Web3.Erc20.GetDecimals(ChainSafeContracts.Erc20);
        return decimals.ToString();
    }

    /// <summary>
    /// Total Supply of ERC20 Token
    /// </summary>
    public async Task<string> TotalSupply()
    {
        var totalSupply = await Web3Unity.Web3.Erc20.GetTotalSupply(ChainSafeContracts.Erc20);

        return totalSupply.ToString();
    }

    /// <summary>
    /// Mints ERC20 Tokens to an address
    /// </summary>
    public async Task<string> MintErc20()
    {
        var mintResponse = await Web3Unity.Web3.Erc20.MintWithReceipt(ChainSafeContracts.Erc20, valueToSend * weiPerEther);

        return mintResponse.TransactionHash;
    }

    /// <summary>
    /// Transfers ERC20 Tokens to an address
    /// </summary>
    public async Task<string> TransferErc20()
    {
        var mintResponse = await Web3Unity.Web3.Erc20.TransferWithReceipt(ChainSafeContracts.Erc20, toAccount, amountTransfer);

        return mintResponse.ToString();
    }
}