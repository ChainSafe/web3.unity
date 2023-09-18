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
        private static readonly string hostVoucher = "https://lazy-minting-voucher-signer.herokuapp.com";
        public static async Task<string> Multicall(Web3 web3, string _chain, string _network, string _contract, string _abi, string _method, string _args, string _multicall = "", string _rpc = "")
        {
            WWWForm form = new WWWForm();
            form.AddField("projectId", web3.ProjectConfig.ProjectId);
            form.AddField("chain", _chain);
            form.AddField("network", _network);
            form.AddField("contract", _contract);
            form.AddField("abi", _abi);
            form.AddField("method", _method);
            form.AddField("args", _args);
            form.AddField("multicall", _multicall);
            form.AddField("rpc", _rpc);
            string url = host + "/multicall";
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
            {
                await webRequest.SendWebRequest();
                Response<string> data = JsonUtility.FromJson<Response<string>>(Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                return data.response;
            }
        }
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


        public static async Task<GetVoucherModel.GetVoucher721Response> Get721Voucher(Web3 web3)
        {
            string url = hostVoucher + "/voucher721?receiver=" + await web3.Signer.GetAddress();

            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                await webRequest.SendWebRequest();
                GetVoucherModel.GetVoucher721Response root721 = JsonConvert.DeserializeObject<GetVoucherModel.GetVoucher721Response>(webRequest.downloadHandler.text);
                return root721;
            }
        }

        public static async Task<GetVoucherModel.GetVoucher1155Response> Get1155Voucher(Web3 web3)
        {
            string url = hostVoucher + "/voucher1155?receiver=" + await web3.Signer.GetAddress();

            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                await webRequest.SendWebRequest();
                GetVoucherModel.GetVoucher1155Response root1155 = JsonConvert.DeserializeObject<GetVoucherModel.GetVoucher1155Response>(webRequest.downloadHandler.text);
                return root1155;
            }
        }

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


        public static async Task<TokenResponse[]> AllErc721(Web3 web3, string chain, string network, string account, string contract = "", int take = 500, int skip = 0)
        {
            WWWForm form = new WWWForm();
            form.AddField("projectId", web3.ProjectConfig.ProjectId);
            form.AddField("chain", chain);
            form.AddField("network", network);
            form.AddField("account", account);
            form.AddField("contract", contract);
            form.AddField("first", take);
            form.AddField("skip", skip);

            string url = host + "/all721";
            string rawNfts;
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
            {
                await webRequest.SendWebRequest();
                Response<string> data = JsonUtility.FromJson<Response<string>>(Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                rawNfts = data.response;
            }

            try
            {
                return JsonConvert.DeserializeObject<TokenResponse[]>(rawNfts);
            }
            catch (JsonException e)
            {
                throw new Web3Exception("NFTs deserialization failed.", e);
            }
        }

        public static async Task<TokenResponse[]> AllErc1155(Web3 web3, string chain, string network, string account, string contract = "", int take = 500, int skip = 0)
        {
            WWWForm form = new WWWForm();
            form.AddField("projectId", web3.ProjectConfig.ProjectId);
            form.AddField("chain", chain);
            form.AddField("network", network);
            form.AddField("account", account);
            form.AddField("contract", contract);
            form.AddField("first", take);
            form.AddField("skip", skip);
            string url = host + "/all1155";
            string rawNfts;
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
            {
                await webRequest.SendWebRequest();
                Response<string> data = JsonUtility.FromJson<Response<string>>(Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                rawNfts = data.response;
            }

            try
            {
                return JsonConvert.DeserializeObject<TokenResponse[]>(rawNfts);
            }
            catch (JsonException e)
            {
                throw new Web3Exception("NFTs deserialization failed.", e);
            }
        }
        public static async Task<RedeemVoucherTxModel.Response> CreateRedeemTransaction(Web3 web3, string chain, string network, string voucher, string type, string nftAddress, string account)
        {
            WWWForm form = new WWWForm();
            form.AddField("projectId", web3.ProjectConfig.ProjectId);
            form.AddField("chain", chain);
            form.AddField("network", network);
            form.AddField("voucher", voucher);
            form.AddField("type", type);
            form.AddField("nftAdrress", nftAddress);
            form.AddField("account", account);
            string url = host + "/createRedeemTransaction";
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
            {
                await webRequest.SendWebRequest();
                RedeemVoucherTxModel.Root data = JsonConvert.DeserializeObject<RedeemVoucherTxModel.Root>(Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                return data.response;
            }
        }
    }
}

