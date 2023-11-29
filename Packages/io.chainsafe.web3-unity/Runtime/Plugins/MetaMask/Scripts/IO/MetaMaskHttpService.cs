using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using evm.net.Network;
using MetaMask.Unity;
using UnityEngine;
using UnityEngine.Networking;
#if UNITY_WEBGL && !UNITY_EDITOR
using System;
using UnityEngine.Scripting;
using MetaMask.Unity.Utils;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
#endif

namespace MetaMask.IO
{
    /// <summary>
    /// A Singleton class that handles all the network requests. It uses UnityWebRequest to make the requests.
    /// </summary>
    public class MetaMaskHttpService : Singleton<MetaMaskHttpService>
    {
        public enum RequestType
        {
            GET,
            POST,
            DELETE
        }
        
        /// <summary>
        /// A class that represents a single HTTP Request
        /// </summary>
        public class UnityHttpServiceRequest
        {
            public TaskCompletionSource<string> requestTask;
            public string url;
            public string @params;
            public RequestType requestType;
            public string authKey;
            public string authValue;
        }

        /// <summary>
        /// A class that implements the IHttpService interface.
        /// </summary>
        public class UnityHttpServiceProvider : IHttpService
        {
            private MetaMaskHttpService service;
            private string baseUrl;
            private string authValue;
            private string authKey;

            public UnityHttpServiceProvider(string baseUrl, string authKey, string authValue, MetaMaskHttpService service)
            {
                this.service = service;
                this.baseUrl = baseUrl;
                this.authValue = authValue;
                this.authKey = authKey;
            }

            public Task<string> Get(string uri)
            {
                var fullUrl = string.IsNullOrWhiteSpace(baseUrl) ? uri : 
                    baseUrl.EndsWith("/") || uri.StartsWith("/") ? $"{baseUrl}{uri}" : $"{baseUrl}/{uri}";
                
                // ensure we dont end on a /
                if (fullUrl.EndsWith("/"))
                    fullUrl = fullUrl.Substring(0, fullUrl.Length - 1);
                
                var request = new UnityHttpServiceRequest()
                {
                    url = fullUrl,
                    requestTask = new TaskCompletionSource<string>(),
                    authKey = authKey,
                    authValue = authValue,
                    requestType = RequestType.GET
                };
                
                service.requests.Enqueue(request);

                return request.requestTask.Task;
            }

            public Task<string> Post(string uri, string @params)
            {
                var fullUrl = string.IsNullOrWhiteSpace(baseUrl) ? uri : 
                    baseUrl.EndsWith("/") || uri.StartsWith("/") ? $"{baseUrl}{uri}" : $"{baseUrl}/{uri}";
                
                // ensure we dont end on a /
                if (fullUrl.EndsWith("/"))
                    fullUrl = fullUrl.Substring(0, fullUrl.Length - 1);
                
                var request = new UnityHttpServiceRequest()
                {
                    url = fullUrl,
                    requestTask = new TaskCompletionSource<string>(),
                    requestType = RequestType.POST,
                    @params = @params,
                    authKey = authKey,
                    authValue = authValue
                };
                
                service.requests.Enqueue(request);

                return request.requestTask.Task;
            }
            
            public Task<string> Delete(string uri, string @params)
            {
                var fullUrl = string.IsNullOrWhiteSpace(baseUrl) ? uri : 
                    baseUrl.EndsWith("/") || uri.StartsWith("/") ? $"{baseUrl}{uri}" : $"{baseUrl}/{uri}";
                
                // ensure we dont end on a /
                if (fullUrl.EndsWith("/"))
                    fullUrl = fullUrl.Substring(0, fullUrl.Length - 1);
                
                var request = new UnityHttpServiceRequest()
                {
                    url = fullUrl,
                    requestTask = new TaskCompletionSource<string>(),
                    requestType = RequestType.DELETE,
                    @params = @params,
                    authKey = authKey,
                    authValue = authValue
                };
                
                service.requests.Enqueue(request);

                return request.requestTask.Task;
            }
        }

        private Queue<UnityHttpServiceRequest> requests = new();
        private bool isCheckingQueue;

        private void Awake()
        {
            HttpServiceFactory.SetCreator(CreateHttpService);
        }

        private IHttpService CreateHttpService(string baseUrl, string authHeaderValue, string authHeaderName = "Authorization")
        {
            return new UnityHttpServiceProvider(baseUrl, authHeaderName, authHeaderValue, this);
        }

        private void Update()
        {
            if (!isCheckingQueue)
            {
                isCheckingQueue = true;
                StartCoroutine(ProcessQueue());
            }
        }

        private IEnumerator ProcessQueue()
        {
            while (requests.Count > 0)
            {
                yield return ProcessRequest(requests.Dequeue());
            }

            isCheckingQueue = false;
        }

