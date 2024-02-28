using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugins.CountlySDK.Models;
using Plugins.CountlySDK.Persistance.Repositories;
using UnityEngine;
using UnityEngine.Networking;

namespace Plugins.CountlySDK.Helpers
{
    public class RequestCountlyHelper
    {
        private bool _isQueueBeingProcess;

        private readonly CountlyLogHelper Log;
        private readonly CountlyUtils _countlyUtils;
        private readonly CountlyConfiguration _config;
        private readonly RequestBuilder _requestBuilder;
        internal readonly RequestRepository _requestRepo;

        private readonly MonoBehaviour _monoBehaviour;

        internal RequestCountlyHelper(CountlyConfiguration config, CountlyLogHelper log, CountlyUtils countlyUtils, RequestBuilder requestBuilder, RequestRepository requestRepo, MonoBehaviour monoBehaviour)
        {
            Log = log;
            _config = config;
            _requestRepo = requestRepo;
            _countlyUtils = countlyUtils;
            _requestBuilder = requestBuilder;
            _monoBehaviour = monoBehaviour;
        }

        internal void AddRequestToQueue(CountlyRequestModel request)
        {
            Log.Verbose("[RequestCountlyHelper] AddRequestToQueue: " + request.ToString());

            if (_config.EnableTestMode)
            {
                return;
            }

            if (_requestRepo.Count == _config.StoredRequestLimit)
            {

                Log.Warning("[RequestCountlyHelper] Request Queue is full. Dropping the oldest request.");

                _requestRepo.Dequeue();
            }

            _requestRepo.Enqueue(request);
        }

        /// <summary>
        ///  An internal method which iterates a queue of requests and sends them for processing.
        ///  If a request fails, processing stops, and only successful requests are removed from the queue.
        /// </summary>
        internal async Task ProcessQueue()
        {
            if (_isQueueBeingProcess)
            {
                return;
            }

            _isQueueBeingProcess = true;
            CountlyRequestModel[] requests = _requestRepo.Models.ToArray();

            Log.Verbose("[RequestCountlyHelper] Process queue, requests: " + requests.Length);

            foreach (CountlyRequestModel reqModel in requests)
            {
                //add the remaining request count in RequestData
                reqModel.RequestData += "&rr=" + (requests.Length - 1);

                CountlyResponse response = await ProcessRequest(reqModel);

                if (!response.IsSuccess)
                {
                    Log.Verbose("[RequestCountlyHelper] ProcessQueue: Request fail, " + response.ToString());
                    break;
                }

                _requestRepo.Dequeue();
            }

            _isQueueBeingProcess = false;
        }

        /// <summary>
        /// Decides whether to use a POST or GET method based on configuration and request size, and then send the request accordingly.
        /// </summary>
        private async Task<CountlyResponse> ProcessRequest(CountlyRequestModel model)
        {
            Log.Verbose("[RequestCountlyHelper] Process request, request: " + model);
            bool shouldPost = _config.EnablePost || model.RequestData.Length > 2000;

#if UNITY_WEBGL
            return await StartProcessRequestRoutine(_countlyUtils.ServerInputUrl, model.RequestData);
#else
            if (shouldPost)
            {
                return await Task.Run(() => PostAsync(_countlyUtils.ServerInputUrl, model.RequestData));
            }
            return await Task.Run(() => GetAsync(_countlyUtils.ServerInputUrl, model.RequestData));
#endif
        }

        /// <summary>
        ///  An internal function to add a request to request queue.
        /// </summary>
        internal void AddToRequestQueue(Dictionary<string, object> queryParams)
        {
            CountlyRequestModel requestModel = _requestBuilder.BuildRequest(_countlyUtils.GetBaseParams(), queryParams);

            AddRequestToQueue(requestModel);
        }

        private string AddChecksum(string query)
        {
            if (!string.IsNullOrEmpty(_config.Salt))
            {
                // Create a SHA256
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(query + _config.Salt));
                    string hex = _countlyUtils.GetStringFromBytes(bytes);

                    query += "&checksum256=" + hex;
                    Log.Debug("BuildGetRequest: query = " + query);
                }
            }

