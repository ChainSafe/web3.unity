using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using EthereumDefinition;
using UnityEngine;
using UnityEngine.Networking;

public class Ethereum
{
    private static string host = _Config.Host;

    public static async Task<BigInteger>
    BalanceOf(string _network, string _account)
    {
        string url =
            host +
            "/ethereum/balanceOf?network=" +
            _network +
            "&account=" +
            _account;
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        await webRequest.SendWebRequest();
        BalanceOf response =
            JsonUtility
                .FromJson<BalanceOf>(System
                    .Text
                    .Encoding
                    .UTF8
                    .GetString(webRequest.downloadHandler.data));
        return BigInteger.Parse(response.balanceOf);
    }

    public static async Task<string>
    Verify(string _message, string _signature)
    {
        string url =
            host +
            "/ethereum/verify?message=" +
            _message +
            "&signature=" +
            _signature;
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        await webRequest.SendWebRequest();
        Verify response =
            JsonUtility
                .FromJson<Verify>(System
                    .Text
                    .Encoding
                    .UTF8
                    .GetString(webRequest.downloadHandler.data));
        return response.verify;
    }

    public static async Task<string>
    Broadcast(string _network, string _transaction)
    {
        string url =
            host +
            "/ethereum/broadcast?network=" +
            _network +
            "&transaction=" +
            _transaction;
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        await webRequest.SendWebRequest();
        Broadcast response =
            JsonUtility
                .FromJson<Broadcast>(System
                    .Text
                    .Encoding
                    .UTF8
                    .GetString(webRequest.downloadHandler.data));
        return response.broadcast;
    }

    public static async Task<Transaction>
    CreateTransaction(string _network, string _from, string _to, string _eth)
    {
        string url =
            host +
            "/ethereum/createtransaction?network=" +
            _network +
            "&from=" +
            _from +
            "&to=" +
            _to +
            "&eth=" +
            _eth;
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        await webRequest.SendWebRequest();
        Transaction response =
            JsonUtility
                .FromJson<Transaction>(System
                    .Text
                    .Encoding
                    .UTF8
                    .GetString(webRequest.downloadHandler.data));
        return response;
    }
}
