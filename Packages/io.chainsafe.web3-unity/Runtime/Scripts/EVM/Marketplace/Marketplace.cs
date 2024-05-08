using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Model;
using Scripts.EVM.Remote;
using UnityEngine;

namespace Scripts.EVM.Marketplace
{
    public class Marketplace
    {
        #region Methods
        
        #region MarketplaceCalls
        
        /// <summary>
        /// Gets all items in a project.
        /// Path: https://api.gaming.chainsafe.io/v1/projects/{projectID}/items
        /// </summary>
        public static async Task<MarketplaceModel.Root> GetProjectItems()
        {
            string path = $"/items?chainId={Web3Accessor.Web3.ChainConfig.ChainId}";
            var response = await CSServer.GetData<MarketplaceModel.Root>(path);
            return response;
        }
        
        /// <summary>
        /// Gets all items in a marketplace.
        /// Path: https://api.gaming.chainsafe.io/v1/projects/{projectID}/marketplaces/{marketplaceID}/items
        /// </summary>
        public static async Task<MarketplaceModel.Root> GetMarketplaceItems(string marketplaceId)
        {
            string path = $"/marketplaces/{marketplaceId}/items";
            var response = await CSServer.GetData<MarketplaceModel.Root>(path);
            return response;
        }

        /// <summary>
        /// Gets items listed by token id.
        /// Path: https://api.gaming.chainsafe.io/v1/projects/{projectID}/marketplaces/{marketplaceID}/items/{itemID}
        /// </summary>
        public static async Task<MarketplaceModel.Root> GetItem(string marketplaceId, string tokenId)
        {
            string path = $"/marketplaces/{marketplaceId}/items/{tokenId}";
            var response = await CSServer.GetData<MarketplaceModel.Root>(path);
            return response;
        }
        
        #endregion
        
        #region TokenCalls

        /// <summary>
        /// Gets all tokens in a project.
        /// Path: https://api.gaming.chainsafe.io/v1/projects/{projectID}/tokens
        /// </summary>
        public static async Task<NftTokenModel.Root> GetProjectTokens()
        {
            string path = $"/tokens?chainId={Web3Accessor.Web3.ChainConfig.ChainId}";
            var response = await CSServer.GetData<NftTokenModel.Root>(path);
            return response;
        }

        /// <summary>
        /// Gets all tokens in a 721 collection.
        /// Path: https://api.gaming.chainsafe.io/v1/projects/{projectID}/collections/{collectionID}/tokens
        /// </summary>
        public static async Task<NftTokenModel.Root> GetCollectionTokens721(string collectionId721)
        {
            string path = $"/collections/{collectionId721}/tokens";
            var response = await CSServer.GetData<NftTokenModel.Root>(path);
            return response;
        }

        /// <summary>
        /// Gets all tokens in a 1155 collection.
        /// Path https://api.gaming.chainsafe.io/v1/projects/{projectID}/collections/{collectionID}/tokens
        /// </summary>
        public static async Task<NftTokenModel.Root> GetCollectionTokens1155(string collectionId1155)
        {
            string path = $"/collections/{collectionId1155}/tokens";
            var response = await CSServer.GetData<NftTokenModel.Root>(path);
            return response;
        }

        /// <summary>
        /// Gets the information of a token in a collection via id.
        /// Path: https://api.gaming.chainsafe.io/v1/projects/{projectID}/collections/{collectionID}/tokens/:tokenID
        /// Token id is optional
        /// </summary>
        public static async Task<NftTokenModel.Token> GetCollectionToken(string collectionId, string tokenId)
        {
            string path = $"/collections/{collectionId}/tokens/{tokenId}";
            var response = await CSServer.GetData<NftTokenModel.Token>(path);
            return response;
        }

        /// <summary>
        /// Gets the owners of a token id in a collection.
        /// Path: https://api.gaming.chainsafe.io/v1/projects/{projectID}/collections/{collectionID}/tokens/{tokenID}/owners
        /// </summary>
        public static async Task<NftTokenModel.Token> GetTokenOwners(string collectionId, string tokenId)
        {
            string path = $"/collections/{collectionId}/tokens/{tokenId}/owners";
            var response = await CSServer.GetData<NftTokenModel.Token>(path);
            return response;
        }

        #region Utilities

        /// <summary>
        /// Prints json properties in the console on new lines.
        /// </summary>
        /// <param name="obj">The object to print out</param>
        public static void PrintObject(object obj)
        {
            var properties = obj.GetType().GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(obj);
                Debug.Log($"{property.Name}: {value}");
            }
        }

        #endregion
        
        #endregion
        
        #endregion
    }
}