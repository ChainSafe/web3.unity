using System.Numerics;
using System.Threading.Tasks;
using Token20Definition;
using UnityEngine;
using UnityEngine.Networking;

public class XDai20
{
    private static string host = _Config.Host;

    public static async Task<BigInteger>
    BalanceOf(string _contract, string _account)
    {
        string url =
            host +
            "/xdai20/balanceOf?" +
            "contract=" +
            _contract +
            "&account=" +
            _account;
        UnityWebRequest webRequest =UnityWebRequest.Get(url);
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

    public static async Task<BigInteger>
    Decimals(string _contract)
    {
        string url =
            host +
            "/xdai20/decimals?" +
            "contract=" +
            _contract;
        UnityWebRequest webRequest =UnityWebRequest.Get(url);
        await webRequest.SendWebRequest();
        Decimals response =
            JsonUtility
                .FromJson<Decimals>(System
                    .Text
                    .Encoding
                    .UTF8
                    .GetString(webRequest.downloadHandler.data));
        return BigInteger.Parse(response.decimals);
    }

    public static async Task<string> Name(string _contract)
    {
        string url =
            host + "/xdai20/name?" + "contract=" + _contract;
        UnityWebRequest webRequest =UnityWebRequest.Get(url);
        await webRequest.SendWebRequest();
        Name response =
            JsonUtility
                .FromJson<Name>(System
                    .Text
                    .Encoding
                    .UTF8
                    .GetString(webRequest.downloadHandler.data));
        return response.name;
    }

    public static async Task<string> Symbol(string _contract)
    {
        string url =
            host +
            "/xdai20/symbol?" +
            "contract=" +
            _contract;
        UnityWebRequest webRequest =UnityWebRequest.Get(url);
        await webRequest.SendWebRequest();
        Symbol response =
            JsonUtility
                .FromJson<Symbol>(System
                    .Text
                    .Encoding
                    .UTF8
                    .GetString(webRequest.downloadHandler.data));
        return response.symbol;
    }

    public static async Task<BigInteger>
    TotalSupply(string _contract)
    {
        string url =
            host +
            "/xdai20/totalSupply?" +
            "contract=" +
            _contract;
        UnityWebRequest webRequest =UnityWebRequest.Get(url);
        await webRequest.SendWebRequest();
        TotalSupply response =
            JsonUtility
                .FromJson<TotalSupply>(System
                    .Text
                    .Encoding
                    .UTF8
                    .GetString(webRequest.downloadHandler.data));
        return BigInteger.Parse(response.totalSupply);
    }
}
