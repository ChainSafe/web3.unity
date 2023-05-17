using System.Collections;
using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine.Networking;
using UnityEngine;

public class Web3AuthApi
{
    static Web3AuthApi instance;
    static string baseAddress = "https://broadcast-server.tor.us";

    public static Web3AuthApi getInstance()
    {
        if (instance == null)
            instance = new Web3AuthApi();
        return instance;
    }

    public IEnumerator authorizeSession(string key, Action<StoreApiResponse> callback)
    {
        var request = UnityWebRequest.Get($"{baseAddress}/store/get?key={key}");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string result = request.downloadHandler.text;
            callback(Newtonsoft.Json.JsonConvert.DeserializeObject<StoreApiResponse>(result));
        }
        else
            callback(null);
    }

    public IEnumerator logout(LogoutApiRequest logoutApiRequest, Action<JObject> callback)
    {
        WWWForm data = new WWWForm();
        data.AddField("key", logoutApiRequest.key);
        data.AddField("data", logoutApiRequest.data);
        data.AddField("signature", logoutApiRequest.signature);
        data.AddField("timeout", logoutApiRequest.timeout.ToString());

        var request = UnityWebRequest.Post($"{baseAddress}/store/set", data);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string result = request.downloadHandler.text;
            callback(Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(result));
        }
        else
            callback(null);
    }
}
