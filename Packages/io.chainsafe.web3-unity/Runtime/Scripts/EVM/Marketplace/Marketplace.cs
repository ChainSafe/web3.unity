using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Model;
using ChainSafe.Gaming.Web3;
using Newtonsoft.Json;
using Scripts.EVM.Remote;
using Scripts.EVM.Token;
using UnityEngine;
using UnityEngine.Networking;

namespace Scripts.EVM.Marketplace
{
    public class Marketplace
    {
        #region Methods

        /// <summary>
        /// Gets all items in a project.
        /// Path: https://api.gaming.chainsafe.io/v1/projects/{projectID}/items
        /// </summary>
        /// <returns>MarketplaceModel.Root</returns>
        public static async Task<MarketplaceModel.Root> GetProjectItems()
        {
            var path = $"/items?chainId={Web3Accessor.Web3.ChainConfig.ChainId}";
            var response = await CSServer.GetData<MarketplaceModel.Root>(path);
            return response;
        }

        /// <summary>
        /// Gets all items in a marketplace.
        /// Path: https://api.gaming.chainsafe.io/v1/projects/{projectID}/marketplaces/{marketplaceID}/items
        /// </summary>
        /// <param name="marketplaceId">MarketplaceID to query</param>
        /// <returns>MarketplaceModel.Root</returns>
        public static async Task<MarketplaceModel.Root> GetMarketplaceItems(string marketplaceId)
        {
            var path = $"/marketplaces/{marketplaceId}/items";
            var response = await CSServer.GetData<MarketplaceModel.Root>(path);
            return response;
        }

        /// <summary>
        /// Gets items listed by token id.
        /// Path: https://api.gaming.chainsafe.io/v1/projects/{projectID}/marketplaces/{marketplaceID}/items/{itemID}
        /// </summary>
        /// <param name="marketplaceId">MarketplaceID to query</param>
        /// <param name="tokenId">TokenID to query</param>
        /// <returns>MarketplaceModel.Item</returns>
        public static async Task<MarketplaceModel.Item> GetItem(string marketplaceId, string tokenId)
        {
            var path = $"/marketplaces/{marketplaceId}/items/{tokenId}";
            var response = await CSServer.GetData<MarketplaceModel.Item>(path);
            return response;
        }
        
        /// <summary>
        /// Gets all tokens in a project.
        /// Path: https://api.gaming.chainsafe.io/v1/projects/{projectID}/tokens
        /// </summary>
        /// <returns>NftTokenModel.Root</returns>
        public static async Task<NftTokenModel.Root> GetProjectTokens()
        {
            var path = $"/tokens?chainId={Web3Accessor.Web3.ChainConfig.ChainId}";
            var response = await CSServer.GetData<NftTokenModel.Root>(path);
            return response;
        }

        /// <summary>
        /// Gets all tokens in a 721 collection.
        /// Path: https://api.gaming.chainsafe.io/v1/projects/{projectID}/collections/{collectionID}/tokens
        /// </summary>
        /// <param name="collectionId721">CollectionID721 to query</param>
        /// <returns>NftTokenModel.Root</returns>
        public static async Task<NftTokenModel.Root> GetCollectionTokens721(string collectionId721)
        {
            var path = $"/collections/{collectionId721}/tokens";
            var response = await CSServer.GetData<NftTokenModel.Root>(path);
            return response;
        }

        /// <summary>
        /// Gets all tokens in a 1155 collection.
        /// Path https://api.gaming.chainsafe.io/v1/projects/{projectID}/collections/{collectionID}/tokens
        /// </summary>
        /// <param name="collectionId1155">CollectionID1155 to query</param>
        /// <returns>NftTokenModel.Root</returns>
        public static async Task<NftTokenModel.Root> GetCollectionTokens1155(string collectionId1155)
        {
            var path = $"/collections/{collectionId1155}/tokens";
            var response = await CSServer.GetData<NftTokenModel.Root>(path);
            return response;
        }

        /// <summary>
        /// Gets the information of a token in a collection via id.
        /// Path: https://api.gaming.chainsafe.io/v1/projects/{projectID}/collections/{collectionID}/tokens/:tokenID
        /// Token id is optional
        /// </summary>
        /// <param name="collectionId">CollectionID to query</param>
        /// <param name="tokenId">TokenID to query</param>
        /// <returns>NftTokenModel.Token</returns>
        public static async Task<NftTokenModel.Token> GetCollectionToken(string collectionId, string tokenId)
        {
            var path = $"/collections/{collectionId}/tokens/{tokenId}";
            var response = await CSServer.GetData<NftTokenModel.Token>(path);
            return response;
        }

