using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage.Model;
using ChainSafe.Gaming.Web3;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Scripts.EVM.Token;

namespace Scripts.EVM.Remote
{
    // todo: should we take chain and network from the Web3 object as well?
    public class CSServer
    {
        public class Response<T> { public T response; }
        private static readonly string host = "https://api.gaming.chainsafe.io/evm";

        /// <summary>
        /// Creates a mint transaction for an NFT (Non-Fungible Token) on a specified blockchain network.
        /// </summary>
        /// <param name="web3">The Web3 instance used for the transaction.</param>
        /// <param name="_chain">The blockchain chain identifier (e.g., Ethereum).</param>
        /// <param name="_network">The network identifier (e.g., Mainnet, Ropsten).</param>
        /// <param name="_account">The sender's account address initiating the mint transaction.</param>
        /// <param name="_to">The recipient's account address for the minted NFT.</param>
        /// <param name="_cid">The identifier or content reference for the NFT.</param>
        /// <param name="_type">The type or category of the NFT being minted.</param>
        /// <returns>A response with the mint transaction object.</returns>
        public static async Task<CreateMintModel.Response> CreateMint(Web3 web3, string _chain, string _network, string _account, string _to, string _cid, string _type)
        {
            WWWForm form = new WWWForm();
            form.AddField("projectId", web3.ProjectConfig.ProjectId);
            form.AddField("chain", _chain);
            form.AddField("network", _network);
            form.AddField("account", _account);
            form.AddField("to", _to);
            form.AddField("cid", _cid);
            form.AddField("type", _type);
            string url = host + "/createMintNFTTransaction";
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
            {
                await webRequest.SendWebRequest();
                CreateMintModel.Root data = JsonUtility.FromJson<CreateMintModel.Root>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                return data.response;
            }
        }

        /// <summary>
        /// Retrieves the NFT collection associated with the specified account.
        /// </summary>
        /// <returns>A string containing collection details.</returns>
        public static async Task<string> GetNftCollectionByAcc(Web3 web3, string _account)
        {
            WWWForm form = new WWWForm();
            form.AddField("projectId", web3.ProjectConfig.ProjectId);
            form.AddField("account", _account);
            string url = host + "/collection/getByAccount";
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
            {
                await webRequest.SendWebRequest();
                string data = webRequest.downloadHandler.text;
                return data;
            }
        }

        /// <summary>
        /// Retrieves the NFT collection details by its unique slug.
        /// </summary>
        /// <param name="web3">The Web3 instance used for the request.</param>
        /// <param name="_slug">The unique slug of the NFT collection.</param>
        /// <returns>A string containing collection details.</returns>
        public static async Task<string> GetNftCollectionBySlug(Web3 web3, string _slug)
        {
            WWWForm form = new WWWForm();
            form.AddField("projectId", web3.ProjectConfig.ProjectId);
            form.AddField("slug", _slug);
            string url = host + "/collection/get";
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
            {
                await webRequest.SendWebRequest();
                string data = webRequest.downloadHandler.text;
                return data;
            }
        }

        /// <param name="web3">The Web3 instance used for the request.</param>
        /// <param name="_account">The account address making the request.</param>
        /// <param name="_chain">The blockchain chain identifier (e.g., Ethereum).</param>
        /// <param name="_network">The network identifier (e.g., Mainnet, Ropsten).</param>
        /// <param name="_nftContract">The address of the NFT contract containing the token.</param>
        /// <param name="_tokenId">The unique identifier of the NFT token to retrieve data for.</param>
        /// <returns>A string containing the NFT data.</returns>
        public static async Task<string> GetNft(Web3 web3, string _account, string _chain, string _network, string _nftContract, string _tokenId)
        {
            WWWForm form = new WWWForm();
            form.AddField("projectId", web3.ProjectConfig.ProjectId);
            form.AddField("account", _account);
            form.AddField("chain", _chain);
            form.AddField("network", _network);
            form.AddField("nftContract", _nftContract);
            form.AddField("tokenId", _tokenId);
            string url = host + "/getNftData";
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
            {
                await webRequest.SendWebRequest();
                string data = webRequest.downloadHandler.text;
                return data;
            }
        }

        /// <summary>
        /// Retrieves a list of NFTs from the market.
        /// </summary>
        /// <param name="web3">The Web3 instance used for the request.</param>
        /// <param name="_chain">The blockchain chain identifier (e.g., Ethereum).</param>
        /// <param name="_network">The network identifier (e.g., Mainnet, Ropsten).</param>
        /// <returns>A list of response objects with the NFTs details.</returns>
        public static async Task<List<GetNftListModel.Response>> GetNftMarket(Web3 web3, string _chain, string _network)
        {
            WWWForm form = new WWWForm();
            form.AddField("projectId", web3.ProjectConfig.ProjectId);
            form.AddField("chain", _chain);
            form.AddField("network", _network);
            string url = host + "/getListedNfts";
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
            {
                await webRequest.SendWebRequest();
                GetNftListModel.Root data = JsonUtility.FromJson<GetNftListModel.Root>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                return data.response;
            }
        }

        /// <summary>
        /// Retrieves a list of minted NFTs.
        /// </summary>
        /// <param name="web3">The Web3 instance used for the request.</param>
        /// <param name="_chain">The blockchain chain identifier (e.g., Ethereum).</param>
        /// <param name="_network">The network identifier (e.g., Mainnet, Ropsten).</param>
        /// <param name="_account">The account address for which to retrieve minted NFTs.</param>
        /// <returns>A list of response objects containing details of the minted NFTs.</returns>
        /// <returns>A list of response objects with the minted NFTs details.</returns>
        public static async Task<List<MintedNFT.Response>> GetMintedNFT(Web3 web3, string _chain, string _network, string _account)
        {
            WWWForm form = new WWWForm();
            form.AddField("projectId", web3.ProjectConfig.ProjectId);
            form.AddField("chain", _chain);
            form.AddField("network", _network);
            form.AddField("account", _account);
            string url = host + "/getMintedNfts";
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
            {
                await webRequest.SendWebRequest();
                MintedNFT.Root data = JsonUtility.FromJson<MintedNFT.Root>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                return data.response;
            }
        }

