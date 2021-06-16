using System.Numerics;
using System.Threading.Tasks;
using Token20Definition;
using UnityEngine;
using UnityEngine.Networking;

public class Avalanche20
{
    private static string host = _Config.Host;

    public static async Task<BigInteger>
    BalanceOf(string _network, string _contract, string _account)
    {
        string url =
            host +
            "/avalanche20/balanceOf?network=" +
            _network +
            "&contract=" +
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
    Decimals(string _network, string _contract)
    {
        string url =
            host +
            "/avalanche20/decimals?network=" +
            _network +
            "&contract=" +
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

    public static async Task<string> Name(string _network, string _contract)
    {
        string url =
            host + "/avalanche20/name?network=" + _network + "&contract=" + _contract;
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

    public static async Task<string> Symbol(string _network, string _contract)
    {
        string url =
            host +
            "/avalanche20/symbol?network=" +
            _network +
            "&contract=" +
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
    TotalSupply(string _network, string _contract)
    {
        string url =
            host +
            "/avalanche20/totalSupply?network=" +
            _network +
            "&contract=" +
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
