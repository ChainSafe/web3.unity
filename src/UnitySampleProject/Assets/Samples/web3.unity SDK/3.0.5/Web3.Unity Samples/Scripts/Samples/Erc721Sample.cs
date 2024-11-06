using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming;
using ChainSafe.Gaming.Evm.Contracts.BuiltIn;
using ChainSafe.Gaming.UnityPackage;
using Scripts.EVM.Token;
using UnityEngine;
using StringBuilder = System.Text.StringBuilder;

/// <summary>
/// ERC721 calls used in the sample scene
/// </summary>
public class Erc721Sample : MonoBehaviour, ISample
{
    #region Fields

    [field: SerializeField] public string Title { get; private set; }

    [field: SerializeField, TextArea] public string Description { get; private set; }

    public Type[] DependentServiceTypes => Array.Empty<Type>();

    [Header("Change the fields below for testing purposes")]

    #region Balance Of

    [Header("Balance Of Call")]
    [SerializeField] private string accountBalanceOf = "0xd25b827D92b0fd656A1c829933e9b0b836d5C3e2";

    #endregion

    #region Owner Of

    [Header("Owner Of Call")]
    [SerializeField] private string tokenIdOwnerOf = "1";

    #endregion

    [Header("Balance Of Batch Call")]
    #region Owner Of Batch

    [SerializeField] private string[] tokenIdsOwnerOfBatch = { "4", "50", "6" };

    #endregion

    #region Uri

    [Header("URI Call")]
    [SerializeField] private string tokenIdUri = "0x01559ae4021a565d5cc4740f1cefa95de8c1fb193949ecd32c337b03047da501";

    #endregion

    #region Mint

    [Header("Mint Call")]
    [SerializeField] private string uriMint = "1";

    #endregion

    #region Transfer

    [Header("Transfer Call")]
    [SerializeField] private string contractTransfer = "0x358AA13c52544ECCEF6B0ADD0f801012ADAD5eE3";
    [SerializeField] private string toAccountTransfer = "0xdD4c825203f97984e7867F11eeCc813A036089D1";
    [SerializeField] private int tokenIdTransfer = 0;

    #endregion

    #endregion


    /// <summary>
    /// Balance Of ERC721 Address
    /// </summary>
    public async Task<string> BalanceOf()
    {
        var balance = await Web3Unity.Web3.Erc721.GetBalanceOf(ChainSafeContracts.Erc721, accountBalanceOf);

        return balance.ToString();
    }

    /// <summary>
    /// Owner Of ERC721 tokens
    /// </summary>
    public async Task<string> OwnerOf()
    {
        var owner = await Web3Unity.Web3.Erc721.GetOwnerOf(ChainSafeContracts.Erc721, tokenIdOwnerOf);

        return owner;
    }

    /// <summary>
    /// Owner Of batch ERC721
    /// </summary>
    public async Task<string> OwnerOfBatch()
    {
        var owners = await Web3Unity.Web3.Erc721.GetOwnerOfBatch(ChainSafeContracts.Erc721, tokenIdsOwnerOfBatch);

        return BuildOwnerOfBatchText(owners);
    }

    /// <summary>
    /// Uri Of ERC721 Address
    /// </summary>
    public async Task<string> Uri()
    {
        var uri = await Web3Unity.Web3.Erc721.GetUri(ChainSafeContracts.Erc721, tokenIdUri);
        return uri;
    }

    /// <summary>
    /// Mint ERC1155 tokens
    /// </summary>
    public async Task<string> MintErc721()
    {
        var response = await Web3Unity.Web3.Erc721.MintWithReceipt(ChainSafeContracts.Erc721, uriMint);

        return response.TransactionHash;
    }

    /// <summary>
    /// Transfer ERC1155 tokens
    /// </summary>
    public async Task<string> TransferErc721()
    {
        var response = await Web3Unity.Web3.Erc721.TransferWithReceipt(contractTransfer, toAccountTransfer, tokenIdTransfer);

        return response.TransactionHash;
    }

    private string BuildOwnerOfBatchText(IEnumerable<OwnerOfBatchModel> owners)
    {
        var ownersString = new StringBuilder();

        var dict = owners.GroupBy(x => x.Owner).ToDictionary(x => x.Key, x => x.Select(x => x.TokenId).ToList());

        foreach (var owner in dict)
        {
            ownersString.AppendLine($"Owner: {owner.Key} owns the following token(s):");

            foreach (var tokenId in owner.Value)
            {
                ownersString.AppendLine("\t" + tokenId);
            }
        }

        return ownersString.ToString();
    }
}