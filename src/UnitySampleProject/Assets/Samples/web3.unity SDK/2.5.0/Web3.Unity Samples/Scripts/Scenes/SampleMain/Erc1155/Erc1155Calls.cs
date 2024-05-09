using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Model;
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
        var balance = tokenIdBalanceOf.StartsWith("0x") ?
            await Erc1155.BalanceOf(Web3Accessor.Web3, Contracts.Erc1155, accountBalanceOf, tokenIdBalanceOf)
            : await Erc1155.BalanceOf(Web3Accessor.Web3, Contracts.Erc1155, accountBalanceOf, BigInteger.Parse(tokenIdBalanceOf));
        SampleOutputUtil.PrintResult(balance.ToString(), nameof(Erc1155), nameof(Erc1155.BalanceOf));
    }

    /// <summary>
    /// Balance Of batch ERC1155
    /// </summary>
    public async void BalanceOfBatch()
    {
        var balances = await Erc1155.BalanceOfBatch(Web3Accessor.Web3, Contracts.Erc1155, accountsBalanceOfBatch, tokenIdsBalanceOfBatch);
        var balancesString = string.Join(", ", balances);
        SampleOutputUtil.PrintResult(balancesString, nameof(Erc1155), nameof(Erc1155.BalanceOfBatch));
    }

    /// <summary>
    /// Uri Of ERC1155 Address
    /// </summary>
    public async void Uri()
    {
        var uri = await Erc1155.Uri(Web3Accessor.Web3, Contracts.Erc1155, tokenIdUri);
        SampleOutputUtil.PrintResult(uri, nameof(Erc1155), nameof(Erc1155.Uri));
    }

    /// <summary>
    /// Mint ERC1155 tokens
    /// </summary>
    public async void MintErc1155()
    {
        var response = await Erc1155.MintErc1155(Web3Accessor.Web3, ABI.Erc1155, Contracts.Erc1155, idMint, amountMint);
        var output = SampleOutputUtil.BuildOutputValue(response);
        SampleOutputUtil.PrintResult(output, nameof(Erc1155), nameof(Erc1155.MintErc1155));
    }

    /// <summary>
    /// Transfer ERC1155 tokens
    /// </summary>
    public async void TransferErc1155()
    {
        var response = await Erc1155.TransferErc1155(Web3Accessor.Web3, Contracts.Erc1155, tokenIdTransfer, amountTransfer, toAccountTransfer);
        var output = SampleOutputUtil.BuildOutputValue(response);
        SampleOutputUtil.PrintResult(output, nameof(Erc1155), nameof(Erc1155.TransferErc1155));
    }

    /// <summary>
    /// Imports an NFTs texture via URI data
    /// </summary>
    public async void ImportNftTexture1155()
    {
        var textureRequest = await Erc1155.ImportNftTexture1155(Web3Accessor.Web3, Contracts.Erc1155, tokenIdTexture);
        rawImage.texture = textureRequest;
    }
}