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
        // Wait for a single frame
        yield return 0;
        //var requestURL = $"{baseAddress}/store/get?key={key}";
        //var request = UnityWebRequest.Get(requestURL);
        WWWForm data = new WWWForm();
        data.AddField("key", key);

        var request = UnityWebRequest.Post($"{baseAddress}/store/get", data);

        yield return request.SendWebRequest();
        // Debug.Log("baseAddress =>" + baseAddress);
        // Debug.Log("key =>" + key);
        // //Debug.Log("request URL =>"+ requestURL);
        // Debug.Log("request.isNetworkError =>" + request.isNetworkError);
        // Debug.Log("request.isHttpError =>" + request.isHttpError);
        // Debug.Log("request.isHttpError =>" + request.error);
        // Debug.Log("request.result =>" + request.result);
        // Debug.Log("request.downloadHandler.text =>" + request.downloadHandler.text);
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
        // Debug.Log("key during logout session =>" + logoutApiRequest.key);

        var request = UnityWebRequest.Post($"{baseAddress}/store/set", data);
        yield return request.SendWebRequest();

        // Debug.Log("baseAddress =>" + baseAddress);
        // Debug.Log("key =>" + logoutApiRequest.key);
        // Debug.Log("request URL =>"+ requestURL);
        // Debug.Log("request.isNetworkError =>" + request.isNetworkError);
        // Debug.Log("request.isHttpError =>" + request.isHttpError);
        // Debug.Log("request.isHttpError =>" + request.error);
        // Debug.Log("request.result =>" + request.result);
        // Debug.Log("request.downloadHandler.text =>" + request.downloadHandler.text);

        if (request.result == UnityWebRequest.Result.Success)
        {
            string result = request.downloadHandler.text;
            callback(Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(result));
        }
        else
            callback(null);
    }

    public IEnumerator createSession(LogoutApiRequest logoutApiRequest, Action<JObject> callback)
    {
        WWWForm data = new WWWForm();
        data.AddField("key", logoutApiRequest.key);
        data.AddField("data", logoutApiRequest.data);
        data.AddField("signature", logoutApiRequest.signature);
        data.AddField("timeout", logoutApiRequest.timeout.ToString());
        // Debug.Log("key during create session =>" + logoutApiRequest.key);
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
