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

    #region Collections
    
    [SerializeField] private string collectionId721 = "d588268b-8a5b-486a-8ea1-4122b510d71e";
    [SerializeField] private string collectionId1155 = "ebeaaee5-f7c2-4561-abb9-60ba749db7cd";
    [SerializeField] private string marketplaceId = "4986983b-2bcc-4bb3-b0db-a3448fbdee2b";
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
        Marketplace.PrintObject(response);
    }
    
    /// <summary>
    /// Gets all items in a marketplace.
    /// </summary>
    public async void GetMarketplaceItems()
    {
        var response = await Marketplace.GetMarketplaceItems(marketplaceId);
        Marketplace.PrintObject(response);
    }
    
    /// <summary>
    /// Gets items listed by token id.
    /// </summary>
    public async void GetItem()
    {
        var response = await Marketplace.GetItem(marketplaceId, tokenId);
        Marketplace.PrintObject(response);
    }
    #endregion
    
    #region TokenCalls
    
    /// <summary>
    /// Gets all tokens in a project.
    /// </summary>
    public async void GetProjectTokens()
    {
        var response = await Marketplace.GetProjectTokens();
        Marketplace.PrintObject(response);
    }
    
    /// <summary>
    /// Gets all tokens in a 721 collection.
    /// </summary>
    public async void GetCollectionTokens721()
    {
        var response = await Marketplace.GetCollectionTokens721(collectionId721);
        Marketplace.PrintObject(response);;
    }
    
    /// <summary>
    /// Gets all tokens in a 1155 collection.
    /// </summary>
    public async void GetCollectionTokens1155()
    {
        var response = await Marketplace.GetCollectionTokens1155(collectionId1155);
        Marketplace.PrintObject(response);
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
        Marketplace.PrintObject(response);
    }

    #endregion
    
    #endregion
}