        /// <summary>
        /// Creates a purchase transaction for an NFT.
        /// </summary>
        /// <param name="web3">The Web3 instance used for the transaction.</param>
        /// <param name="_chain">The blockchain chain identifier (e.g., Ethereum).</param>
        /// <param name="_network">The network identifier (e.g., Mainnet, Ropsten).</param>
        /// <param name="_account">The buyer's account address initiating the purchase transaction.</param>
        /// <param name="_itemId">The identifier of the NFT to be purchased.</param>
        /// <param name="_price">The price of the NFT in a suitable format.</param>
        /// <param name="_tokenType">The type of token used for payment (e.g., cryptocurrency).</param>
        /// <returns>A response object with the transaction details.</returns>
        public static async Task<BuyNFT.Response> CreatePurchaseNftTransaction(Web3 web3, string _chain, string _network, string _account, string _itemId, string _price, string _tokenType)
        {
            WWWForm form = new WWWForm();
            form.AddField("projectId", web3.ProjectConfig.ProjectId);
            form.AddField("chain", _chain);
            form.AddField("network", _network);
            form.AddField("account", _account);
            form.AddField("itemId", _itemId);
            form.AddField("price", _price);
            form.AddField("tokenType", _tokenType);

            string url = host + "/createPurchaseNftTransaction";
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
            {
                await webRequest.SendWebRequest();
                BuyNFT.Root data = JsonUtility.FromJson<BuyNFT.Root>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                return data.response;
            }
        }

        /// <summary>
        /// Lists an NFT for sale.
        /// </summary>
        /// <param name="web3">The Web3 instance used for the transaction.</param>
        /// <param name="_chain">The blockchain chain identifier (e.g., Ethereum).</param>
        /// <param name="_network">The network identifier (e.g., Mainnet, Ropsten).</param>
        /// <param name="_account">The seller's account address listing the NFT.</param>
        /// <param name="_tokenId">The unique identifier of the NFT to be listed.</param>
        /// <param name="_priceHex">The price of the NFT encoded in hexadecimal format.</param>
        /// <param name="_tokenType">The type of token used for payment (e.g., cryptocurrency).</param>
        /// <returns>A response object with the transaction details.</returns>
        public static async Task<ListNFT.Response> CreateListNftTransaction(Web3 web3, string _chain, string _network, string _account, string _tokenId, string _priceHex, string _tokenType)
        {
            WWWForm form = new WWWForm();
            form.AddField("projectId", web3.ProjectConfig.ProjectId);
            form.AddField("chain", _chain);
            form.AddField("network", _network);
            form.AddField("account", _account);
            form.AddField("tokenId", _tokenId);
            form.AddField("priceHex", _priceHex);
            form.AddField("tokenType", _tokenType);
            string url = host + "/createListNftTransaction";
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
            {
                await webRequest.SendWebRequest();
                ListNFT.Root data = JsonUtility.FromJson<ListNFT.Root>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                return data.response;
            }
        }

        /// <summary>
        /// Cancels an NFT listing.
        /// </summary>
        /// <param name="web3">The Web3 instance used for the transaction.</param>
        /// <param name="_chain">The blockchain chain identifier (e.g., Ethereum).</param>
        /// <param name="_network">The network identifier (e.g., Mainnet, Ropsten).</param>
        /// <param name="_account">The seller's account address canceling the NFT listing.</param>
        /// <param name="_itemId">The identifier of the NFT listing to be canceled.</param>
        /// <returns>A list of response objects with the NFTs details.</returns>
        public static async Task<List<GetNftListModel.Response>> CreateCancelNftTransaction(Web3 web3, string _chain, string _network, string _account, string _itemId)
        {
            WWWForm form = new WWWForm();
            form.AddField("projectId", web3.ProjectConfig.ProjectId);
            form.AddField("chain", _chain);
            form.AddField("network", _network);
            form.AddField("account", _account);
            form.AddField("itemId", _itemId);
            string url = host + "/createCancelNftTransaction";
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
            {
                await webRequest.SendWebRequest();
                GetNftListModel.Root data = JsonUtility.FromJson<GetNftListModel.Root>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                return data.response;
            }
        }

        /// <summary>
        /// Creates an approval transaction for a specific token type.
        /// </summary>
        /// <param name="web3">The Web3 instance used for the transaction.</param>
        /// <param name="chain">The blockchain chain identifier (e.g., Ethereum).</param>
        /// <param name="network">The network identifier (e.g., Mainnet, Ropsten).</param>
        /// <param name="account">The account address initiating the approval transaction.</param>
        /// <param name="tokenType">The type of token for which approval is being granted.</param>
        /// <returns>A response object containing information about the approval transaction.</returns>
        public static async Task<CreateApprovalModel.Response> CreateApproveTransaction(Web3 web3, string chain, string network, string account, string tokenType)
        {
            WWWForm form = new WWWForm();
            form.AddField("projectId", web3.ProjectConfig.ProjectId);
            form.AddField("chain", chain);
            form.AddField("network", network);
            form.AddField("account", account);
            form.AddField("tokenType", tokenType);

            string url = host + "/createApproveTransaction";
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
            {
                await webRequest.SendWebRequest();
                CreateApprovalModel.Root data = JsonUtility.FromJson<CreateApprovalModel.Root>(Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                return data.response;
            }
        }
    }
}

