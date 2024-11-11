using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming;
using ChainSafe.Gaming.UnityPackage;
using Scripts.EVM.Token;
using UnityEngine;
using UnityEngine.UI;
using Erc1155Contract = ChainSafe.Gaming.Evm.Contracts.Custom.Erc1155Contract;

/// <summary>
/// ERC1155 calls used in the sample scene
/// </summary>
public class Erc1155Sample : MonoBehaviour, ISample
{
    #region Fields

    [field: SerializeField] public string Title { get; private set; }

    [field: SerializeField, TextArea] public string Description { get; private set; }

    public Type[] DependentServiceTypes => Array.Empty<Type>();

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
    public GameObject textureDisplayPrefab;

    #endregion

    #endregion

    private Erc1155Contract _erc1155;

    private GameObject _textureDisplay;

    private RawImage _rawImage;

    /// <summary>
    /// Balance Of ERC1155 Address
    /// </summary>
    public async Task<string> BalanceOf()
    {
        var balance = await Web3Unity.Web3.Erc1155.GetBalanceOf(ChainSafeContracts.Erc1155, tokenIdBalanceOf);

        return balance.ToString();
    }

    /// <summary>
    /// Balance Of batch ERC1155
    /// </summary>
    public async Task<string> BalanceOfBatch()
    {
        var balances = await Web3Unity.Web3.Erc1155.GetBalanceOfBatch(ChainSafeContracts.Erc1155,
            accountsBalanceOfBatch, tokenIdsBalanceOfBatch);

        return string.Join(",\n", balances.Select(o => o.ToString()));
    }

    /// <summary>
    /// Uri Of ERC1155 Address
    /// </summary>
    public async Task<string> Uri()
    {
        var uri = await Web3Unity.Web3.Erc1155.GetUri(ChainSafeContracts.Erc1155, tokenIdUri);
        
        return uri;
    }

    /// <summary>
    /// Mint ERC1155 tokens
    /// </summary>
    public async Task<string> MintErc1155()
    {
        var response = await Web3Unity.Web3.Erc1155.MintWithReceipt(ChainSafeContracts.Erc1155, idMint, amountMint);

        return response.TransactionHash;
    }

    /// <summary>
    /// Transfer ERC1155 tokens
    /// </summary>
    public async Task<string> TransferErc1155()
    {
        var response = await Web3Unity.Web3.Erc1155.TransferWithReceipt(ChainSafeContracts.Erc1155, tokenIdTransfer,
            amountTransfer, toAccountTransfer);

        return response.TransactionHash;
    }

    /// <summary>
    /// Imports an NFTs texture via URI data
    /// </summary>
    public async Task<string> ImportNftTexture1155()
    {
        var texture = await Web3Unity.Web3.Erc1155.ImportTexture(ChainSafeContracts.Erc1155, tokenIdTexture);

        if (_textureDisplay == null)
        {
            _textureDisplay = Instantiate(textureDisplayPrefab);

            _rawImage = _textureDisplay.GetComponentInChildren<RawImage>();
        }

        _textureDisplay.SetActive(true);

        _rawImage.texture = texture;

        return "Nft Texture Set.";
    }
}