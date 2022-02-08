using System;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class EVM
{
    public class Response<T> { public T response; }

    private readonly static string host = "https://api.gaming.chainsafe.io/evm";

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
            try
            {
                Response<string> data = JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                return data.response;
            }
            finally
            {
                webRequest.Dispose();
            }
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
            try
            {
                Response<string> data = JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                return data.response;
            }
            finally
            {
                webRequest.Dispose();
            }
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
            try
            {
                Response<string> data = JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                return data.response;
            }
            finally
            {
                webRequest.Dispose();
            }
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
            try
            {
                Response<string> data = JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                return data.response;
            }
            finally
            {
                webRequest.Dispose();
            }
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
            try
            {
                Response<string> data = JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                return data.response;
            }
            finally
            {
                webRequest.Dispose();
            }
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
            try
            {
                Response<int> data = JsonUtility.FromJson<Response<int>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                return data.response;
            }
            finally
            {
                webRequest.Dispose();
            }
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
            try
            {
                Response<string> data = JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                return data.response;
            }
            finally
            {
                webRequest.Dispose();
            }
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
            try
            {
                Response<string> data = JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                return data.response;
            }
            finally
            {
                webRequest.Dispose();
            }
        }
    }

    public static async Task<string> AllErc721(string _chain, string _network, string _account, string _contract = "")
    {
        WWWForm form = new WWWForm();
        form.AddField("chain", _chain);
        form.AddField("network", _network);
        form.AddField("account", _account);
        form.AddField("contract", _contract);
        string url = host + "/all721";
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            await webRequest.SendWebRequest();
            try
            {
                Response<string> data = JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                return data.response;
            }
            finally
            {
                webRequest.Dispose();
            }
        }
    }

    public static async Task<AllERCData[]> AllErc721WithClass(string _chain, string _network, string _account, string _contract = "")
    {
        WWWForm form = new WWWForm();
        form.AddField("chain", _chain);
        form.AddField("network", _network);
        form.AddField("account", _account);
        form.AddField("contract", _contract);
        string url = host + "/all721";
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            await webRequest.SendWebRequest();
            try
            {
                AllERCData[] data = JsonHelper.FromJson<AllERCData>(JsonHelper.FixJson(JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data)).response));
                return data;
            }
            finally
            {
                webRequest.Dispose();
            }
        }
    }

    public static async Task<string> AllErc1155(string _chain, string _network, string _account, string _contract = "")
    {
        WWWForm form = new WWWForm();
        form.AddField("chain", _chain);
        form.AddField("network", _network);
        form.AddField("account", _account);
        form.AddField("contract", _contract);
        string url = host + "/all1155";
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            await webRequest.SendWebRequest();
            try
            {
                Response<string> data = JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                return data.response;
            }
            finally
            {
                webRequest.Dispose();
            }
        }
    }

    public static async Task<AllERCData[]> AllErc1155WithClass(string _chain, string _network, string _account, string _contract = "")
    {
        WWWForm form = new WWWForm();
        form.AddField("chain", _chain);
        form.AddField("network", _network);
        form.AddField("account", _account);
        form.AddField("contract", _contract);
        string url = host + "/all1155";
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            await webRequest.SendWebRequest();
            try
            {
                AllERCData[] data = JsonHelper.FromJson<AllERCData>(JsonHelper.FixJson(JsonUtility.FromJson<Response<string>>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data)).response));
                return data;
            }
            finally
            {
                webRequest.Dispose();
            }
        }
    }
}