using System.Linq;
using System.Numerics;
using ChainSafe.Gaming.UnityPackage;
using Scripts.EVM.Token;
using UnityEngine;
using UnityEngine.UI;
using ABI = Scripts.EVM.Token.ABI;

/// <summary>
/// ERC1155 calls used in the sample scene
/// </summary>
public class Erc1155Calls : MonoBehaviour
{
    #region Fields
    
    #region All Erc

    private string chainAllErc = "ethereum";
    private string networkAllErc = "goerli";
    private string accountAllErc = "0xfaecAE4464591F8f2025ba8ACF58087953E613b1";
    private string contractAllErc = "";
    private int takeAllErc = 1000;
    private int skipAllErc = 0;

    #endregion

    #region Balance Of

    private string tokenContract = "0x44a38dc4A733bD9cFe7df25098c5FfD44538B7a6";
    private string accountBalanceOf = "0xD5c8010ef6dff4c83B19C511221A7F8d1e5cFF44";
    private string tokenIdBalanceOf = "1";

    #endregion
    
    #region Balance Of Batch

    private string contractBalanceOfBatch = "0xdc4aff511e1b94677142a43df90f948f9ae181dd";
    private string[] accountsBalanceOfBatch = { "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2", "0xE51995Cdb3b1c109E0e6E67ab5aB31CDdBB83E4a" };
    private string[] tokenIdsBalanceOfBatch = { "1", "2" };
        
    #endregion
    
    #region Uri
    
    private string tokenIdUri = "0x01559ae4021aee70424836ca173b6a4e647287d15cee8ac42d8c2d8d128927e5";

    #endregion
    
    #region Mint
    
    private int idMint = 1;
    private int amountMint = 1;

    #endregion
    
    #region Transfer

    private string toAccountTransfer = "0xdD4c825203f97984e7867F11eeCc813A036089D1";
    private int tokenIdTransfer = 1;
    private int amountTransfer = 1;

    #endregion

    #region Texture
    
    private string tokenIdTexture = "1";

    #endregion
    
    #endregion
    
    public RawImage rawImage;
    
    /// <summary>
    /// All ERC 1155 tokens belonging to an address
    /// </summary>
    public async void AllErc1155()
    {
        var allNfts = await Erc1155.AllErc1155(Web3Accessor.Web3, chainAllErc, networkAllErc, accountAllErc, contractAllErc, takeAllErc, skipAllErc);
        var output = string.Join(",\n", allNfts.Select(nft => $"{nft.TokenId} - {nft.Uri}"));
        SampleOutputUtil.PrintResult(output, nameof(Erc1155), nameof(Erc1155.AllErc1155));
    }
    
    /// <summary>
    /// Balance Of ERC1155 Address
    /// </summary>
    public async void BalanceOf()
    {
        var balance = tokenIdBalanceOf.StartsWith("0x") ? 
            await Erc1155.BalanceOf(Web3Accessor.Web3, tokenContract, accountBalanceOf, tokenIdBalanceOf)
            : await Erc1155.BalanceOf(Web3Accessor.Web3, tokenContract, accountBalanceOf, BigInteger.Parse(tokenIdBalanceOf));
        SampleOutputUtil.PrintResult(balance.ToString(), nameof(Erc1155), nameof(Erc1155.BalanceOf));
    }
    
    /// <summary>
    /// Balance Of batch ERC1155
    /// </summary>
    public async void BalanceOfBatch()
    {
        var balances = await Erc1155.BalanceOfBatch(Web3Accessor.Web3, contractBalanceOfBatch, accountsBalanceOfBatch, tokenIdsBalanceOfBatch);
        var balancesString = string.Join(", ", balances);
        SampleOutputUtil.PrintResult(balancesString, nameof(Erc1155), nameof(Erc1155.BalanceOfBatch));
    }
    
    /// <summary>
    /// Uri Of ERC1155 Address
    /// </summary>
    public async void Uri()
    {
        var uri = await Erc1155.Uri(Web3Accessor.Web3, tokenContract, tokenIdUri);
        SampleOutputUtil.PrintResult(uri, nameof(Erc1155), nameof(Erc1155.Uri));
    }
    
    /// <summary>
    /// Mint ERC1155 tokens
    /// </summary>
    public async void MintErc1155()
    {
        var response = await Erc1155.MintErc1155(Web3Accessor.Web3, ABI.Mint1155, tokenContract, idMint, amountMint);
        var output = SampleOutputUtil.BuildOutputValue(response);
        SampleOutputUtil.PrintResult(output, nameof(Erc1155), nameof(Erc1155.MintErc1155));
    }
    
    /// <summary>
    /// Transfer ERC1155 tokens
    /// </summary>
    public async void TransferErc1155()
    {
        var response = await Erc1155.TransferErc1155(Web3Accessor.Web3, tokenContract, tokenIdTransfer, amountTransfer, toAccountTransfer);
        var output = SampleOutputUtil.BuildOutputValue(response);
        SampleOutputUtil.PrintResult(output, nameof(Erc1155), nameof(Erc1155.TransferErc1155));
    }
    
    /// <summary>
    /// Imports an NFTs texture via URI data
    /// </summary>
    public async void ImportNftTexture1155()
    {
        var textureRequest = await Erc1155.ImportNftTexture1155(Web3Accessor.Web3, tokenContract, tokenIdTexture);
        rawImage.texture = textureRequest;
    }
}