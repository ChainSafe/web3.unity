using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using MoonbeamDefinition;
using UnityEngine;
using UnityEngine.Networking;

public class Moonbeam 
{
    private static string host = _Config.Host;

    public static async Task<BigInteger>
    BalanceOf(string _network, string _account)
    {
        string url =
            host +
            "/moonbeam/balanceOf?network=" +
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
            "/moonbeam/verify?message=" +
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

}
