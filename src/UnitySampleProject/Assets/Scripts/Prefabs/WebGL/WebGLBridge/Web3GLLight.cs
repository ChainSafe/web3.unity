using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

#if UNITY_WEBGL
public class Web3GLLight
{
    [DllImport("__Internal")]
    private static extern void ClearResponseJs();

    [DllImport("__Internal")]
    private static extern string SendAsyncResponse();

    [DllImport("__Internal")]
    private static extern string SendAsyncError();

    [DllImport("__Internal")]
    private static extern void SendAsyncJs(string method, string parameters);

    public static async Task<string> SendAsync(string method, string parameters)
    {
        ClearResponseJs();

        SendAsyncJs(method, parameters);
        var response = "";
        var error = "";

        while (response == "" && error == "")
        {
            await new WaitForSeconds(0.1f);
            response = SendAsyncResponse();
            error = SendAsyncError();
        }

        if (error != "")
        {
            var err = JsonConvert.DeserializeObject<WalletError>(error);
            throw new WalletException(err!.Code, err.Message);
        }

        return response;
    }

    public class WalletError
    {
        [JsonProperty(PropertyName = "code")] public int Code { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }

    [Serializable]
    public class WalletException : Exception
    {
        public int code;
        public string message;

        public WalletException(int code, string message)
        {
            this.code = code;
            this.message = message;
        }
    }
}
#endif