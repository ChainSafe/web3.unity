#if MARKETPLACE_AVAILABLE
using Scripts.EVM.Marketplace;
using UnityEngine;

/// <summary>
/// Marketplace sample calls for use with the api documentation.
/// Marketplace Api: https://docs.gaming.chainsafe.io/marketplace-api/docs/marketplaceapi
/// Token Api: https://docs.gaming.chainsafe.io/token-api/docs/tokenapi
/// </summary>
public class MarketplaceCalls : MonoBehaviour
{
    #region fields
    [Header("Change the fields below for testing purposes")]
    
    [Header("Bearer token")]
    [SerializeField] private string bearerToken = "Please set your bearer token from the ChainSafe dashboard";
    
    [Header("721 Collection Call")]
    [SerializeField] private string collectionId721 = "Set 721 collection ID";
    
    [Header("1155 Collection Call")]
    [SerializeField] private string collectionId1155 = "Set 1155 collection ID";
    
    [Header("Marketplace Calls")]
    [SerializeField] private string marketplaceId = "Set marketplace ID";
    
    [Header("Token Calls")]
    [SerializeField] private string tokenId = "Set token ID i.e 1";
    
    [Header("Create 721 Collection Call")]
    [SerializeField] private string collectionName721 = "Set 721 collection name";
    [SerializeField] private string collectionDescription721 = "Set 721 collection description";
    [SerializeField] private bool collectionMintingPublic721 = false;
    
    [Header("Create 1155 Collection Call")]
    [SerializeField] private string collectionName1155 = "Set 1155 collection name";
    [SerializeField] private string collectionDescription1155 = "Set 1155 collection description";
    [SerializeField] private bool collectionMintingPublic1155 = false;
    
    [Header("Delete calls (Can only be used before the item is on chain)")]
    [SerializeField] private string collectionToDelete = "Set collection to delete";
    [SerializeField] private string marketplaceToDelete = "Set marketplace to delete";
    
    [Header("Mint 721 to collection calls")]
    [SerializeField] private string collectionContract721 = "Set 721 collection to mint to";
    [SerializeField] private string uri721 = "Set metadata uri with full path i.e. https://ipfs.chainsafe.io/ipfs/bafyjvzacdj4apx52hvbyjkwyf7i6a7t3pcqd4kw4xxfc67hgvn3a";
    
    [Header("Mint 1155 to collection calls")]
    [SerializeField] private string collectionContract1155 = "Set 1155 collection to mint to";
    [SerializeField] private string uri1155 = "Set metadata uri with full path i.e. https://ipfs.chainsafe.io/ipfs/bafyjvzacdj4apx52hvbyjkwyf7i6a7t3pcqd4kw4xxfc67hgvn3a";
    [SerializeField] private string amount1155 = "Set amount of Nfts to mint i.e 1";
    
    [Header("Create marketplace call")]
    [SerializeField] private string marketplaceName = "Set marketplace name";
    [SerializeField] private string marketplaceDescription = "Set marketplace description";
    [SerializeField] private bool marketplaceWhitelisting = false;
    
    [Header("List to marketplace calls")]
    [SerializeField] private string tokenIdToList = "Set token ID to list";
    [SerializeField] private string weiPriceToList = "Set price in wei to list for i.e 100000000000000";
    [SerializeField] private string marketplaceContractToListTo = "Set marketplace contract to list to";
    [SerializeField] private string collectionContractToList = "Set collection contract to list from";
    
    [Header("List to marketplace calls")]
    [SerializeField] private string marketplaceContractToBuyFrom = "Set marketplace contract to buy from";
    [SerializeField] private string tokenIdToBuy = "Set token ID to buy";
    [SerializeField] private string weiPriceToBuy = "Set price in wei to buy with i.e 100000000000000";
    
    #endregion

    #region Methods
    
    /// <summary>
    /// Gets all items in a project.
    /// </summary>
    public async void GetProjectItems()
    {
        var response = await Marketplace.GetProjectItems();
        Debug.Log($"Total: {response.total}");
        foreach (var item in response.items)
        {
            Marketplace.PrintObject(item);
        }
    }
    
    /// <summary>
    /// Gets all items in a marketplace.
    /// </summary>
    public async void GetMarketplaceItems()
    {
        var response = await Marketplace.GetMarketplaceItems(marketplaceId);
        Debug.Log($"Total: {response.total}");
        foreach (var item in response.items)
        {
            Marketplace.PrintObject(item);
        }
    }
    
    /// <summary>
    /// Gets items listed by token id.
    /// </summary>
    public async void GetItem()
    {
        var response = await Marketplace.GetItem(marketplaceId, tokenId);
        Marketplace.PrintObject(response.token);
    }
    
    /// <summary>
    /// Gets all tokens in a project.
    /// </summary>
    public async void GetProjectTokens()
    {
        var response = await Marketplace.GetProjectTokens();
        foreach (var token in response.tokens)
        {
            Marketplace.PrintObject(token);
        }
    }
    
