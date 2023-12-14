﻿using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Plugins.CountlySDK.Models;
using Plugins.CountlySDK.Persistance.Repositories;

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

        internal RequestCountlyHelper(CountlyConfiguration config, CountlyLogHelper log, CountlyUtils countlyUtils, RequestBuilder requestBuilder, RequestRepository requestRepo)
        {
            Log = log;
            _config = config;
            _requestRepo = requestRepo;
            _countlyUtils = countlyUtils;
            _requestBuilder = requestBuilder;

        }

        internal void AddRequestToQueue(CountlyRequestModel request)
        {

            Log.Verbose("[RequestCountlyHelper] AddRequestToQueue: " + request.ToString());

            if (_config.EnableTestMode) {
                return;
            }

            if (_requestRepo.Count == _config.StoredRequestLimit) {

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
            if (_isQueueBeingProcess) {
                return;
            }

            _isQueueBeingProcess = true;
            CountlyRequestModel[] requests = _requestRepo.Models.ToArray();

            Log.Verbose("[RequestCountlyHelper] Process queue, requests: " + requests.Length);

            foreach (CountlyRequestModel reqModel in requests) {
                //add the remaining request count in RequestData
                reqModel.RequestData += "&rr=" + (requests.Length - 1);
                CountlyResponse response = await ProcessRequest(reqModel);

                if (!response.IsSuccess) {
                    Log.Verbose("[RequestCountlyHelper] ProcessQueue: Request fail, " + response.ToString());
                    break;
                }

                _requestRepo.Dequeue();
            }

            _isQueueBeingProcess = false;
        }

        /// <summary>
        ///  Decides whether to use a POST or GET method based on configuration and request size, and then send the request accordingly.
        /// </summary>
        private async Task<CountlyResponse> ProcessRequest(CountlyRequestModel model)
        {
            Log.Verbose("[RequestCountlyHelper] Process request, request: " + model);

            if (_config.EnablePost || model.RequestData.Length > 2000) {
                return await Task.Run(() => PostAsync(_countlyUtils.ServerInputUrl, model.RequestData));
            }

            return await Task.Run(() => GetAsync(_countlyUtils.ServerInputUrl, model.RequestData));
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
            if (!string.IsNullOrEmpty(_config.Salt)) {
                // Create a SHA256
                using (SHA256 sha256Hash = SHA256.Create()) {
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

            using (UnityWebRequest request = UnityWebRequest.Get(url)) {
                await request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success) {
                    countlyResponse.ErrorMessage = request.error;
                    countlyResponse.IsSuccess = false;
                } else {
                    countlyResponse.Data = request.downloadHandler.text;
                    countlyResponse.StatusCode = (int)request.responseCode;
                    JObject body = JObject.Parse(countlyResponse.Data);
                    countlyResponse.IsSuccess = body.ContainsKey("result");
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
            Log.Verbose("[RequestCountlyHelper] PostAsync request: " + uri + " body: " + data);

            CountlyResponse countlyResponse = new CountlyResponse();
            string query = AddChecksum(data);

            using (UnityWebRequest request = new UnityWebRequest(uri, "POST")) {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(query);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

                await request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success) {
                    countlyResponse.ErrorMessage = request.error;
                    countlyResponse.IsSuccess = false;
                } else {
                    countlyResponse.Data = request.downloadHandler.text;
                    countlyResponse.StatusCode = (int)request.responseCode;
                    JObject body = JObject.Parse(countlyResponse.Data);
                    countlyResponse.IsSuccess = body.ContainsKey("result");
                }
            }

            Log.Verbose("[RequestCountlyHelper] PostAsync request: " + uri + " body: " + data + " response: " + countlyResponse.ToString());

            return countlyResponse;
        }


    }
}
