using System.Numerics;
using System.Threading.Tasks;
using Token721Definition;
using UnityEngine;
using UnityEngine.Networking;

public class Polygon721
{
    private static string host = _Config.Host;

    public static async Task<BigInteger>
    BalanceOf(string _network, string _contract, string _account)
    {
        string url =
            host +
            "/polygon721/balanceOf?network=" +
            _network +
            "&contract=" +
            _contract +
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
    OwnerOf(string _network, string _contract, string _tokenId)
    {
        string url =
            host +
            "/polygon721/ownerOf?network=" +
            _network +
            "&contract=" +
            _contract +
            "&token=" +
            _tokenId;
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        await webRequest.SendWebRequest();
        OwnerOf response =
            JsonUtility
                .FromJson<OwnerOf>(System
                    .Text
                    .Encoding
                    .UTF8
                    .GetString(webRequest.downloadHandler.data));
        return response.ownerOf;
    }

    public static async Task<string>
    TokenURI(string _network, string _contract, string _tokenId)
    {
        string url =
            host +
            "/polygon721/tokenURI?network=" +
            _network +
            "&contract=" +
            _contract +
            "&token=" +
            _tokenId;
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        await webRequest.SendWebRequest();
        TokenURI response =
            JsonUtility
                .FromJson<TokenURI>(System
                    .Text
                    .Encoding
                    .UTF8
                    .GetString(webRequest.downloadHandler.data));
        return response.tokenURI;
    }
}