    /// <summary>
    /// Gets all tokens in a 721 collection.
    /// </summary>
    public async void GetCollectionTokens721()
    {
        var response = await Marketplace.GetCollectionTokens721(collectionId721);
        foreach (var token in response.tokens)
        {
            Marketplace.PrintObject(token);
        }
    }
    
    /// <summary>
    /// Gets all tokens in a 1155 collection.
    /// </summary>
    public async void GetCollectionTokens1155()
    {
        var response = await Marketplace.GetCollectionTokens1155(collectionId1155);
        foreach (var token in response.tokens)
        {
            Marketplace.PrintObject(token);
        }
    }
    
    /// <summary>
    /// Gets the information of a token in a collection via id. Token id is optional.
    /// </summary>
    public async void GetCollectionToken()
    {
        var response = await Marketplace.GetCollectionToken(collectionId721, tokenId);
        Marketplace.PrintObject(response);
    }
    
    /// <summary>
    /// Gets the owners of a token id in a collection.
    /// </summary>
    public async void GetTokenOwners()
    {
        var response = await Marketplace.GetTokenOwners(collectionId1155, tokenId);
        foreach (var owner in response.owners)
        {
            Marketplace.PrintObject(owner);
        }
    }
    
    /// <summary>
    /// Creates a 721 collection
    /// </summary>
    public async void Create721Collection()
    {
        var data = await Marketplace.Create721Collection(bearerToken, collectionName721, collectionDescription721, collectionMintingPublic721);
        var response = SampleOutputUtil.BuildOutputValue(data);
        Debug.Log($"TX: {response}");
    }
    
    /// <summary>
    /// Creates a 1155 collection
    /// </summary>
    public async void Create1155Collection()
    {
        var data = await Marketplace.Create1155Collection(bearerToken, collectionName1155, collectionDescription1155, collectionMintingPublic1155);
        var response = SampleOutputUtil.BuildOutputValue(data);
        Debug.Log($"TX: {response}");
    }
    
    /// <summary>
    /// Mints an NFT to a 721 collection
    /// </summary>
    public async void Mint721CollectionNft()
    {
        var data = await Marketplace.Mint721CollectionNft(collectionContract721, uri721);
        var response = SampleOutputUtil.BuildOutputValue(data);
        Debug.Log($"TX: {response}");
    }
    
    /// <summary>
    /// Mints an NFT to a 1155 collection
    /// </summary>
    public async void Mint1155CollectionNft()
    {
        var data = await Marketplace.Mint1155CollectionNft(collectionContract1155, uri1155, amount1155);
        var response = SampleOutputUtil.BuildOutputValue(data);
        Debug.Log($"TX: {response}");
    }
    
    /// <summary>
    /// Deletes a collection that isn't on chain yet
    /// </summary>
    public async void DeleteCollection()
    {
        var response = await Marketplace.DeleteCollection(bearerToken, collectionToDelete);
        Debug.Log(response);
    }
    
    /// <summary>
    /// Creates a marketplace
    /// </summary>
    public async void CreateMarketplace()
    {
        var data = await Marketplace.CreateMarketplace(bearerToken, marketplaceName, marketplaceDescription, marketplaceWhitelisting);
        var response = SampleOutputUtil.BuildOutputValue(data);
        Debug.Log($"TX: {response}");
    }
    
    /// <summary>
    /// Deletes a marketplace that isn't on chain yet
    /// </summary>
    public async void DeleteMarketplace()
    {
        var response = await Marketplace.DeleteMarketplace(bearerToken,marketplaceToDelete);
        Debug.Log(response);
    }
    
    /// <summary>
    /// Approves marketplace to list tokens
    /// </summary>
    public async void ApproveListNftsToMarketplace()
    {
        var data = await Marketplace.SetApprovalMarketplace(collectionContractToList, marketplaceContractToListTo, "1155",true);
        var response = SampleOutputUtil.BuildOutputValue(data);
        Debug.Log($"TX: {response}");
    }
    
    /// <summary>
    /// Revokes approval from marketplace to list tokens
    /// </summary>
    public async void RevokeApprovalListNftsToMarketplace()
    {
        var data = await Marketplace.SetApprovalMarketplace(collectionContractToList, marketplaceContractToListTo, "1155",false);
        var response = SampleOutputUtil.BuildOutputValue(data);
        Debug.Log($"TX: {response}");
    }
    
    /// <summary>
    /// Lists NFTs to the marketplace
    /// </summary>
    public async void ListNftsToMarketplace()
    {
        var data = await Marketplace.ListNftsToMarketplace(marketplaceContractToListTo,collectionContractToList, tokenIdToList, weiPriceToList);
        var response = SampleOutputUtil.BuildOutputValue(data);
        Debug.Log($"TX: {response}");
    }
    
    /// <summary>
    /// Purchases an Nft from the marketplace
    /// </summary>
    public async void PurchaseNftFromMarketplace()
    {
        var data = await Marketplace.PurchaseNft(marketplaceContractToBuyFrom, tokenIdToBuy, weiPriceToBuy);
        var response = SampleOutputUtil.BuildOutputValue(data);
        Debug.Log($"TX: {response}");
    }
    
    #endregion
}
#endif