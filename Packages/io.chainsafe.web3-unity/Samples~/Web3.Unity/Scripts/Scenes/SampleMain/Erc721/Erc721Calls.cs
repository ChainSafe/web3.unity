using System.Linq;
using System.Numerics;
using ChainSafe.Gaming.UnityPackage;
using Scripts.EVM.Token;
using UnityEngine;
using ABI = Scripts.EVM.Token.ABI;

/// <summary>
/// ERC721 calls used in the sample scene
/// </summary>
public class Erc721Calls : MonoBehaviour
{
    #region Fields

    #region All Erc

    private string chainAllErc = "ethereum";
    private string networkAllErc = "goerli"; // mainnet goerli
    private string accountAllErc = "0xfaecAE4464591F8f2025ba8ACF58087953E613b1";
    private string contractAllErc = "0x2c1867BC3026178A47a677513746DCc6822A137A";
    private int takeAllErc = 500;
    private int skipAllErc = 0;

    #endregion

    #region Balance Of
    
    private string accountBalanceOf = "0xD5c8010ef6dff4c83B19C511221A7F8d1e5cFF44";

    #endregion

    #region Owner Of
    
    private string tokenIdOwnerOf = "1";

    #endregion
    
    #region Owner Of Batch

    private string[] tokenIdsOwnerOfBatch = { "1", "2" };
    // optional: multicall contract https://github.com/makerdao/multicall
    private string multicallOwnerOfBatch = "0x77dca2c955b15e9de4dbbcf1246b4b85b651e50e";

    #endregion

    #region Uri
    
    private string tokenIdUri = "0x01559ae4021a565d5cc4740f1cefa95de8c1fb193949ecd32c337b03047da501";

    #endregion
    
    #region Mint
    
    private string uriMint = "ipfs://QmNn5EaGR26kU7aAMH7LhkNsAGcmcyJgun3Wia4MftVicW/1.json";

    #endregion
    
    #region Transfer
    
    private string toAccountTransfer = "0xdD4c825203f97984e7867F11eeCc813A036089D1";
    private int tokenIdTransfer = 0;

    #endregion

    #endregion
    
    /// <summary>
    /// All ERC 721 tokens belonging to an address
    /// </summary>
    public async void AllErc721()
    {
        var allNfts = await Erc721.AllErc721(Web3Accessor.Web3, chainAllErc, networkAllErc, accountAllErc, contractAllErc, takeAllErc, skipAllErc);
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
        var owners = await Erc721.OwnerOfBatch(Web3Accessor.Web3, Contracts.Erc721, tokenIdsOwnerOfBatch, multicallOwnerOfBatch);
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
        var response = await Erc721.MintErc721(Web3Accessor.Web3, ABI.Erc1155, Contracts.Erc1155, uriMint);
        var output = SampleOutputUtil.BuildOutputValue(response);
        SampleOutputUtil.PrintResult(output, nameof(Erc721), nameof(Erc721.MintErc721));
    }
    
    /// <summary>
    /// Transfer ERC1155 tokens
    /// </summary>
    public async void TransferErc721()
    {
        var response = await Erc721.TransferErc721(Web3Accessor.Web3, Contracts.Erc721, toAccountTransfer, tokenIdTransfer);
        var output = SampleOutputUtil.BuildOutputValue(response);
        SampleOutputUtil.PrintResult(output, nameof(Erc721), nameof(Erc721.TransferErc721));
    }
}