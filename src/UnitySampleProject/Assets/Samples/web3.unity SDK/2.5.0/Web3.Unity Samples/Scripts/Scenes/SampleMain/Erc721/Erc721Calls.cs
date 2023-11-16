using System.Linq;
using System.Numerics;
using ChainSafe.Gaming.UnityPackage;
using Scripts.EVM.Token;
using UnityEngine;

/// <summary>
/// ERC721 calls used in the sample scene
/// </summary>
public class Erc721Calls : MonoBehaviour
{
    #region Fields

    #region All Erc
    
    private string accountAllErc = "0xfaecAE4464591F8f2025ba8ACF58087953E613b1";

    #endregion

    #region Balance Of
    
    private string accountBalanceOf = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";

    #endregion

    #region Owner Of
    
    private string tokenIdOwnerOf = "1";

    #endregion
    
    #region Owner Of Batch

    private string[] tokenIdsOwnerOfBatch = { "4", "6" };

    #endregion

    #region Uri
    
    private string tokenIdUri = "QmfUHuFj3YL2JMZkyXNtGRV8e9aLJgQ6gcSrqbfjWFvbqQ";

    #endregion
    
    #region Mint
    
    private string uriMint = "1";

    #endregion
    
    #region Transfer

    private string contractTransfer = "0x4f75BB7bdd6f7A0fD32f1b3A94dfF409F5a3F1CC";
    private string toAccountTransfer = "0xdD4c825203f97984e7867F11eeCc813A036089D1";
    private int tokenIdTransfer = 1;

    #endregion

    #endregion
    
    /// <summary>
    /// All ERC 721 tokens belonging to an address
    /// </summary>
    public async void AllErc721()
    {
        var allNfts = await Erc721.AllErc721(Web3Accessor.Web3, accountAllErc);
        var output = string.Join(",\n", allNfts.Select(nft => $"{nft.TokenId} - {nft.Uri}"));
        SampleOutputUtil.PrintResult(output, nameof(Erc721), nameof(Erc721.AllErc721));
    }
    
    /// <summary>
    /// Balance Of ERC721 Address
    /// </summary>
    public async void BalanceOf()
    {
        var balance = await Erc721.BalanceOf(Web3Accessor.Web3, Contracts.Erc721, accountBalanceOf);
        SampleOutputUtil.PrintResult(balance.ToString(), nameof(Erc721), nameof(Erc721.BalanceOf));
    }
    
    /// <summary>
    /// Owner Of ERC721 tokens
    /// </summary>
    public async void OwnerOf()
    {
        var owner = tokenIdOwnerOf.StartsWith("0x") ? 
            await Erc721.OwnerOf(Web3Accessor.Web3, Contracts.Erc721, tokenIdOwnerOf) 
            : await Erc721.OwnerOf(Web3Accessor.Web3, Contracts.Erc721, BigInteger.Parse(tokenIdOwnerOf));
        SampleOutputUtil.PrintResult(owner, nameof(Erc721), nameof(Erc721.OwnerOf));
    }
    
    /// <summary>
    /// Owner Of batch ERC721
    /// </summary>
    public async void OwnerOfBatch()
    {
        var owners = await Erc721.OwnerOfBatch(Web3Accessor.Web3, Contracts.Erc721, tokenIdsOwnerOfBatch);
        var ownersString = $"{owners.Count} owner(s):\n" + string.Join(",\n", owners);
        SampleOutputUtil.PrintResult(ownersString, nameof(Erc721), nameof(Erc721.OwnerOfBatch));
    }
    
    /// <summary>
    /// Uri Of ERC721 Address
    /// </summary>
    public async void Uri()
    {
        var uri = await Erc721.Uri(Web3Accessor.Web3, Contracts.Erc721, tokenIdUri);
        SampleOutputUtil.PrintResult(uri, nameof(Erc721), nameof(Erc721.Uri));
    }
    
    /// <summary>
    /// Mint ERC1155 tokens
    /// </summary>
    public async void MintErc721()
    {
        var response = await Erc721.MintErc721(Web3Accessor.Web3, ABI.Erc721, Contracts.Erc721, uriMint);
        var output = SampleOutputUtil.BuildOutputValue(response);
        SampleOutputUtil.PrintResult(output, nameof(Erc721), nameof(Erc721.MintErc721));
    }
    
    /// <summary>
    /// Transfer ERC1155 tokens
    /// </summary>
    public async void TransferErc721()
    {
        var response = await Erc721.TransferErc721(Web3Accessor.Web3, contractTransfer, toAccountTransfer, tokenIdTransfer);
        var output = SampleOutputUtil.BuildOutputValue(response);
        SampleOutputUtil.PrintResult(output, nameof(Erc721), nameof(Erc721.TransferErc721));
    }
}