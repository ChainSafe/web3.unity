using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Models;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using SDKConfiguration;

public class EVM
{
    public class Response<T> { public T response; }

    private static readonly string host = "https://api.gaming.chainsafe.io/evm";
    private static readonly string hostVoucher = "https://lazy-minting-voucher-signer.herokuapp.com";

    public static async Task<string> BalanceOf(string _chain, string _network, string _account, string _rpc = "")
    {
        WWWForm form = new WWWForm();
        form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));
        form.AddField("chain", _chain);
        form.AddField("network", _network);
        form.AddField("account", _account);
        form.AddField("rpc", _rpc);
        string url = host + "/balanceOf";
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            await webRequest.SendWebRequest();
            Response<string> data = JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
            return data.response;
        }
    }

    public static async Task<string> Verify(string _message, string _signature)
    {
        WWWForm form = new WWWForm();
        form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));
        form.AddField("message", _message);
        form.AddField("signature", _signature);
        string url = host + "/verify";
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            await webRequest.SendWebRequest();
            Response<string> data = JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
            return data.response;
        }
    }

    public static async Task<string> Call(string _chain, string _network, string _contract, string _abi, string _method, string _args, string _rpc = "")
    {
        WWWForm form = new WWWForm();
        form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));
        form.AddField("chain", _chain);
        form.AddField("network", _network);
        form.AddField("contract", _contract);
        form.AddField("abi", _abi);
        form.AddField("method", _method);
        form.AddField("args", _args);
        form.AddField("rpc", _rpc);
        string url = host + "/call";
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            await webRequest.SendWebRequest();
            Response<string> data = JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
            return data.response;
        }
    }

    public static async Task<string> MultiCall(string _chain, string _network, string _contract, string _abi, string _method, string _args, string _multicall = "", string _rpc = "")
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

    public static async Task<string> TxStatus(string _chain, string _network, string _transaction, string _rpc = "")
    {
        WWWForm form = new WWWForm();
        form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));
        form.AddField("chain", _chain);
        form.AddField("network", _network);
        form.AddField("transaction", _transaction);
        form.AddField("rpc", _rpc);
        string url = host + "/txStatus";
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
            Debug.Log("Data: " + JsonConvert.SerializeObject( data.response, Formatting.Indented ));
            return data.response;
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
                Debug.Log("Data: " +  JsonConvert.SerializeObject( data.response, Formatting.Indented ));
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
            ListNFT.Root data =JsonUtility.FromJson<ListNFT.Root>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
            Debug.Log("Data: " + JsonConvert.SerializeObject( data.response, Formatting.Indented ));
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
            GetVoucherModel.GetVoucher721Response root721= JsonConvert.DeserializeObject<GetVoucherModel.GetVoucher721Response>(webRequest.downloadHandler.text);
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

    public static async Task<int> BlockNumber(string _chain, string _network, string _rpc = "")
    {
        WWWForm form = new WWWForm();
        form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));
        form.AddField("chain", _chain);
        form.AddField("network", _network);
        form.AddField("rpc", _rpc);
        string url = host + "/blockNumber";
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            await webRequest.SendWebRequest();
            Response<int> data = JsonUtility.FromJson<Response<int>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
            return data.response;
        }
    }

    public static async Task<string> Nonce(string _chain, string _network, string _account, string _rpc = "")
    {
        WWWForm form = new WWWForm();
        form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));
        form.AddField("chain", _chain);
        form.AddField("network", _network);
        form.AddField("account", _account);
        form.AddField("rpc", _rpc);
        string url = host + "/nonce";
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            await webRequest.SendWebRequest();
            Response<string> data = JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
            return data.response;
        }
    }

    public static async Task<string> CreateContractData(string _abi, string _method, string _args)
    {
        WWWForm form = new WWWForm();
        form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));
        form.AddField("abi", _abi);
        form.AddField("method", _method);
        form.AddField("args", _args);
        string url = host + "/createContractData";
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            await webRequest.SendWebRequest();
            Response<string> data = JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
            return data.response;
        }
    }

    public static async Task<string> GasPrice(string _chain, string _network, string _rpc = "")
    {
        WWWForm form = new WWWForm();
        form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));
        form.AddField("chain", _chain);
        form.AddField("network", _network);
        form.AddField("rpc", _rpc);
        string url = host + "/gasPrice";
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            await webRequest.SendWebRequest();
            Response<string> data = JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
            return data.response;
        }
    }

    public static async Task<string> GasLimit(string _chain, string _network, string _to, string _value, string _data, string _rpc = "")
    {
        WWWForm form = new WWWForm();
        form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));
        form.AddField("chain", _chain);
        form.AddField("network", _network);
        form.AddField("to", _to);
        form.AddField("value", _value);
        form.AddField("data", _data);
        form.AddField("rpc", _rpc);
        string url = host + "/gasLimit";
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            await webRequest.SendWebRequest();
            Response<string> data = JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
            return data.response;
        }
    }

    public static async Task<string> ChainId(string _chain, string _network, string _rpc = "")
    {
        WWWForm form = new WWWForm();
        form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));
        form.AddField("chain", _chain);
        form.AddField("network", _network);
        form.AddField("rpc", _rpc);
        string url = host + "/chainId";
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            await webRequest.SendWebRequest();
            Response<string> data = JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
            return data.response;
        }
    }

    public static async Task<string> CreateTransaction(string _chain, string _network, string _account, string _to, string _value, string _data, string _gasPrice = "", string _gasLimit = "", string _rpc = "", string _nonce = "")
    {
        WWWForm form = new WWWForm();
        form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));
        form.AddField("chain", _chain);
        form.AddField("network", _network);
        form.AddField("account", _account);
        form.AddField("to", _to);
        form.AddField("value", _value);
        form.AddField("data", _data);
        form.AddField("gasPrice", _gasPrice);
        form.AddField("gasLimit", _gasLimit);
        form.AddField("rpc", _rpc);
        form.AddField("nonce", _nonce);
        string url = host + "/createTransaction";
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

    public static async Task<string> BroadcastTransaction(string _chain, string _network, string _account, string _to, string _value, string _data, string _signature, string _gasPrice, string _gasLimit, string _rpc)
    {
        WWWForm form = new WWWForm();
        form.AddField("projectId", PlayerPrefs.GetString("ProjectID"));
        form.AddField("chain", _chain);
        form.AddField("network", _network);
        form.AddField("account", _account);
        form.AddField("to", _to);
        form.AddField("value", _value);
        form.AddField("data", _data);
        form.AddField("signature", _signature);
        form.AddField("gasPrice", _gasPrice);
        form.AddField("gasLimit", _gasLimit);
        form.AddField("rpc", _rpc);
        string url = host + "/broadcastTransaction";
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            await webRequest.SendWebRequest();
            Response<string> data = JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
            return data.response;
        }
    }

}
