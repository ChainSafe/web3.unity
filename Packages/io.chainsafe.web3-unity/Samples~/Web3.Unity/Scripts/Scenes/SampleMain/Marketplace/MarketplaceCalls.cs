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
    
    #region Collections
    
    [Header("721 Collection Call")]
    [SerializeField] private string collectionId721 = "d588268b-8a5b-486a-8ea1-4122b510d71e";
    [Header("1155 Collection Call")]
    [SerializeField] private string collectionId1155 = "ebeaaee5-f7c2-4561-abb9-60ba749db7cd";
    [Header("Marketplace Calls")]
    [SerializeField] private string marketplaceId = "4986983b-2bcc-4bb3-b0db-a3448fbdee2b";
    [Header("Token Calls")]
    [SerializeField] private string tokenId = "0";

    #endregion
    
    #endregion

    #region Methods
    
    #region MarketplaceCalls
    
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
    #endregion
    
    #region TokenCalls
    
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

    #endregion
    
    #endregion
}
