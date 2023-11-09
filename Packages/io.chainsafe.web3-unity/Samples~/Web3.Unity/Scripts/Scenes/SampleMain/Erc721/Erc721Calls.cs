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

    private string contractBalanceOf = "0x9123541E259125657F03D7AD2A7D1a8Ec79375BA";
    private string accountBalanceOf = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";

    #endregion

    #region Owner Of

    private string contractOwnerOf = "0x06dc21f89f01409e7ed0e4c80eae1430962ae52c";
    private string tokenIdOwnerOf = "0x01559ae4021a565d5cc4740f1cefa95de8c1fb193949ecd32c337b03047da501";

    #endregion
    
    #region Owner Of Batch

    private string[] tokenIdsOwnerOfBatch = { "33", "29" };
    private string contractOwnerOfBatch = "0x47381c5c948254e6e0E324F1AA54b7B24104D92D";
    // optional: multicall contract https://github.com/makerdao/multicall
    private string multicallOwnerOfBatch = "0x77dca2c955b15e9de4dbbcf1246b4b85b651e50e";

    #endregion

    #region Uri

    private string contractUri = "0x06dc21f89f01409e7ed0e4c80eae1430962ae52c";
    private string tokenIdUri = "0x01559ae4021a565d5cc4740f1cefa95de8c1fb193949ecd32c337b03047da501";

    #endregion
    
    #region Mint

    private string contractMint = "0x0B102638532be8A1b3d0ed1fcE6eC603Bec37848";
    private string uriMint = "ipfs://QmNn5EaGR26kU7aAMH7LhkNsAGcmcyJgun3Wia4MftVicW/1.json";

    #endregion
    
    #region Transfer

    private string contractTransfer = "0x358AA13c52544ECCEF6B0ADD0f801012ADAD5eE3";
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
        var balance = await Erc721.BalanceOf(Web3Accessor.Web3, contractBalanceOf, accountBalanceOf);
        SampleOutputUtil.PrintResult(balance.ToString(), nameof(Erc721), nameof(Erc721.BalanceOf));
    }
    
    /// <summary>
    /// Owner Of ERC721 tokens
    /// </summary>
    public async void OwnerOf()
    {
        var owner = tokenIdOwnerOf.StartsWith("0x") ? 
            await Erc721.OwnerOf(Web3Accessor.Web3, contractOwnerOf, tokenIdOwnerOf) 
            : await Erc721.OwnerOf(Web3Accessor.Web3, contractOwnerOf, BigInteger.Parse(tokenIdOwnerOf));
        SampleOutputUtil.PrintResult(owner, nameof(Erc721), nameof(Erc721.OwnerOf));
    }
    
    /// <summary>
    /// Owner Of batch ERC721
    /// </summary>
    public async void OwnerOfBatch()
    {
        var owners = await Erc721.OwnerOfBatch(Web3Accessor.Web3, contractOwnerOfBatch, tokenIdsOwnerOfBatch, multicallOwnerOfBatch);
        var ownersString = $"{owners.Count} owner(s):\n" + string.Join(",\n", owners);
        SampleOutputUtil.PrintResult(ownersString, nameof(Erc721), nameof(Erc721.OwnerOfBatch));
    }
    
    /// <summary>
    /// Uri Of ERC721 Address
    /// </summary>
    public async void Uri()
    {
        var uri = await Erc721.Uri(Web3Accessor.Web3, contractUri, tokenIdUri);
        SampleOutputUtil.PrintResult(uri, nameof(Erc721), nameof(Erc721.Uri));
    }
    
    /// <summary>
    /// Mint ERC1155 tokens
    /// </summary>
    public async void MintErc721()
    {
        var response = await Erc721.MintErc721(Web3Accessor.Web3, ABI.Mint721, contractMint, uriMint);
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