        /// <summary>
        /// Gets the owners of a token id in a collection.
        /// Path: https://api.gaming.chainsafe.io/v1/projects/{projectID}/collections/{collectionID}/tokens/{tokenID}/owners
        /// </summary>
        /// <param name="collectionId">CollectionID to query</param>
        /// <param name="tokenId">TokenID to query</param>
        /// <returns>NftTokenModel.Token</returns>
        public static async Task<MarketplaceModel.Root> GetTokenOwners(string collectionId, string tokenId)
        {
            var path = $"/collections/{collectionId}/tokens/{tokenId}/owners";
            var response = await CSServer.GetData<MarketplaceModel.Root>(path);
            return response;
        }
        
        /// <summary>
        /// Creates a 721 collection
        /// /// Path https://api.gaming.chainsafe.io/v1/projects/8524f420-ecd1-4cfd-a651-706ade97cac7/collections
        /// </summary>
        /// <param name="_bearerToken">Bearer token to access dashboard services</param>
        /// <param name="_name">Name of the 721 collection being created</param>
        /// <param name="_description">Description of the 721 collection being created</param>
        /// <param name="_isMintingPublic">If minting is public or not</param>
        /// <returns>Contract send data object</returns>
        public static async Task<object[]> Create721Collection(string _bearerToken, string _name, string _description, bool _isMintingPublic)
        {
                var logoImageData = await GetImageData();
                var bannerImageData = await GetImageData();
                var formData = new List<IMultipartFormSection>
                {
                    new MultipartFormDataSection("name", _name),
                    new MultipartFormDataSection("description", _description),
                    new MultipartFormDataSection("owner", Web3Accessor.Web3.Signer.PublicAddress),
                    new MultipartFormDataSection("chain_id", Web3Accessor.Web3.ChainConfig.ChainId),
                    new MultipartFormDataSection("projectID", Web3Accessor.Web3.ProjectConfig.ProjectId),
                    new MultipartFormFileSection("logo", logoImageData, "logo.png", "image/png"),
                    new MultipartFormFileSection("banner", bannerImageData, "banner.png", "image/png"),
                    new MultipartFormDataSection("isImported", "true"),
                    new MultipartFormDataSection("contractAddress", Token.Contracts.MarketplaceContracts[Web3Accessor.Web3.ChainConfig.ChainId]),
                    new MultipartFormDataSection("type", "erc721")
                };
                var path = "/collections";
                var collectionResponse = await CSServer.CreateData(_bearerToken, path, formData);
                var collectionData = JsonConvert.DeserializeObject<CollectionResponses.Collections>(collectionResponse);
                var method = "create721Collection";
                object[] args =
                {
                    Web3Accessor.Web3.ProjectConfig.ProjectId,
                    collectionData.id,
                    _name,
                    collectionData.type,
                    collectionData.banner,
                    _isMintingPublic
                };
                var data = await Evm.ContractSend(Web3Accessor.Web3, method, Token.ABI.MarketplaceFactory, Token.Contracts.MarketplaceContracts["11155111"], args);
                return data;
        }
        
