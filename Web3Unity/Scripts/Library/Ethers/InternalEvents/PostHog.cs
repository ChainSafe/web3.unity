using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Web3Unity.Scripts.Library.Ethers.InternalEvents
{
    public class PostHog
    {
        private readonly string _baseUrl = "https://app.posthog.com";
        private readonly string _apiKey;

        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        public static PostHog Client = new PostHog("phc_BjFujqKYxjetZ9WkGXXsVcn3vObD8Taes6nocf8LrhH");

        public PostHog(string apiKey, string baseUrl = null)
        {
            _apiKey = apiKey;

            if (baseUrl != null)
            {
                _baseUrl = baseUrl;
            }
        }

        public async void Capture(string eventName, Dictionary<string, object> properties)
        {
            properties["$lib"] = "web3.unity-posthog";
            properties["$lib_version"] = "2.0.0-beta"; // TODO: get version dynamically

            var userUuid = PlayerPrefs.GetString("ph_user_uuid", null);
            if (string.IsNullOrEmpty(userUuid))
            {
                userUuid = Guid.NewGuid().ToString();
                PlayerPrefs.SetString("ph_user_uuid", userUuid);
                PlayerPrefs.Save();
            }

            try
            {
                var json = JsonConvert.SerializeObject(new PostHogEvent
                {
                    ApiKey = _apiKey,
                    Event = eventName,
                    Properties = properties,
                    DistinctId = userUuid
                }, _jsonSerializerSettings);

                var req = new UnityWebRequest(_baseUrl + "/batch/", "POST");
                req.uploadHandler = new UploadHandlerRaw(new UTF8Encoding().GetBytes(json));
                req.downloadHandler = new DownloadHandlerBuffer();
                req.SetRequestHeader("Content-Type", "application/json");
                await req.SendWebRequest();

                if (req.result != UnityWebRequest.Result.Success)
                {
                    req.Dispose();
                    throw new Exception(req.error);
                }

                req.Dispose();
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        public class PostHogEvent
        {
            [JsonProperty(PropertyName = "api_key")]
            public string ApiKey { get; set; }

            [JsonProperty(PropertyName = "properties")]
            public object Properties { get; set; }

            [JsonProperty(PropertyName = "timestamp")]
            public string TimeStamp { get; set; }

            [JsonProperty(PropertyName = "context")]
            public object Context { get; set; }

            [JsonProperty(PropertyName = "distinct_id")]
            public string DistinctId { get; set; }

            [JsonProperty(PropertyName = "type")] public string Type { get; set; }

            [JsonProperty(PropertyName = "event")] public string Event { get; set; }

            [JsonProperty(PropertyName = "messageId")]
            public string MessageId { get; set; }
        }
    }
}