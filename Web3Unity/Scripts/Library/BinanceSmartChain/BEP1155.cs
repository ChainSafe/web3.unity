using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Token1155Definition;
using UnityEngine;
using UnityEngine.Networking;

public class BEP1155
{
    private static string host = _Config.Host;

    public static async Task<BigInteger>
    BalanceOf(string _network, string _contract, string _account, string _id)
    {
        string url =
            host +
            "/bep1155/balanceOf?network=" +
            _network +
            "&contract=" +
            _contract +
            "&id=" +
            _id +
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

    public static async Task<List<BigInteger>>
    BalanceOfBatch(
        string _network,
        string _contract,
        string[] _accounts,
        string[] _ids
    )
    {
        string url =
            host +
            "/bep1155/balanceOfBatch?network=" +
            _network +
            "&contract=" +
            _contract +
            "&ids=" +
            string.Join(",", _ids) +
            "&accounts=" +
            string.Join(",", _accounts);
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        await webRequest.SendWebRequest();
        BalanceOfBatch response =
            JsonUtility
                .FromJson<BalanceOfBatch>(System
                    .Text
                    .Encoding
                    .UTF8
                    .GetString(webRequest.downloadHandler.data));

        // convert list of strings to list of BigIntegers
        List<BigInteger> balances = new List<BigInteger>();
        for (int i = 0; i < response.balanceOfBatch.Count; i++)
        {
            balances.Add(BigInteger.Parse(response.balanceOfBatch[i]));
        }
        return balances;
    }

    public static async Task<bool>
    IsApprovedForAll(
        string _network,
        string _contract,
        string _account,
        string _operator
    )
    {
        string url =
            host +
            "/bep1155/isApprovedForAll?network=" +
            _network +
            "&contract=" +
            _contract +
            "&account=" +
            _account +
            "&operator=" +
            _operator;
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        await webRequest.SendWebRequest();
        IsApprovedForAll response =
            JsonUtility
                .FromJson<IsApprovedForAll>(System
                    .Text
                    .Encoding
                    .UTF8
                    .GetString(webRequest.downloadHandler.data));
        return response.isApprovedForAll;
    }

    public static async Task<string>
    URI(string _network, string _contract, string _id)
    {
        string url =
            host +
            "/bep1155/uri?network=" +
            _network +
            "&contract=" +
            _contract +
            "&id=" +
            _id;
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        await webRequest.SendWebRequest();
        Uri response =
            JsonUtility
                .FromJson<Uri>(System
                    .Text
                    .Encoding
                    .UTF8
                    .GetString(webRequest.downloadHandler.data));
        return response.uri;
    }
}