        /// <summary>
        /// Creates a 1155 collection
        /// Path https://api.gaming.chainsafe.io/v1/projects/8524f420-ecd1-4cfd-a651-706ade97cac7/collections/
        /// </summary>
        /// <param name="_bearerToken">Bearer token to access dashboard services</param>
        /// <param name="_name">Name of the 1155 collection being created</param>
        /// <param name="_description">Description of the 1155 collection being created</param>
        /// <param name="_isMintingPublic">If minting is public or not</param>
        /// <returns>Contract send data object</returns>
        public static async Task<object[]> Create1155Collection(string _bearerToken, string _name, string _description, bool _isMintingPublic)
        {
            try
            {
                var logoImageData = await GetImageData();
                var bannerImageData = await GetImageData();
                var formData = new List<IMultipartFormSection>
                {
                    new MultipartFormDataSection("name", _name),
                    new MultipartFormDataSection("description", _description),
                    new MultipartFormDataSection("owner", Web3Accessor.Web3.Signer.PublicAddress),
                    new MultipartFormDataSection("chain_id", Web3Accessor.Web3.ChainConfig.ChainId),
                    new MultipartFormDataSection("projectID", Web3Accessor.Web3.ProjectConfig.ProjectId),
                    new MultipartFormFileSection("logo", logoImageData, "logo.png", "image/png"),
                    new MultipartFormFileSection("banner", bannerImageData, "banner.png", "image/png"),
                    new MultipartFormDataSection("isImported", "true"),
                    new MultipartFormDataSection("contractAddress", Token.Contracts.MarketplaceContracts[Web3Accessor.Web3.ChainConfig.ChainId]),
                    new MultipartFormDataSection("type", "erc1155")
                };
                var path = "/collections";
                var collectionResponse = await CSServer.CreateData(_bearerToken, path, formData);
                var collectionData = JsonConvert.DeserializeObject<CollectionResponses.Collections>(collectionResponse);
                var method = "create1155Collection";
                object[] args =
                {
                    Web3Accessor.Web3.ProjectConfig.ProjectId,
                    collectionData.id,
                    collectionData.banner,
                    _isMintingPublic
                };
                var data = await Evm.ContractSend(Web3Accessor.Web3, method, Token.ABI.MarketplaceFactory, Token.Contracts.MarketplaceContracts["11155111"], args);
                return data;
            }
            catch (Web3Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        /// <summary>
        /// /// Mints a 721 collection nft to the collection
        /// </summary>
        /// <param name="_collectionContract">721 collection contract to mint from/to</param>
        /// <param name="_uri">URI in full format i.e https://ipfs.chainsafe.io/ipfs/bafyjvzacdj4apx52hvbyjkwyf7i6a7t3pcqd4kw4xxfc67hgvn3a</param>
        /// <returns>Contract send data object</returns>
        public static async Task<object[]> Mint721CollectionNft(string _collectionContract, string _uri)
        {
            try
            {
                var method = "mint";
                object[] args =
                {
                    Web3Accessor.Web3.Signer.PublicAddress,
                    _uri
                };
                var data = await Evm.ContractSend(Web3Accessor.Web3, method, Token.ABI.GeneralErc721, _collectionContract, args);
                return data;
            }
            catch (Web3Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        /// <summary>
        /// Mints a 1155 collection nft to the collection
        /// </summary>
        /// <param name="_collectionContract">1155 collection contract to mint from/to</param>
        /// <param name="_uri">URI in full format i.e https://ipfs.chainsafe.io/ipfs/bafyjvzacdj4apx52hvbyjkwyf7i6a7t3pcqd4kw4xxfc67hgvn3a</param>
        /// <param name="_amount">Amount of Nfts to mint</param>
        /// <returns>Contract send data object</returns>
        public static async Task<object[]> Mint1155CollectionNft(string _collectionContract, string _uri, string _amount)
        {
            try
            {
                var method = "mint";
                var amount = BigInteger.Parse(_amount);
                object[] args =
                {
                    Web3Accessor.Web3.Signer.PublicAddress,
                    _uri,
                    amount
                };
                var data = await Evm.ContractSend(Web3Accessor.Web3, method, Token.ABI.GeneralErc1155, _collectionContract, args);
                return data;
            }
            catch (Web3Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        /// <summary>
        /// Deletes a collection that isn't on chain yet by ID
        /// Path https://api.gaming.chainsafe.io/v1/projects/8524f420-ecd1-4cfd-a651-706ade97cac7/collections/e38e9465-fb9b-4316-8d1d-c77e81b50d6a
        /// </summary>
        /// <param name="_bearerToken">Bearer token to access dashboard services</param>
        /// <param name="_collectionId">Collection ID to delete</param>
        /// <returns>Server response</returns>
        public static async Task<string> DeleteCollection(string _bearerToken, string _collectionId)
        {
            var path = $"/collections/{_collectionId}";
            var response = await CSServer.DeleteData(_bearerToken, path);
            return response;
        }
        
        /// <summary>
        /// Creates a marketplace
        /// Path: https://api.gaming.chainsafe.io/v1/projects/8524f420-ecd1-4cfd-a651-706ade97cac7/marketplaces
        /// </summary>
        /// <param name="_bearerToken">Bearer token to access dashboard services</param>
        /// <param name="_name">Marketplace name</param>
        /// <param name="_description">Marketplace description</param>
        /// <param name="_whitelisting">If whitelisting is enabled or not</param>
        /// <returns>Contract send data object</returns>
        public static async Task<object[]> CreateMarketplace(string _bearerToken, string _name, string _description, bool _whitelisting)
        {
            try
            {
                var bannerImageData = await GetImageData();
                var formData = new List<IMultipartFormSection>
                {
                    new MultipartFormDataSection("name", _name),
                    new MultipartFormDataSection("description", _description),
                    new MultipartFormDataSection("chain_id", Web3Accessor.Web3.ChainConfig.ChainId),
                    new MultipartFormFileSection("banner", bannerImageData, "banner.png", "image/png")
                };
                var path = "/marketplaces";
                var marketplaceResponse = await CSServer.CreateData(_bearerToken, path, formData);
                Debug.Log(marketplaceResponse);
                var collectionData = JsonConvert.DeserializeObject<CollectionResponses.Marketplace>(marketplaceResponse);
                var method = "createMarketplace";
                object[] args =
                {
                    Web3Accessor.Web3.ProjectConfig.ProjectId,
                    collectionData.id,
                    _whitelisting
                };
                var data = await Evm.ContractSend(Web3Accessor.Web3, method, Token.ABI.MarketplaceFactory, Token.Contracts.MarketplaceContracts["11155111"], args);
                return data;
            }
            catch (Web3Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        /// <summary>
        /// Deletes a marketplace that isn't on chain yet by ID
        /// Path: https://api.gaming.chainsafe.io/v1/projects/8524f420-ecd1-4cfd-a651-706ade97cac7/marketplaces/{marketplaceId}
        /// </summary>
        /// <param name="_bearerToken">Bearer token to access dashboard services</param>
        /// <param name="_marketplaceId">Marketplace ID to delete</param>
        /// <returns>Server response</returns>
        public static async Task<string> DeleteMarketplace(string _bearerToken, string _marketplaceId)
        {
            var path = $"/marketplaces/{_marketplaceId}";
            var response = await CSServer.DeleteData(_bearerToken, path);
            return response;
        }
        
        /// <summary>
        /// Approves the marketplace to list 721 Nfts
        /// </summary>
        /// <param name="_nftContract">Nft contract to approve</param>
        /// <param name="_marketplaceContract">Marketplace to approve</param>
        /// <param name="_permission">Permission being granted</param>
        /// <returns>Contract send data object</returns>
        public static async Task<object[]> SetApprovalMarketplace(string _nftContract, string _marketplaceContract, string _type, bool _permission)
        {
            try
            {
                var method = "setApprovalForAll";
                object[] args =
                {
                    _marketplaceContract,
                    _permission
                };
                var abi = _type == "721" ? Token.ABI.GeneralErc721 : Token.ABI.GeneralErc1155;
                var data = await Evm.ContractSend(Web3Accessor.Web3, method, abi, _nftContract, args);
                return data;
            }
            catch (Web3Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        /// <summary>
        /// Lists Nfts to the marketplace
        /// </summary>
        /// <param name="_marketplaceContract">Marketplace contract to list to</param>
        /// <param name="_nftContract">Nft contract to list from</param>
        /// <param name="_tokenId">Toked ID to list</param>
        /// <param name="_price">Price in wei to list for</param>
        /// <returns>Contract send data object</returns>
        public static async Task<object[]> ListNftsToMarketplace(string _marketplaceContract, string _nftContract, string _tokenId, string _priceInWei)
        {
            try
            {
                var method = "listItem";
                BigInteger priceInWei = BigInteger.Parse(_priceInWei);
                BigInteger tokenId = BigInteger.Parse(_tokenId);
                BigInteger deadline = DateTimeOffset.UtcNow.AddDays(7).ToUnixTimeSeconds();
                object[] args =
                {
                    _nftContract,
                    tokenId,
                    priceInWei,
                    deadline
                };
                var data = await Evm.ContractSend(Web3Accessor.Web3, method, Token.ABI.Marketplace, _marketplaceContract, args);
                return data;
            }
            catch (Web3Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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
        
        /// <summary>
        /// Gets the binary data of a png image.
        /// </summary>
        /// <returns>Byte array of image data</returns>
        private static async Task<byte[]> GetImageData()
        {
            #if UNITY_WEBGL && !UNITY_EDITOR
            var imageData = await CSServer.UploadImageWebGL();
            #else
            var imagePath = UnityEditor.EditorUtility.OpenFilePanel("Select Image", "", "png,jpg,jpeg,gif");
            while (string.IsNullOrEmpty(imagePath)) return null;
            UnityWebRequest www = UnityWebRequestTexture.GetTexture("file://" + imagePath);
            await www.SendWebRequest();
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            var imageData = texture.EncodeToPNG();
            #endif
            return imageData;
        }

        #endregion
        
        #endregion
    }
}