using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts.BuiltIn;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Connection;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core.Logout;
using Scripts.EVM.Token;
using UnityEngine;
using UnityEngine.UI;
using Erc1155Contract = ChainSafe.Gaming.Evm.Contracts.Custom.Erc1155Contract;

/// <summary>
/// ERC1155 calls used in the sample scene
/// </summary>
public class Erc1155Calls : MonoBehaviour, IWeb3InitializedHandler, IWeb3BuilderServiceAdapter, ILogoutHandler
{
    #region Fields
    [Header("Change the fields below for testing purposes")]

    #region Balance Of

    [Header("Balance Of Call")]
    [SerializeField] private string accountBalanceOf = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";
    [SerializeField] private string tokenIdBalanceOf = "1";

    #endregion

    #region Balance Of Batch

    [Header("Balance Of Batch Call")]
    [SerializeField] private string[] accountsBalanceOfBatch = { "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2", "0xE51995Cdb3b1c109E0e6E67ab5aB31CDdBB83E4a" };
    [SerializeField] private string[] tokenIdsBalanceOfBatch = { "1", "2" };

    #endregion

    #region Uri

    [Header("URI Call")]
    [SerializeField] private string tokenIdUri = "1";

    #endregion

    #region Mint

    [Header("Mint Call")]
    [SerializeField] private BigInteger idMint = 1;
    [SerializeField] private BigInteger amountMint = 1;

    #endregion

    #region Transfer

    [Header("Transfer Call")]
    [SerializeField] private string toAccountTransfer = "0xdD4c825203f97984e7867F11eeCc813A036089D1";
    [SerializeField] private BigInteger tokenIdTransfer = 1;
    [SerializeField] private BigInteger amountTransfer = 1;

    #endregion

    #region Texture

    [Header("Token ID for IPFS texture")]
    [SerializeField] private string tokenIdTexture = "0";
    public RawImage rawImage;

    #endregion

    #endregion

    private Erc1155Contract _erc1155;
    
    /// <summary>
    /// Balance Of ERC1155 Address
    /// </summary>
    public async void BalanceOf()
    {
        var balance = await _erc1155.BalanceOf(accountBalanceOf, BigInteger.Parse(tokenIdBalanceOf));
        SampleOutputUtil.PrintResult(balance.ToString(), "ERC-1155", nameof(Erc1155Service.GetBalanceOf));
    }

    /// <summary>
    /// Balance Of batch ERC1155
    /// </summary>
    public async void BalanceOfBatch()
    {
        var balances = await _erc1155.BalanceOfBatch(
            accountsBalanceOfBatch,
            tokenIdsBalanceOfBatch.Select(BigInteger.Parse).ToArray());
        SampleOutputUtil.PrintResult(string.Join(", ", balances), "ERC-1155", nameof(Erc1155Service.GetBalanceOfBatch));
    }

    /// <summary>
    /// Uri Of ERC1155 Address
    /// </summary>
    public async void Uri()
    {
        var uri = await _erc1155.Uri(BigInteger.Parse(tokenIdUri));
        SampleOutputUtil.PrintResult(uri, "ERC-1155", nameof(Erc1155Service.GetUri));
    }

    /// <summary>
    /// Mint ERC1155 tokens
    /// </summary>
    public async void MintErc1155()
    {
        var response = await _erc1155.MintWithReceipt(
            Web3Unity.Web3.Signer.PublicAddress,
            idMint, amountMint, Array.Empty<byte>());
        var output = SampleOutputUtil.BuildOutputValue(new object[] {response.TransactionHash, true});
        SampleOutputUtil.PrintResult(output, "ERC-1155", nameof(Erc1155Service.Mint));
    }

    /// <summary>
    /// Transfer ERC1155 tokens
    /// </summary>
    public async void TransferErc1155()
    {
        var response = await _erc1155.SafeTransferFromWithReceipt(
            Web3Unity.Web3.Signer.PublicAddress,
            toAccountTransfer,
            tokenIdTransfer,
            amountTransfer,
            Array.Empty<byte>()
            );
        var output = SampleOutputUtil.BuildOutputValue(new object[] {true, response.TransactionHash});
        SampleOutputUtil.PrintResult(output, "ERC-1155", nameof(Erc1155Service.Transfer));
    }

    /// <summary>
    /// Imports an NFTs texture via URI data
    /// </summary>
    public async void ImportNftTexture1155()
    {
        var texture = await Web3Unity.Web3.Erc1155.ImportTexture(ChainSafeContracts.Erc1155, tokenIdTexture);
        rawImage.texture = texture;
    }

    public int Priority => 0;
    
    public async Task OnWeb3Initialized(Web3 web3)
    {
        _erc1155 = await web3.ContractBuilder.Build<Erc1155Contract>(ChainSafeContracts.Erc1155);
    }

    public Web3Builder ConfigureServices(Web3Builder web3Builder)
    {
        return web3Builder.Configure(services =>
        {
            services.AddSingleton<IWeb3InitializedHandler, ILogoutHandler, Erc1155Calls>(_ => this);
        });
    }

    public async Task OnLogout()
    {
        await _erc1155.DisposeAsync();
    }
}