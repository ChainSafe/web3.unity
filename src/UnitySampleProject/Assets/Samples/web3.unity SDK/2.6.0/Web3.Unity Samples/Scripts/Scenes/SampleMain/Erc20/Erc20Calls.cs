using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts.BuiltIn;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Connection;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core.Logout;
using Scripts.EVM.Token;
using UnityEngine;
using Erc20Contract = ChainSafe.Gaming.Evm.Contracts.Custom.Erc20Contract;

/// <summary>
/// ERC20 calls used in the sample scene
/// </summary>
public class Erc20Calls : MonoBehaviour, IWeb3InitializedHandler, IWeb3BuilderServiceAdapter, ILogoutHandler
{
    #region Fields

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

    [Header("Transfer Call")] [SerializeField]
    private string toAccount = "0xdD4c825203f97984e7867F11eeCc813A036089D1";

    [SerializeField] private BigInteger amountTransfer = 1000000000000000;

    #endregion

    #endregion

    private Erc20Contract _erc20;

    /// <summary>
    /// Balance Of ERC20 Address
    /// </summary>
    public async void BalanceOf()
    {
        var balance = await _erc20.BalanceOf(accountBalanceOf);
        SampleOutputUtil.PrintResult(balance.ToString(), "ERC-20", nameof(Erc20Service.GetBalanceOf));
    }

    /// <summary>
    /// Native ERC20 balance of an Address
    /// </summary>
    public async void NativeBalanceOf()
    {
        var balance = await Web3Unity.Web3.RpcProvider.GetBalance(accountBalanceOf);
        SampleOutputUtil.PrintResult(balance.ToString(), "ERC-20", nameof(NativeBalanceOf));
    }

    /// <summary>
    /// Name of ERC20 Token
    /// </summary>
    public async void Name()
    {
        var name = await _erc20.Name();
        SampleOutputUtil.PrintResult(name, "ERC-20", nameof(Erc20Service.GetName));
    }

    /// <summary>
    /// Symbol of ERC20 Token
    /// </summary>
    public async void Symbol()
    {
        var symbol = await _erc20.Symbol();
        SampleOutputUtil.PrintResult(symbol, "ERC-20", nameof(Erc20Service.GetSymbol));
    }

    /// <summary>
    /// Decimals of ERC20 Token
    /// </summary>
    public async void Decimals()
    {
        var decimals = await _erc20.Decimals();
        SampleOutputUtil.PrintResult(decimals.ToString(), "ERC-20", nameof(Erc20Service.GetDecimals));
    }

    /// <summary>
    /// Total Supply of ERC20 Token
    /// </summary>
    public async void TotalSupply()
    {
        var totalSupply = await _erc20.TotalSupply();
        SampleOutputUtil.PrintResult(totalSupply.ToString(), "ERC-20", nameof(Erc20Service.GetTotalSupply));
    }

    /// <summary>
    /// Mints ERC20 Tokens to an address
    /// </summary>
    public async void MintErc20()
    {
        var mintResponse = await _erc20.MintWithReceipt(Web3Unity.Web3.Signer.PublicAddress, valueToSend * weiPerEther);
        var output = SampleOutputUtil.BuildOutputValue(new object[] { mintResponse.TransactionHash });
        SampleOutputUtil.PrintResult(output, "ERC-20", nameof(Erc20Service.Mint));
    }

    /// <summary>
    /// Transfers ERC20 Tokens to an address
    /// </summary>
    public async void TransferErc20()
    {
        var mintResponse = await _erc20.Transfer(toAccount, amountTransfer);
        var output = SampleOutputUtil.BuildOutputValue(new object[] { mintResponse });
        _erc20.OnTransfer += Test;
        SampleOutputUtil.PrintResult(output, "ERC-20", nameof(Erc20Service.Transfer));
    }

    private void Test(Erc20Contract.TransferEventDTO obj)
    {
        Debug.LogError("TRANSFERED" + obj.ToString());
        _erc20.OnTransfer -= Test;
    }

    public int Priority => 0;
    
    public async Task OnWeb3Initialized(Web3 web3)
    {
        _erc20 = await web3.ContractBuilder.Build<Erc20Contract>(ChainSafeContracts.Erc20);
    }

    public Web3Builder ConfigureServices(Web3Builder web3Builder)
    {
        return web3Builder.Configure(services =>
        {
            services.AddSingleton<IWeb3InitializedHandler, ILogoutHandler, Erc20Calls>(_ => this);
        });
    }

    public async Task OnLogout()
    {
        await _erc20.DisposeAsync();
    }
}