using Web3Unity.Scripts.Library.Ethers.RPC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

namespace Web3Unity.Scripts.Library.Ethers.Unity
{
    public class UnityRpcEnvironment : IRpcEnvironment
    {
        Dispatcher _dispatcher;
        DataDog _dataDog;
        string _defaultRpcUrl;

#pragma warning disable CS0414
        // Make sure we're referencing the Unity build of the Core library.
        // To make this work, pass `/r:property:Unity=true` to `dotnet build`
        // when building `ChainSafe.GamingSDK.EVM` and this library.
        readonly Utils.IsUnityBuild _isUnityBuild = null;
#pragma warning restore CS0414

        private UnityRpcEnvironment(string defaultRpcUrl, string dataDogApiKey)
        {
            _dispatcher = Dispatcher.Initialize();
            _dataDog = new DataDog(dataDogApiKey);
            _defaultRpcUrl = defaultRpcUrl;
        }

        public static void InitializeRpcEnvironment(string defaultRpcUrl, string dataDogApiKey)
        {
            RpcEnvironmentStore.Initialize(new UnityRpcEnvironment(defaultRpcUrl, dataDogApiKey));
        }

        public void CaptureEvent(string eventName, Dictionary<string, object> properties)
        {
            _dataDog.Capture(eventName, properties);
        }

        public Task<NetworkResponse> GetAsync(string url) =>
            _dispatcher.EnqueueTask(async () =>
            {
                using var request = UnityWebRequest.Get(url);
                request.downloadHandler = new DownloadHandlerBuffer();
                await request.SendWebRequest();

                Assert.AreNotEqual(request.result, UnityWebRequest.Result.InProgress);

                if (request.result != UnityWebRequest.Result.Success)
                {
                    return NetworkResponse.Failure(request.error);
                }

                return NetworkResponse.Success(request.downloadHandler.text);
            });

        public Task<NetworkResponse> PostAsync(string url, string requestBody, string contentType) =>
            _dispatcher.EnqueueTask(async () =>
            {
                using var request = new UnityWebRequest(url, "POST");
                request.uploadHandler = new UploadHandlerRaw(new UTF8Encoding().GetBytes(requestBody));
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", contentType);
                await request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    return NetworkResponse.Failure(request.error);
                }

                return NetworkResponse.Success(request.downloadHandler.text);
            });

        public string GetDefaultRpcUrl() => _defaultRpcUrl;

        public void LogError(string message)
        {
            RunOnForegroundThread(() => Debug.LogError(message));
        }

        public void RunOnForegroundThread(Action action)
        {
            _dispatcher.Enqueue(action);
        }
    }
}
