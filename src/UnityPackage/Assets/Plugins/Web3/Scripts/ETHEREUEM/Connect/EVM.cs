using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Web3Unity.Scripts.Library.Ethers.Contracts;

namespace Web3Unity.Scripts.Library.ETHEREUEM.Connect
{
    public class EVM
    {
        public class Response<T> { public T response; }
        private static readonly string host = "https://api.gaming.chainsafe.io/evm";
        private static readonly string hostVoucher = "https://lazy-minting-voucher-signer.herokuapp.com";
        public static async Task<string> Multicall(string _chain, string _network, string _contract, string _abi, string _method, string _args, string _multicall = "", string _rpc = "")
        {
            WWWForm form = new WWWForm();
            form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));
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
                Response<string> data = JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                return data.response;
            }
        }
        public static async Task<CreateMintModel.Response> CreateMint(string _chain, string _network, string _account, string _to, string _cid, string _type)
        {
            Debug.Log("Chain: " + _chain);
            Debug.Log("Network: " + _network);
            Debug.Log("Account: " + _account);
            Debug.Log("to: " + _to);
            Debug.Log("CID: " + _cid);
            Debug.Log("Type: " + _type);
            WWWForm form = new WWWForm();
            form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));
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
                Debug.Log("Data: " + JsonConvert.SerializeObject(data.response, Formatting.Indented));
                return data.response;
            }
        }

        public static async Task<string> GetNftCollectionByAcc(string _account)
        {
            WWWForm form = new WWWForm();
            form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));
            form.AddField("account", _account);
            string url = host + "/collection/getByAccount";
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
            {
                await webRequest.SendWebRequest();
                string data = webRequest.downloadHandler.text;
                return data;
            }
        }

        public static async Task<string> GetNftCollectionBySlug(string _slug)
        {
            WWWForm form = new WWWForm();
            form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));
            form.AddField("slug", _slug);
            string url = host + "/collection/get";
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
            {
                await webRequest.SendWebRequest();
                string data = webRequest.downloadHandler.text;
                return data;
            }
        }

        public static async Task<string> GetNft(string _account, string _chain, string _network, string _nftContract, string _tokenId)
        {
            WWWForm form = new WWWForm();
            form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));
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

        public static async Task<List<GetNftListModel.Response>> GetNftMarket(string _chain, string _network)
        {
            WWWForm form = new WWWForm();
            form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));
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

        public static async Task<List<MintedNFT.Response>> GetMintedNFT(string _chain, string _network, string _account)
        {
            WWWForm form = new WWWForm();
            form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));
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

        public static async Task<BuyNFT.Response> CreatePurchaseNftTransaction(string _chain, string _network, string _account, string _itemId, string _price, string _tokenType)
        {
            WWWForm form = new WWWForm();
            form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));
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
                if (data.response != null)
                {
                    Debug.Log("Data: " + JsonConvert.SerializeObject(data.response, Formatting.Indented));
                }
                else
                {
                    Debug.Log("Response Object Empty ");
                }
                return data.response;
            }
        }

        public static async Task<ListNFT.Response> CreateListNftTransaction(string _chain, string _network, string _account, string _tokenId, string _priceHex, string _tokenType)
        {
            WWWForm form = new WWWForm();
            form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));
            form.AddField("chain", _chain);
            form.AddField("network", _network);
            form.AddField("account", _account);
            form.AddField("tokenId", _tokenId);
            Debug.Log("Token ID: " + _tokenId);
            form.AddField("priceHex", _priceHex);
            Debug.Log("Price Hex: " + _priceHex);
            form.AddField("tokenType", _tokenType);
            Debug.Log("Token Type: " + _tokenType);
            string url = host + "/createListNftTransaction";
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
            {
                await webRequest.SendWebRequest();
                ListNFT.Root data = JsonUtility.FromJson<ListNFT.Root>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                Debug.Log("Data: " + JsonConvert.SerializeObject(data.response, Formatting.Indented));
                return data.response;
            }
        }

        public static async Task<List<GetNftListModel.Response>> CreateCancelNftTransaction(string _chain, string _network, string _account, string _itemId)
        {
            WWWForm form = new WWWForm();
            form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));
            form.AddField("chain", _chain);
            form.AddField("network", _network);
            form.AddField("account", _account);
            form.AddField("tokenId", _itemId);
            string url = host + "/createCancelNftTransaction";
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
            {
                await webRequest.SendWebRequest();
                GetNftListModel.Root data = JsonUtility.FromJson<GetNftListModel.Root>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                return data.response;
            }
        }


        public static async Task<GetVoucherModel.GetVoucher721Response> Get721Voucher()
        {
            string url = hostVoucher + "/voucher721?receiver=" + PlayerPrefs.GetString("Account");

            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                await webRequest.SendWebRequest();
                GetVoucherModel.GetVoucher721Response root721 = JsonConvert.DeserializeObject<GetVoucherModel.GetVoucher721Response>(webRequest.downloadHandler.text);
                return root721;
            }
        }

        public static async Task<GetVoucherModel.GetVoucher1155Response> Get1155Voucher()
        {
            string url = hostVoucher + "/voucher1155?receiver=" + PlayerPrefs.GetString("Account");

            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                await webRequest.SendWebRequest();
                GetVoucherModel.GetVoucher1155Response root1155 = JsonConvert.DeserializeObject<GetVoucherModel.GetVoucher1155Response>(webRequest.downloadHandler.text);
                Debug.Log("Voucher Data" + root1155);
                return root1155;
            }
        }

        public static async Task<CreateApprovalModel.Response> CreateApproveTransaction(string _chain, string _network, string _account, string _tokenType)
        {
            WWWForm form = new WWWForm();
            form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));
            form.AddField("chain", _chain);
            form.AddField("network", _network);
            form.AddField("account", _account);
            form.AddField("tokenType", _tokenType);

            string url = host + "/createApproveTransaction";
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
            {
                await webRequest.SendWebRequest();
                CreateApprovalModel.Root data = JsonUtility.FromJson<CreateApprovalModel.Root>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                return data.response;
            }
        }


        public static async Task<string> AllErc721(string _chain, string _network, string _account, string _contract = "", int _first = 500, int _skip = 0)
        {
            WWWForm form = new WWWForm();
            form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));
            form.AddField("chain", _chain);
            form.AddField("network", _network);
            form.AddField("account", _account);
            form.AddField("contract", _contract);
            form.AddField("first", _first);
            form.AddField("skip", _skip);

            string url = host + "/all721";
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
            {
                await webRequest.SendWebRequest();
                Response<string> data = JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                return data.response;
            }
        }

        public static async Task<string> AllErc1155(string _chain, string _network, string _account, string _contract = "", int _first = 500, int _skip = 0)
        {
            WWWForm form = new WWWForm();
            form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));
            form.AddField("chain", _chain);
            form.AddField("network", _network);
            form.AddField("account", _account);
            form.AddField("contract", _contract);
            form.AddField("first", _first);
            form.AddField("skip", _skip);
            string url = host + "/all1155";
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
            {
                await webRequest.SendWebRequest();
                Response<string> data = JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                return data.response;
            }
        }
        public static async Task<RedeemVoucherTxModel.Response> CreateRedeemTransaction(string _chain, string _network, string _voucher, string _type, string _nftAddress, string _account)
        {
            WWWForm form = new WWWForm();
            form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));
            form.AddField("chain", _chain);
            form.AddField("network", _network);
            form.AddField("voucher", _voucher);
            form.AddField("type", _type);
            form.AddField("nftAdrress", _nftAddress);
            form.AddField("account", _account);
            string url = host + "/createRedeemTransaction";
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
            {
                await webRequest.SendWebRequest();
                RedeemVoucherTxModel.Root data = JsonConvert.DeserializeObject<RedeemVoucherTxModel.Root>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                return data.response;
            }
        }
    }
}

