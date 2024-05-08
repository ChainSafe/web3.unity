using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Model;
using Scripts.EVM.Remote;
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
    /// Path: https://api.gaming.chainsafe.io/v1/projects/{projectID}/items
    /// </summary>
    public async void GetProjectItems()
    {
        string path = $"/items?chainId={Web3Accessor.Web3.ChainConfig.ChainId}";
        var response = await CSServer.GetData<MarketplaceModel.Root>(path);
        CSServer.PrintObject(response);
    }
    
    /// <summary>
    /// Gets all items in a marketplace.
    /// Path: https://api.gaming.chainsafe.io/v1/projects/{projectID}/marketplaces/{marketplaceID}/items
    /// </summary>
    public async void GetMarketplaceItems()
    {
        string path = $"/marketplaces/{marketplaceId}/items";
        var response = await CSServer.GetData<MarketplaceModel.Root>(path);
        CSServer.PrintObject(response);
    }
    
    /// <summary>
    /// Gets items listed by token id.
    /// Path: https://api.gaming.chainsafe.io/v1/projects/{projectID}/marketplaces/{marketplaceID}/items/{itemID}
    /// </summary>
    public async void GetItem()
    {
        string path = $"/marketplaces/{marketplaceId}/items/{tokenId}";
        var response = await CSServer.GetData<MarketplaceModel.Root>(path);
        CSServer.PrintObject(response);
    }
    #endregion
    
    #region TokenCalls
    
    /// <summary>
    /// Gets all tokens in a project.
    /// Path: https://api.gaming.chainsafe.io/v1/projects/{projectID}/tokens
    /// </summary>
    public async void GetProjectTokens()
    {
        string path = $"/tokens?chainId={Web3Accessor.Web3.ChainConfig.ChainId}";
        var response = await CSServer.GetData<NftTokenModel.Root>(path);
        CSServer.PrintObject(response);
    }
    
    /// <summary>
    /// Gets all tokens in a 721 collection.
    /// Path: https://api.gaming.chainsafe.io/v1/projects/{projectID}/collections/{collectionID}/tokens
    /// </summary>
    public async void GetCollectionTokens721()
    {
        string path = $"/collections/{collectionId721}/tokens";
        var response = await CSServer.GetData<NftTokenModel.Root>(path);
        CSServer.PrintObject(response);
    }
    
    /// <summary>
    /// Gets all tokens in a 1155 collection.
    /// Path https://api.gaming.chainsafe.io/v1/projects/{projectID}/collections/{collectionID}/tokens
    /// </summary>
    public async void GetCollectionTokens1155()
    {
        string path = $"/collections/{collectionId1155}/tokens";
        var response = await CSServer.GetData<NftTokenModel.Root>(path);
        CSServer.PrintObject(response);
    }
    
    /// <summary>
    /// Gets the information of a token in a collection via id.
    /// Path: https://api.gaming.chainsafe.io/v1/projects/{projectID}/collections/{collectionID}/tokens/:tokenID
    /// Token id is optional
    /// </summary>
    public async void GetCollectionToken()
    {
        string path = $"/collections/{collectionId721}/tokens/{tokenId}";
        var response = await CSServer.GetData<NftTokenModel.Token>(path);
        CSServer.PrintObject(response);
    }
    
    /// <summary>
    /// Gets the owners of a token id in a collection.
    /// Path: https://api.gaming.chainsafe.io/v1/projects/{projectID}/collections/{collectionID}/tokens/{tokenID}/owners
    /// </summary>
    public async void GetTokenOwners()
    {
        string path = $"/collections/{collectionId1155}/tokens/{tokenId}/owners";
        var response = await CSServer.GetData<NftTokenModel.Token>(path);
        CSServer.PrintObject(response);
    }

    #endregion
    
    #endregion
}
