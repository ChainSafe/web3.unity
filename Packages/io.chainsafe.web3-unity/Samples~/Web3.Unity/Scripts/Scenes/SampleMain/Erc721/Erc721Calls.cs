using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Model;
using Scripts.EVM.Token;
using UnityEngine;

/// <summary>
/// ERC721 calls used in the sample scene
/// </summary>
public class Erc721Calls : MonoBehaviour
{
    #region Fields

    #region Balance Of

    private string accountBalanceOf = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";

    #endregion

    #region Owner Of

    private string tokenIdOwnerOf = "1";

    #endregion

    #region Owner Of Batch

    private string[] tokenIdsOwnerOfBatch = { "4", "50", "6" };

    #endregion

    #region Uri

    private string tokenIdUri = "0x01559ae4021a565d5cc4740f1cefa95de8c1fb193949ecd32c337b03047da501";

    #endregion

    #region Mint

    private string uriMint = "1";

    #endregion

    #region Transfer

    private string contractTransfer = "0x358AA13c52544ECCEF6B0ADD0f801012ADAD5eE3";
    private string toAccountTransfer = "0xdD4c825203f97984e7867F11eeCc813A036089D1";
    private int tokenIdTransfer = 0;

    #endregion

    #endregion

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
        var contract = Web3Accessor.Web3.ContractBuilder.Build(ABI.Erc721, Contracts.Erc721);
        Debug.Log(contract.Calldata("ownerOf", new object[]{tokenIdOwnerOf}));
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
        StringBuilder ownersString = new StringBuilder();
        var dict = owners.GroupBy(x => x.Owner).ToDictionary(x => x.Key, x => x.Select(x => x.TokenId).ToList());
        foreach (var owner in dict)
        {
            ownersString.AppendLine($"Owner: {owner.Key} owns the following token(s):");
            foreach (var tokenId in owner.Value)
            {
                ownersString.AppendLine("\t" + tokenId);
            }
        }
        SampleOutputUtil.PrintResult(ownersString.ToString(), nameof(Erc721), nameof(Erc721.OwnerOfBatch));
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