using System;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class EVM
{
  public class StringResponse { public string response; }
  public class IntResponse { public int response; }

  private readonly static string host = "https://api.gaming.chainsafe.io/evm";

  public static async Task<string> BalanceOf(string _chain, string _network, string _account, string _rpc = "")
  {
    WWWForm form = new WWWForm();
    form.AddField("chain", _chain);
    form.AddField("network", _network);
    form.AddField("account", _account);
    form.AddField("rpc", _rpc);
    string url = host + "/balanceOf";
    UnityWebRequest webRequest = UnityWebRequest.Post(url, form);
    await webRequest.SendWebRequest();
    StringResponse data = JsonUtility.FromJson<StringResponse>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
    return data.response;
  }

  public static async Task<string> Verify(string _message, string _signature)
  {
    WWWForm form = new WWWForm();
    form.AddField("message", _message);
    form.AddField("signature", _signature);
    string url = host + "/verify";
    UnityWebRequest webRequest = UnityWebRequest.Post(url, form);
    await webRequest.SendWebRequest();
    StringResponse data = JsonUtility.FromJson<StringResponse>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
    return data.response;
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
    UnityWebRequest webRequest = UnityWebRequest.Post(url, form);
    await webRequest.SendWebRequest();
    StringResponse data = JsonUtility.FromJson<StringResponse>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
    return data.response; 
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
    UnityWebRequest webRequest = UnityWebRequest.Post(url, form);
    await webRequest.SendWebRequest();
    StringResponse data = JsonUtility.FromJson<StringResponse>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
    return data.response; 
  }

  public static async Task<string> TxStatus(string _chain, string _network, string _transaction, string _rpc = "")
  {
    WWWForm form = new WWWForm();
    form.AddField("chain", _chain);
    form.AddField("network", _network);
    form.AddField("transaction", _transaction);
    form.AddField("rpc", _rpc);
    string url = host + "/txStatus";
    UnityWebRequest webRequest = UnityWebRequest.Post(url, form);
    await webRequest.SendWebRequest();
    StringResponse data = JsonUtility.FromJson<StringResponse>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
    return data.response;
  }

  public static async Task<int> BlockNumber(string _chain, string _network, string _rpc = "")
  {
    WWWForm form = new WWWForm();
    form.AddField("chain", _chain);
    form.AddField("network", _network);
    form.AddField("rpc", _rpc);
    string url = host + "/blockNumber";
    UnityWebRequest webRequest = UnityWebRequest.Post(url, form);
    await webRequest.SendWebRequest();
    IntResponse data = JsonUtility.FromJson<IntResponse>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
    return data.response;
  } 

  public static async Task<string> Nonce(string _chain, string _network, string _account, string _rpc = "")
  {
    WWWForm form = new WWWForm();
    form.AddField("chain", _chain);
    form.AddField("network", _network);
    form.AddField("account", _account);
    form.AddField("rpc", _rpc);
    string url = host + "/nonce";
    UnityWebRequest webRequest = UnityWebRequest.Post(url, form);
    await webRequest.SendWebRequest();
    StringResponse data = JsonUtility.FromJson<StringResponse>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
    return data.response;
  } 

  public static async Task<string> CreateContractData(string _abi, string _method, string _args)
  {
    WWWForm form = new WWWForm();
    form.AddField("abi", _abi);
    form.AddField("method", _method);
    form.AddField("args", _args);
    string url = host + "/createContractData";
    UnityWebRequest webRequest = UnityWebRequest.Post(url, form);
    await webRequest.SendWebRequest();
    StringResponse data = JsonUtility.FromJson<StringResponse>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
    return data.response;
  }
}