            return query;
        }

        /// <summary>
        ///     Makes an Asynchronous GET request to the Countly server.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="addToRequestQueue"></param>
        /// <returns></returns>
        internal async Task<CountlyResponse> GetAsync(string uri, string data)
        {
            Log.Verbose("[RequestCountlyHelper] GetAsync request: " + uri + " params: " + data);

            CountlyResponse countlyResponse = new CountlyResponse();
            string query = AddChecksum(data);
            string url = uri + query;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    int code = (int)response.StatusCode;
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string res = await reader.ReadToEndAsync();

                        JObject body = JObject.Parse(res);

                        countlyResponse.Data = res;
                        countlyResponse.StatusCode = code;
                        countlyResponse.IsSuccess = IsSuccess(countlyResponse);
                    }

                }
            }
            catch (WebException ex)
            {
                countlyResponse.ErrorMessage = ex.Message;
                if (ex.Response != null)
                {
                    HttpWebResponse response = (HttpWebResponse)ex.Response;
                    int code = (int)response.StatusCode;
                    using (Stream stream = ex.Response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string res = await reader.ReadToEndAsync();
                        countlyResponse.StatusCode = code;
                        countlyResponse.Data = res;
                        countlyResponse.IsSuccess = IsSuccess(countlyResponse);
                    }
                }

            }

            Log.Verbose("[RequestCountlyHelper] GetAsync request: " + url + " params: " + query + " response: " + countlyResponse.ToString());

            return countlyResponse;
        }

        /// <summary>
        ///     Makes an Asynchronous POST request to the Countly server.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <param name="addToRequestQueue"></param>
        /// <returns></returns>
        internal async Task<CountlyResponse> PostAsync(string uri, string data)
        {
            CountlyResponse countlyResponse = new CountlyResponse();

            try
            {
                string query = AddChecksum(data);
                byte[] dataBytes = Encoding.ASCII.GetBytes(query);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.ContentLength = dataBytes.Length;
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "POST";


                using (Stream requestBody = request.GetRequestStream())
                {
                    await requestBody.WriteAsync(dataBytes, 0, dataBytes.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    int code = (int)response.StatusCode;
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string res = await reader.ReadToEndAsync();

                        JObject body = JObject.Parse(res);

                        countlyResponse.Data = res;
                        countlyResponse.StatusCode = code;
                        countlyResponse.IsSuccess = IsSuccess(countlyResponse);
                    }
                }
            }
            catch (WebException ex)
            {
                countlyResponse.ErrorMessage = ex.Message;
                if (ex.Response != null)
                {
                    HttpWebResponse response = (HttpWebResponse)ex.Response;
                    int code = (int)response.StatusCode;
                    using (Stream stream = ex.Response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string res = await reader.ReadToEndAsync();
                        countlyResponse.StatusCode = code;
                        countlyResponse.Data = res;
                        countlyResponse.IsSuccess = IsSuccess(countlyResponse);
                    }
                }

            }

            Log.Verbose("[RequestCountlyHelper] PostAsync request: " + uri + " body: " + data + " response: " + countlyResponse.ToString());

            return countlyResponse;
        }

        /// <summary>
        /// Asynchronously initiates a request routine, combining URL and data, and starts a coroutine to process the request with checksum validation.
        /// </summary>
        /// <param name="shouldPost"></param>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private Task<CountlyResponse> StartProcessRequestRoutine(string uri, string data)
        {
            TaskCompletionSource<CountlyResponse> tcs = new TaskCompletionSource<CountlyResponse>();

            _monoBehaviour.StartCoroutine(ProcessRequestCoroutine(uri, data, (response) =>
            {
                tcs.SetResult(response);
            }));

            return tcs.Task;
        }

        /// <summary>
        /// Processes a UnityWebRequest coroutine, handling response and errors, and invoking a callback with the CountlyResponse.
        /// </summary>
        /// <param name="shouldPost"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        private IEnumerator ProcessRequestCoroutine(string uri, string data, Action<CountlyResponse> callback)
        {
            CountlyResponse countlyResponse = new CountlyResponse();

            string query = AddChecksum(data);
            string url = uri + query;

            using (UnityWebRequest webRequest = UnityWebRequest.Put(url, data))
            {

                webRequest.method = UnityWebRequest.kHttpVerbPOST;

                yield return webRequest.SendWebRequest();

                string[] pages = url.Split('?');
                int page = pages.Length - 1;
                int code = (int)webRequest.responseCode;

                countlyResponse.Data = webRequest.downloadHandler.text;
                countlyResponse.ErrorMessage = webRequest.error;
                countlyResponse.StatusCode = code;
                countlyResponse.IsSuccess = IsSuccess(countlyResponse);

                Log.Debug($"[RequestCountlyHelper] ProcessRequestCoroutine request url: [{uri}] body: [{pages[page]}] response: [{countlyResponse}]");
            }

            callback?.Invoke(countlyResponse);
        }

        private bool IsSuccess(CountlyResponse countlyResponse)
        {
            if (countlyResponse.StatusCode >= 200 && countlyResponse.StatusCode < 300)
            {
                try
                {
                    JObject json = JObject.Parse(countlyResponse.Data);

                    if (json.ContainsKey("result"))
                    {
                        return true;
                    }
                }
                catch (JsonException)
                {
                    Log.Debug("[RequestCountlyHelper] IsSuccess : Returned request is not a JSON object");
                    return false;
                }
            }

            return false;
        }
    }
}