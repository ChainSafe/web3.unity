using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Transactions;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.UnityPackage.Model;
using ChainSafe.Gaming.Web3;
using JetBrains.Annotations;
using Nethereum.Hex.HexTypes;
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
        /// Gets profile marketplaces.
        /// Path: https://api.gaming.chainsafe.io/v1/projects/{projectID}/marketplaces
        /// </summary>
        /// <param name="bearerToken">Bearer auth token.</param>
        /// <returns>MarketplaceModel.ProjectMarketplacesResponse.</returns>
        public static async Task<MarketplaceModel.ProjectMarketplacesResponse> GetProjectMarketplaces(string bearerToken)
        {
            var path = "/marketplaces";
            var response = await CSServer.GetDataWithToken<MarketplaceModel.ProjectMarketplacesResponse>(path, bearerToken);
            return response;
        }

        /// <summary>
        /// Gets project collections.
        /// Path: https://api.gaming.chainsafe.io/v1/projects/{projectID}/collections
        /// </summary>
        /// <param name="bearerToken">Bearer auth token.</param>
        /// <returns>NftTokenModel.ProjectCollectionsResponse.</returns>
        public static async Task<NftTokenModel.ProjectCollectionsResponse> GetProjectCollections(string bearerToken)
        {
            var path = "/collections";
            var response = await CSServer.GetDataWithToken<NftTokenModel.ProjectCollectionsResponse>(path, bearerToken);
            return response;
        }

        /// <summary>
        /// Gets all items in a project.
        /// Path: https://api.gaming.chainsafe.io/v1/projects/{projectID}/items
        /// </summary>
        /// <returns>MarketplaceModel.MarketplaceItemsResponse</returns>
        public static async Task<MarketplaceModel.MarketplaceItemsResponse> GetProjectItems()
        {
            var path = $"/items?chainId={Web3Unity.Web3.ChainConfig.ChainId}";
            var response = await CSServer.GetData<MarketplaceModel.MarketplaceItemsResponse>(path);
            return response;
        }

        /// <summary>
        /// Gets all items in a marketplace.
        /// Path: https://api.gaming.chainsafe.io/v1/projects/{projectID}/marketplaces/{marketplaceID}/items
        /// </summary>
        /// <param name="marketplaceId">MarketplaceID to query</param>
        /// <returns>MarketplaceModel.MarketplaceItemsResponse</returns>
        public static async Task<MarketplaceModel.MarketplaceItemsResponse> GetMarketplaceItems(string marketplaceId)
        {
            var path = $"/marketplaces/{marketplaceId}/items";
            var response = await CSServer.GetData<MarketplaceModel.MarketplaceItemsResponse>(path);
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
        /// <returns>NftTokenModel.CollectionItemsResponse</returns>
        public static async Task<NftTokenModel.CollectionItemsResponse> GetProjectTokens()
        {
            var path = $"/tokens?chainId={Web3Unity.Web3.ChainConfig.ChainId}";
            var response = await CSServer.GetData<NftTokenModel.CollectionItemsResponse>(path);
            return response;
        }

        /// <summary>
        /// Gets all tokens in a 721 collection.
        /// Path: https://api.gaming.chainsafe.io/v1/projects/{projectID}/collections/{collectionID}/tokens
        /// </summary>
        /// <param name="collectionId721">CollectionID721 to query</param>
        /// <returns>NftTokenModel.CollectionItemsResponse</returns>
        public static async Task<NftTokenModel.CollectionItemsResponse> GetCollectionTokens721(string collectionId721)
        {
            var path = $"/collections/{collectionId721}/tokens";
            var response = await CSServer.GetData<NftTokenModel.CollectionItemsResponse>(path);
            return response;
        }

        /// <summary>
        /// Gets all tokens in a 1155 collection.
        /// Path https://api.gaming.chainsafe.io/v1/projects/{projectID}/collections/{collectionID}/tokens
        /// </summary>
        /// <param name="collectionId1155">CollectionID1155 to query</param>
        /// <returns>NftTokenModel.CollectionItemsResponse</returns>
        public static async Task<NftTokenModel.CollectionItemsResponse> GetCollectionTokens1155(string collectionId1155)
        {
            var path = $"/collections/{collectionId1155}/tokens";
            var response = await CSServer.GetData<NftTokenModel.CollectionItemsResponse>(path);
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
        public static async Task<MarketplaceModel.MarketplaceItemsResponse> GetTokenOwners(string collectionId, string tokenId)
        {
            var path = $"/collections/{collectionId}/tokens/{tokenId}/owners";
            var response = await CSServer.GetData<MarketplaceModel.MarketplaceItemsResponse>(path);
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
        public static async Task<TransactionReceipt> Create721Collection(string _bearerToken, string _name, string _description, bool _isMintingPublic)
        {
            var logoImageData = await UploadPlatforms.GetImageData();
            var bannerImageData = await UploadPlatforms.GetImageData();
            var formData = new List<IMultipartFormSection>
                {
                    new MultipartFormDataSection("name", _name),
                    new MultipartFormDataSection("owner", Web3Unity.Web3.Signer.PublicAddress),
                    new MultipartFormDataSection("chain_id", Web3Unity.Web3.ChainConfig.ChainId),
                    new MultipartFormDataSection("projectID", Web3Unity.Web3.ProjectConfig.ProjectId),
                    new MultipartFormFileSection("logo", logoImageData, "logo.png", "image/png"),
                    new MultipartFormFileSection("banner", bannerImageData, "banner.png", "image/png"),
                    new MultipartFormDataSection("isImported", "true"),
                    new MultipartFormDataSection("contractAddress", ChainSafeContracts.MarketplaceContracts[Web3Unity.Web3.ChainConfig.ChainId]),
                    new MultipartFormDataSection("type", "ERC721")
                };
            if (!string.IsNullOrEmpty(_description))
            {
                formData.Insert(1, new MultipartFormDataSection("description", _description));
            }
            var path = "/collections";
            var collectionResponse = await CSServer.CreateData(_bearerToken, path, formData);
            var collectionData = JsonConvert.DeserializeObject<CollectionResponses.Collections>(collectionResponse);
            var method = "create721Collection";
            object[] args =
            {
                    Web3Unity.Web3.ProjectConfig.ProjectId,
                    collectionData.id,
                    _name,
                    collectionData.type,
                    collectionData.banner,
                    _isMintingPublic
                };
            var contract = Web3Unity.Web3.ContractBuilder.Build(ABI.MarketplaceFactory, ChainSafeContracts.MarketplaceContracts[Web3Unity.Web3.ChainConfig.ChainId]);
            var data = await contract.SendWithReceipt(method, args);
            return data.receipt;
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
        public static async Task<TransactionReceipt> Create1155Collection(string _bearerToken, string _name, string _description, bool _isMintingPublic)
        {
            try
            {
                var logoImageData = await UploadPlatforms.GetImageData();
                var bannerImageData = await UploadPlatforms.GetImageData();
                var formData = new List<IMultipartFormSection>
                {
                    new MultipartFormDataSection("name", _name),
                    new MultipartFormDataSection("owner", Web3Unity.Web3.Signer.PublicAddress),
                    new MultipartFormDataSection("chain_id", Web3Unity.Web3.ChainConfig.ChainId),
                    new MultipartFormDataSection("projectID", Web3Unity.Web3.ProjectConfig.ProjectId),
                    new MultipartFormFileSection("logo", logoImageData, "logo.png", "image/png"),
                    new MultipartFormFileSection("banner", bannerImageData, "banner.png", "image/png"),
                    new MultipartFormDataSection("isImported", "true"),
                    new MultipartFormDataSection("contractAddress", ChainSafeContracts.MarketplaceContracts[Web3Unity.Web3.ChainConfig.ChainId]),
                    new MultipartFormDataSection("type", "ERC1155")
                };
                if (!string.IsNullOrEmpty(_description))
                {
                    formData.Insert(1, new MultipartFormDataSection("description", _description));
                }
                var path = "/collections";
                var collectionResponse = await CSServer.CreateData(_bearerToken, path, formData);
                var collectionData = JsonConvert.DeserializeObject<CollectionResponses.Collections>(collectionResponse);
                var method = "create1155Collection";
                object[] args =
                {
                    Web3Unity.Web3.ProjectConfig.ProjectId,
                    collectionData.id,
                    collectionData.banner,
                    _isMintingPublic
                };
                var contract = Web3Unity.Web3.ContractBuilder.Build(ABI.MarketplaceFactory, ChainSafeContracts.MarketplaceContracts[Web3Unity.Web3.ChainConfig.ChainId]);
                var data = await contract.SendWithReceipt(method, args);
                return data.receipt;
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
        public static async Task<TransactionReceipt> Mint721CollectionNft(string _bearerToken, string _collectionContract, string _name, [CanBeNull] string _description)
        {
            try
            {
                var imageData = await UploadPlatforms.GetImageData();
                var formData = new List<IMultipartFormSection>
                {
                    new MultipartFormDataSection("name", _name),
                    new MultipartFormFileSection("image", imageData, "nftImage.png", "image/png"),
                    new MultipartFormDataSection("tokenType", "ERC721")
                    // TODO add attributes to form later
                    // attributes[0].trait_type: prop1
                    // attributes[0].value: 100
                };
                if (!string.IsNullOrEmpty(_description))
                {
                    formData.Insert(2, new MultipartFormDataSection("description", _description));
                }
                var path = "/nft?hash=blake2b-208";
                var collectionResponse = await CSServer.CreateData(_bearerToken, path, formData);
                var collectionData = JsonConvert.DeserializeObject<ApiResponse>(collectionResponse);
                Debug.Log($"CID: {collectionData.cid}");
                var method = "mint";
                object[] args =
                {
                    Web3Unity.Web3.Signer.PublicAddress,
                    collectionData.cid
                };
                var contract = Web3Unity.Web3.ContractBuilder.Build(ABI.GeneralErc721, _collectionContract);
                var txArgs = new TransactionRequest
                {
                    GasLimit = new HexBigInteger(90000)
                };
                var data = await contract.SendWithReceipt(method, args, txArgs);
                return data.receipt;
            }
            catch (Web3Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            catch (Exception e)
            {
                Debug.LogError("Error: " + e.Message);
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
        public static async Task<TransactionReceipt> Mint1155CollectionNft(string _bearerToken, string _collectionContract, string _amount, string _name, [CanBeNull] string _description)
        {
            try
            {
                var imageData = await UploadPlatforms.GetImageData();
                var formData = new List<IMultipartFormSection>
                {
                    new MultipartFormDataSection("name", _name),
                    new MultipartFormFileSection("image", imageData, "nftImage.png", "image/png"),
                    new MultipartFormDataSection("tokenType", "ERC1155")
                    // TODO add attributes to form later
                    // attributes[0].trait_type: prop1
                    // attributes[0].value: 100
                };
                if (!string.IsNullOrEmpty(_description))
                {
                    formData.Insert(1, new MultipartFormDataSection("description", _description));
                }
                var path = "/nft?hash=blake2b-208";
                var collectionResponse = await CSServer.CreateData(_bearerToken, path, formData);
                var collectionData = JsonConvert.DeserializeObject<ApiResponse>(collectionResponse);
                var method = "mint";
                var amount = BigInteger.Parse(_amount);
                Debug.Log($"Amount3: {amount}");
                object[] args =
                {
                    Web3Unity.Web3.Signer.PublicAddress,
                    collectionData.cid,
                    amount
                };

                var contract = Web3Unity.Web3.ContractBuilder.Build(ABI.GeneralErc1155, _collectionContract);
                var txArgs = new TransactionRequest
                {
                    GasLimit = new HexBigInteger(90000)
                };
                var data = await contract.SendWithReceipt(method, args, txArgs);
                return data.receipt;
            }
            catch (Web3Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            catch (Exception e)
            {
                Debug.LogError("Error: " + e.Message);
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
        public static async Task<TransactionReceipt> CreateMarketplace(string _bearerToken, string _name, string _description, bool _whitelisting)
        {
            try
            {
                var bannerImageData = await UploadPlatforms.GetImageData();
                var formData = new List<IMultipartFormSection>
                {
                    new MultipartFormDataSection("name", _name),
                    new MultipartFormDataSection("chain_id", Web3Unity.Web3.ChainConfig.ChainId),
                    new MultipartFormFileSection("banner", bannerImageData, "banner.png", "image/png")
                };
                if (!string.IsNullOrEmpty(_description))
                {
                    formData.Insert(1, new MultipartFormDataSection("description", _description));
                }
                var path = "/marketplaces";
                var marketplaceResponse = await CSServer.CreateData(_bearerToken, path, formData);
                Debug.Log(marketplaceResponse);
                var collectionData = JsonConvert.DeserializeObject<CollectionResponses.Marketplace>(marketplaceResponse);
                var method = "createMarketplace";
                object[] args =
                {
                    Web3Unity.Web3.ProjectConfig.ProjectId,
                    collectionData.id,
                    _whitelisting
                };
                var contract = Web3Unity.Web3.ContractBuilder.Build(ABI.MarketplaceFactory, ChainSafeContracts.MarketplaceContracts[Web3Unity.Web3.ChainConfig.ChainId]);
                var data = await contract.SendWithReceipt(method, args);
                return data.receipt;
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
        public static async Task<TransactionReceipt> SetApprovalMarketplace(string _nftContract, string _marketplaceContract, string _type, bool _permission)
        {
            try
            {
                var method = "setApprovalForAll";
                object[] args =
                {
                    _marketplaceContract,
                    _permission
                };
                var abi = _type == "ERC721" ? Token.ABI.GeneralErc721 : Token.ABI.GeneralErc1155;
                var contract = Web3Unity.Web3.ContractBuilder.Build(abi, _nftContract);
                var data = await contract.SendWithReceipt(method, args);
                return data.receipt;
            }
            catch (Web3Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Purchases NFT from the marketplace
        /// </summary>
        /// <param name="_marketplaceContract">The marketplace contract to purchase from</param>
        /// <param name="_itemId">The NFT id to purchase</param>
        /// <param name="_amountToSend">The amount to send in wei</param>
        /// <returns>Contract send data object</returns>
        public static async Task<TransactionReceipt> PurchaseNft(string _marketplaceContract, string _itemId, string _amountToSend)
        {
            try
            {
                var method = "purchaseItem";
                BigInteger itemId = BigInteger.Parse(_itemId);
                object[] args =
                {
                    itemId
                };
                var tx = new TransactionRequest
                {
                    Value = new HexBigInteger(BigInteger.Parse(_amountToSend).ToString("X"))
                };
                var contract = Web3Unity.Web3.ContractBuilder.Build(ABI.Marketplace, _marketplaceContract);
                var data = await contract.SendWithReceipt(method, args, tx);
                return data.receipt;
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
        public static async Task<TransactionReceipt> ListNftsToMarketplace(string _marketplaceContract, string _nftContract, string _tokenId, string _priceInWei)
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
                var contract = Web3Unity.Web3.ContractBuilder.Build(ABI.Marketplace, _marketplaceContract);
                var data = await contract.SendWithReceipt(method, args);
                return data.receipt;
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
        /// Handles CID response from API for metadata uploads.
        /// </summary>
        public class ApiResponse
        {
            public string cid { get; set; }
        }

        #endregion

        #endregion
    }
}