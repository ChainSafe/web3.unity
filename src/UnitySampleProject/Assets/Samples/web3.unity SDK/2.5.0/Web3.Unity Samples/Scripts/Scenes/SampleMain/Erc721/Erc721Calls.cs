using System.Collections.Generic;
using System.Linq;
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

    private List<string> tokenIdsOwnerOfBatch = new() { "33", "29" };
    private string contractOwnerOfBatch = "0x47381c5c948254e6e0E324F1AA54b7B24104D92D";
    // optional: multicall contract https://github.com/makerdao/multicall
    private string multicallOwnerOfBatch = "0x77dca2c955b15e9de4dbbcf1246b4b85b651e50e";

    #endregion

    #region Uri

    private string contractUri = "0x06dc21f89f01409e7ed0e4c80eae1430962ae52c";
    private string tokenIdUri = "0x01559ae4021a565d5cc4740f1cefa95de8c1fb193949ecd32c337b03047da501";

    #endregion
    
    #region Mint

    private string abiMint = "[{\"inputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"approved\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"Approval\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"ApprovalForAll\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"approve\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"burn\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"},{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"grantRole\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"},{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"renounceRole\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"},{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"revokeRole\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"},{\"indexed\":true,\"internalType\":\"bytes32\",\"name\":\"previousAdminRole\",\"type\":\"bytes32\"},{\"indexed\":true,\"internalType\":\"bytes32\",\"name\":\"newAdminRole\",\"type\":\"bytes32\"}],\"name\":\"RoleAdminChanged\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"sender\",\"type\":\"address\"}],\"name\":\"RoleGranted\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"sender\",\"type\":\"address\"}],\"name\":\"RoleRevoked\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"string\",\"name\":\"uri\",\"type\":\"string\"}],\"name\":\"safeMint\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"safeTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"},{\"internalType\":\"bytes\",\"name\":\"data\",\"type\":\"bytes\"}],\"name\":\"safeTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"setApprovalForAll\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"Transfer\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"transferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"}],\"name\":\"balanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"DEFAULT_ADMIN_ROLE\",\"outputs\":[{\"internalType\":\"bytes32\",\"name\":\"\",\"type\":\"bytes32\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"getApproved\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"}],\"name\":\"getRoleAdmin\",\"outputs\":[{\"internalType\":\"bytes32\",\"name\":\"\",\"type\":\"bytes32\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes32\",\"name\":\"role\",\"type\":\"bytes32\"},{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"hasRole\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"}],\"name\":\"isApprovedForAll\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"MINTER_ROLE\",\"outputs\":[{\"internalType\":\"bytes32\",\"name\":\"\",\"type\":\"bytes32\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"name\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"ownerOf\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes4\",\"name\":\"interfaceId\",\"type\":\"bytes4\"}],\"name\":\"supportsInterface\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"symbol\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"tokenURI\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";
    private string contractMint = "0x0B102638532be8A1b3d0ed1fcE6eC603Bec37848";
    private string uriMint = "ipfs://QmNn5EaGR26kU7aAMH7LhkNsAGcmcyJgun3Wia4MftVicW/1.json";

    #endregion
    
    #region Transfer

    private string contractTransfer = "0x358AA13c52544ECCEF6B0ADD0f801012ADAD5eE3";
    private string toAccountTransfer = "0xdD4c825203f97984e7867F11eeCc813A036089D1";
    private int tokenIdTransfer = 0;

    #endregion

    #endregion
    
    private Erc721 erc721;
    
    // Initializes the protocol class
    public void Awake()
    {
        erc721 = new Erc721(Web3Accessor.Web3);
    }
    
    /// <summary>
    /// All ERC 721 tokens belonging to an address
    /// </summary>
    public async void AllErc721()
    {
        var allNfts = await erc721.All(chainAllErc, networkAllErc, accountAllErc, contractAllErc, takeAllErc, skipAllErc);
        var output = string.Join(",\n", allNfts.Select(nft => $"{nft.TokenId} - {nft.Uri}"));
        SampleOutputUtil.PrintResult(output, nameof(Erc721), nameof(Erc721.All));
    }
    
    /// <summary>
    /// Balance Of ERC721 Address
    /// </summary>
    public async void BalanceOf()
    {
        var balance = await erc721.BalanceOf(contractBalanceOf, accountBalanceOf);
        SampleOutputUtil.PrintResult(balance.ToString(), nameof(Erc721), nameof(Erc721.BalanceOf));
    }
    
    /// <summary>
    /// Owner Of ERC721 tokens
    /// </summary>
    public async void OwnerOf()
    {
        var owner = await erc721.OwnerOf(contractOwnerOf, tokenIdOwnerOf);
        SampleOutputUtil.PrintResult(owner, nameof(Erc721), nameof(Erc721.OwnerOf));
    }
    
    /// <summary>
    /// Owner Of batch ERC721
    /// </summary>
    public async void OwnerOfBatch()
    {
        var owners = await erc721.OwnerOfBatch(contractOwnerOfBatch, tokenIdsOwnerOfBatch.ToArray(), multicallOwnerOfBatch);
        var ownersString = $"{owners.Count} owner(s):\n" + string.Join(",\n", owners);
        SampleOutputUtil.PrintResult(ownersString, nameof(Erc721), nameof(Erc721.OwnerOfBatch));
    }
    
    /// <summary>
    /// Uri Of ERC721 Address
    /// </summary>
    public async void Uri()
    {
        var uri = await erc721.Uri(contractUri, tokenIdUri);
        SampleOutputUtil.PrintResult(uri, nameof(Erc721), nameof(Erc721.Uri));
    }
    
    /// <summary>
    /// Mint ERC1155 tokens
    /// </summary>
    public async void MintErc721()
    {
        var response = await erc721.MintErc721(abiMint, contractMint, uriMint);
        var output = SampleOutputUtil.BuildOutputValue(response);
        SampleOutputUtil.PrintResult(output, nameof(Erc721), nameof(Erc721.MintErc721));
    }
    
    /// <summary>
    /// Transfer ERC1155 tokens
    /// </summary>
    public async void TransferErc721()
    {
        var response = await erc721.TransferErc721(contractTransfer, toAccountTransfer, tokenIdTransfer);
        var output = SampleOutputUtil.BuildOutputValue(response);
        SampleOutputUtil.PrintResult(output, nameof(Erc721), nameof(Erc721.TransferErc721));
    }
}