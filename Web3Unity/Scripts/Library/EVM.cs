using System;
using System.Numerics;
using System.Threading.Tasks;
using Models;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class EVM
{
    public class Response<T> { public T response; }

    private readonly static string host = "https://api.gaming.chainsafe.io/evm";
    private readonly static string hostVoucher = "https://lazy-minting-voucher-signer.herokuapp.com";


    public static async Task<string> BalanceOf(string _chain, string _network, string _account, string _rpc = "")
    {
        WWWForm form = new WWWForm();
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
        WWWForm form = new WWWForm();
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

    public static async Task<int> BlockNumber(string _chain, string _network, string _rpc = "")
    {
        WWWForm form = new WWWForm();
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

    public static async Task<string> AllErc721(string _chain, string _network, string _account, string _contract = "", int _first = 500, int _skip = 0)
    {
        WWWForm form = new WWWForm();
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

    public static async Task<string> GasPrice(string _chain, string _network, string _rpc = "")
    {
        WWWForm form = new WWWForm();
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

    public static async Task<string> CreateTransaction(string _chain, string _network, string _account, string _to, string _value, string _data, string _gasPrice = "", string _gasLimit = "", string _rpc = "")
    {
        WWWForm form = new WWWForm();
        form.AddField("chain", _chain);
        form.AddField("network", _network);
        form.AddField("account", _account);
        form.AddField("to", _to);
        form.AddField("value", _value);
        form.AddField("data", _data);
        form.AddField("gasPrice", _gasPrice);
        form.AddField("gasLimit", _gasLimit);
        form.AddField("rpc", _rpc);
        string url = host + "/createTransaction";
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            await webRequest.SendWebRequest();
            Response<string> data = JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
            return data.response;
        }
    }

    public static async Task<string> BroadcastTransaction(string _chain, string _network, string _account, string _to, string _value, string _data, string _signature, string _gasPrice, string _gasLimit, string _rpc)
    {
        WWWForm form = new WWWForm();
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

    public static async Task<GetVoucherModel.GetVoucher721Response> Get721Voucher()
    {
        string url = hostVoucher + "/voucher721";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            await webRequest.SendWebRequest();
            GetVoucherModel.GetVoucher721Response root721 = JsonConvert.DeserializeObject<GetVoucherModel.GetVoucher721Response>(webRequest.downloadHandler.text);
            return root721;
        }
    }

    public static async Task<GetVoucherModel.GetVoucher1155Response> Get1155Voucher()
    {
        string url = hostVoucher + "/voucher1155";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            await webRequest.SendWebRequest();
            GetVoucherModel.GetVoucher1155Response root1155 = JsonConvert.DeserializeObject<GetVoucherModel.GetVoucher1155Response>(webRequest.downloadHandler.text);
            return root1155;
        }
    }
}
