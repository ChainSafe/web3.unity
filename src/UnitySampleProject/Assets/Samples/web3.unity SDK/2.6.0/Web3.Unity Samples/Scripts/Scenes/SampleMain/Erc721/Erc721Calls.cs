using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts.BuiltIn;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Connection;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core.Logout;
using Scripts.EVM.Token;
using UnityEngine;
using Erc721Contract = ChainSafe.Gaming.Evm.Contracts.Custom.Erc721Contract;

/// <summary>
/// ERC721 calls used in the sample scene
/// </summary>
public class Erc721Calls : Web3BuilderServiceAdapter, IWeb3InitializedHandler, ILogoutHandler
{
    #region Fields
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


    private Erc721Contract _erc721;

    /// <summary>
    /// Balance Of ERC721 Address
    /// </summary>
    public async void BalanceOf()
    {
        var balance = await _erc721.BalanceOf(accountBalanceOf);
        SampleOutputUtil.PrintResult(balance.ToString(), "ERC-721", nameof(Erc721Service.GetBalanceOf));
    }

    /// <summary>
    /// Owner Of ERC721 tokens
    /// </summary>
    public async void OwnerOf()
    {
        var owner = await _erc721.OwnerOf(BigInteger.Parse(tokenIdOwnerOf));
        SampleOutputUtil.PrintResult(owner, "ERC-721", nameof(Erc721Service.GetOwnerOf));
    }

    /// <summary>
    /// Owner Of batch ERC721
    /// </summary>
    public async void OwnerOfBatch()
    {
        var owners = await _erc721.GetOwnerOfBatch(tokenIdsOwnerOfBatch);
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
        SampleOutputUtil.PrintResult(ownersString.ToString(), "ERC-721", nameof(Erc721Service.GetOwnerOfBatch));
    }

    /// <summary>
    /// Uri Of ERC721 Address
    /// </summary>
    public async void Uri()
    {
        var uri = await _erc721.TokenURI(tokenIdUri);
        SampleOutputUtil.PrintResult(uri, "ERC-721", nameof(Erc721Service.GetUri));
    }

    /// <summary>
    /// Mint ERC1155 tokens
    /// </summary>
    public async void MintErc721()
    {
        var response = await _erc721.SafeMintWithReceipt(Web3Unity.Web3.Signer.PublicAddress, uriMint);
        var output = SampleOutputUtil.BuildOutputValue(new object[] {response.TransactionHash});
        SampleOutputUtil.PrintResult(output, "ERC-721", nameof(Erc721Service.GetUri));
    }

    /// <summary>
    /// Transfer ERC1155 tokens
    /// </summary>
    public async void TransferErc721()
    {
        var response = await _erc721.SafeTransferFromWithReceipt(contractTransfer, toAccountTransfer, tokenIdTransfer);
        var output = SampleOutputUtil.BuildOutputValue(new [] {response.TransactionHash});
        SampleOutputUtil.PrintResult(output, "ERC-721", nameof(Erc721Service.Transfer));
    }

    public async Task OnWeb3Initialized(Web3 web3)
    {
        _erc721 = await web3.ContractBuilder.Build<Erc721Contract>(ChainSafeContracts.Erc721);
    }
    
    public override Web3Builder ConfigureServices(Web3Builder web3Builder)
    {
        return web3Builder.Configure(services =>
        {
            services.AddSingleton<IWeb3InitializedHandler, ILogoutHandler, Erc721Calls>(_ => this);
        });
    }

    public async Task OnLogout()
    {
        await _erc721.DisposeAsync();
    }
}