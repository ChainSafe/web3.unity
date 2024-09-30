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
public class Erc20Calls : SampleBase<Erc20Calls>, IWeb3InitializedHandler, ILogoutHandler
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
    public async Task<string> BalanceOf()
    {
        var balance = await _erc20.BalanceOf(accountBalanceOf);
        
        return SampleOutputUtil.BuildResultMessage(balance.ToString(), "ERC-20", nameof(Erc20Service.GetBalanceOf));
    }

    /// <summary>
    /// Native ERC20 balance of an Address
    /// </summary>
    public async Task<string> NativeBalanceOf()
    {
        var balance = await Web3Unity.Web3.RpcProvider.GetBalance(accountBalanceOf);
        
        return SampleOutputUtil.BuildResultMessage(balance.ToString(), "ERC-20", nameof(NativeBalanceOf));
    }

    /// <summary>
    /// Name of ERC20 Token
    /// </summary>
    public async Task<string> Name()
    {
        var getName = await _erc20.Name();
        return SampleOutputUtil.BuildResultMessage(getName, "ERC-20", nameof(Erc20Service.GetName));
    }

    /// <summary>
    /// Symbol of ERC20 Token
    /// </summary>
    public async Task<string> Symbol()
    {
        var symbol = await _erc20.Symbol();
        return SampleOutputUtil.BuildResultMessage(symbol, "ERC-20", nameof(Erc20Service.GetSymbol));
    }

    /// <summary>
    /// Decimals of ERC20 Token
    /// </summary>
    public async Task<string> Decimals()
    {
        var decimals = await _erc20.Decimals();
        return SampleOutputUtil.BuildResultMessage(decimals.ToString(), "ERC-20", nameof(Erc20Service.GetDecimals));
    }

    /// <summary>
    /// Total Supply of ERC20 Token
    /// </summary>
    public async Task<string> TotalSupply()
    {
        var totalSupply = await _erc20.TotalSupply();
        return SampleOutputUtil.BuildResultMessage(totalSupply.ToString(), "ERC-20", nameof(Erc20Service.GetTotalSupply));
    }

    /// <summary>
    /// Mints ERC20 Tokens to an address
    /// </summary>
    public async Task<string> MintErc20()
    {
        var mintResponse = await _erc20.MintWithReceipt(Web3Unity.Web3.Signer.PublicAddress, valueToSend * weiPerEther);
        var output = SampleOutputUtil.BuildOutputValue(new object[] { mintResponse.TransactionHash });
        return SampleOutputUtil.BuildResultMessage(output, "ERC-20", nameof(Erc20Service.Mint));
    }

    /// <summary>
    /// Transfers ERC20 Tokens to an address
    /// </summary>
    public async Task<string> TransferErc20()
    {
        var mintResponse = await _erc20.Transfer(toAccount, amountTransfer);
        var output = SampleOutputUtil.BuildOutputValue(new object[] { mintResponse });
        _erc20.OnTransfer += Test;
        return SampleOutputUtil.BuildResultMessage(output, "ERC-20", nameof(Erc20Service.Transfer));
    }

    private void Test(Erc20Contract.TransferEventDTO obj)
    {
        Debug.LogError("TRANSFERED" + obj.ToString());
        _erc20.OnTransfer -= Test;
    }

    public async Task OnWeb3Initialized(Web3 web3)
    {
        _erc20 = await web3.ContractBuilder.Build<Erc20Contract>(ChainSafeContracts.Erc20);
    }

    public override Web3Builder ConfigureServices(Web3Builder web3Builder)
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