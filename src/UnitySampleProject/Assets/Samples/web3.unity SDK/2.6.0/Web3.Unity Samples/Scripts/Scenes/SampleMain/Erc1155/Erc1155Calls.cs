using System.Numerics;
using ChainSafe.Gaming.Evm.Contracts.BuiltIn;
using ChainSafe.Gaming.UnityPackage;
using Scripts.EVM.Token;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ERC1155 calls used in the sample scene
/// </summary>
public class Erc1155Calls : MonoBehaviour
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
    
    /// <summary>
    /// Balance Of ERC1155 Address
    /// </summary>
    public async void BalanceOf()
    {
        var balance = await Web3Accessor.Web3.Erc1155.GetBalanceOf(
            ChainSafeContracts.Erc1155, 
            tokenIdBalanceOf,
            accountBalanceOf);
        SampleOutputUtil.PrintResult(balance.ToString(), "ERC-1155",nameof(Erc1155Service.GetBalanceOf));
    }

    /// <summary>
    /// Balance Of batch ERC1155
    /// </summary>
    public async void BalanceOfBatch()
    {
        var balances = await Web3Accessor.Web3.Erc1155.GetBalanceOfBatch(
            ChainSafeContracts.Erc1155,
            accountsBalanceOfBatch, 
            tokenIdsBalanceOfBatch);
        SampleOutputUtil.PrintResult(string.Join(", ", balances), "ERC-1155",nameof(Erc1155Service.GetBalanceOfBatch));
    }

    /// <summary>
    /// Uri Of ERC1155 Address
    /// </summary>
    public async void Uri()
    {
        var uri = await Web3Accessor.Web3.Erc1155.GetUri(
            ChainSafeContracts.Erc1155,
            tokenIdUri);
        SampleOutputUtil.PrintResult(uri, "ERC-1155",nameof(Erc1155Service.GetUri));
    }

    /// <summary>
    /// Mint ERC1155 tokens
    /// </summary>
    public async void MintErc1155()
    {
        var response = await Web3Accessor.Web3.Erc1155.Mint(
            ChainSafeContracts.Erc1155,
            idMint,
            amountMint);
        var output = SampleOutputUtil.BuildOutputValue(response);
        SampleOutputUtil.PrintResult(output, "ERC-1155",nameof(Erc1155Service.Mint));
    }

    /// <summary>
    /// Transfer ERC1155 tokens
    /// </summary>
    public async void TransferErc1155()
    {
        var response = await Web3Accessor.Web3.Erc1155.Transfer(
            ChainSafeContracts.Erc1155,
            tokenIdTransfer,
            amountTransfer,
            toAccountTransfer);
        var output = SampleOutputUtil.BuildOutputValue(response);
        SampleOutputUtil.PrintResult(output, "ERC-1155",nameof(Erc1155Service.Transfer));
    }

    /// <summary>
    /// Imports an NFTs texture via URI data
    /// </summary>
    public async void ImportNftTexture1155()
    {
        var texture = await Web3Accessor.Web3.Erc1155.ImportTexture(ChainSafeContracts.Erc1155, tokenIdTexture);
        rawImage.texture = texture;
    }
}