        private IEnumerator ProcessRequest(UnityHttpServiceRequest request)
        {
            string method;
            switch (request.requestType)
            {
                case RequestType.POST:
                    method = "POST";
                    break;
                case RequestType.DELETE:
                    method = "DELETE";
                    break;
                case RequestType.GET:
                default:
                    method = "GET";
                    break;
            }
            
#if UNITY_WEBGL && !UNITY_EDITOR
            yield return SendRequestWebgl(method, request);
#else
            yield return SendRequestUnity(method, request);
#endif
        }

        private IEnumerator SendRequestUnity(string method, UnityHttpServiceRequest request)
        {
            string url = request.url;
            string @params = request.@params;
            bool isGet = request.requestType == RequestType.GET;
            string authHeaderKey = request.authKey;
            string authHeaderValue = request.authValue;
            
            using (UnityWebRequest uwr = !isGet
                       ? new UnityWebRequest(url, method)
                       : UnityWebRequest.Get(url))
            {
                if (!string.IsNullOrWhiteSpace(authHeaderValue) && !string.IsNullOrWhiteSpace(authHeaderKey))
                {
                    uwr.SetRequestHeader(authHeaderKey, authHeaderValue);
                }

                if (Infura.IsUrl(url))
                {
                    uwr.SetRequestHeader("X-Infura-User-Agent", $"metamask/sdk-csharp {MetaMaskUnity.Version}");
                    uwr.SetRequestHeader("Metamask-Sdk-Info", $"Sdk/Unity SdkVersion/{MetaMaskUnity.Version} Platform/{SystemInfo.operatingSystem} dApp/{MetaMaskUnity.Instance.Config.AppUrl}");
                }

                if (!string.IsNullOrWhiteSpace(@params))
                {
                    byte[] jsonToSend = Encoding.UTF8.GetBytes(@params);
                    uwr.uploadHandler = new UploadHandlerRaw(jsonToSend);
                    uwr.downloadHandler = new DownloadHandlerBuffer();
                    uwr.uploadHandler.contentType = "application/json";
                    uwr.SetRequestHeader("Content-Type", "application/json");
                }
                
                yield return uwr.SendWebRequest();

                switch (uwr.result)
                {
                    case UnityWebRequest.Result.ConnectionError: 
                    case UnityWebRequest.Result.DataProcessingError:
                    case UnityWebRequest.Result.ProtocolError:
                        request.requestTask.SetException(new IOException(uwr.error + " | " + uwr.downloadHandler.text));
                        break;
                    case UnityWebRequest.Result.Success:
                        request.requestTask.SetResult(uwr.downloadHandler.text);
                        break;
                }
            }
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        public static extern void _SendRequestFetch(string id, string objectName, string method, string url, string @params, bool isGet, string authHeaderKey, string authHeaderValue);

        private Dictionary<string, TaskCompletionSource<string>> fetchResults =
            new Dictionary<string, TaskCompletionSource<string>>();

        public class FetchResponse
        {
            public string responseJson;
            public string errorMessage;
            public string id;
        }
        
        private IEnumerator SendRequestWebgl(string method, UnityHttpServiceRequest request)
        {
            string url = request.url;
            string @params = request.@params;
            bool isGet = request.requestType == RequestType.GET;
            string authHeaderKey = request.authKey;
            string authHeaderValue = request.authValue;

            string id;
            do
            {
                id = Guid.NewGuid().ToString();
            } while (fetchResults.ContainsKey(id));

            var resultTaskSource = new TaskCompletionSource<string>();
            fetchResults.Add(id, resultTaskSource);
            
            _SendRequestFetch(id, gameObject.name, method, url, @params, isGet, authHeaderKey, authHeaderValue);

            yield return new WaitForTask<string>(resultTaskSource.Task);

            var resultJson = resultTaskSource.Task.Result;
            fetchResults.Remove(id);

            var result = JsonConvert.DeserializeObject<FetchResponse>(resultJson);

            if (!string.IsNullOrWhiteSpace(result.responseJson))
            {
                request.requestTask.TrySetResult(result.responseJson);
            } 
            else if (!string.IsNullOrWhiteSpace(result.errorMessage))
            {
                request.requestTask.TrySetException(new IOException(result.errorMessage));
            }
            else
            {
                request.requestTask.TrySetCanceled();
            }
        }

        [Preserve]
        public void OnFetchResponseCallback(string resultJson)
        {
            var result = JsonConvert.DeserializeObject<FetchResponse>(resultJson);

            if (fetchResults.ContainsKey(result.id))
            {
                fetchResults[result.id].TrySetResult(resultJson);
            }
        }
#endif
    }
}