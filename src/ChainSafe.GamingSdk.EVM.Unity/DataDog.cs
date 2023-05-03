using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Web3Unity.Scripts.Library.Ethers.Unity
{
    public class DataDog
    {
        private readonly string baseUrl = "https://http-intake.logs.datadoghq.com/api/v2/logs";
        private readonly string apiKey;

        private readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
        };

        public DataDog(string apiKey, string baseUrl = null)
        {
            this.apiKey = apiKey;

            if (baseUrl != null)
            {
                this.baseUrl = baseUrl;
            }
        }

        public async void Capture(string eventName, Dictionary<string, object> properties)
        {
            properties["$lib"] = "web3.unity-datadog";
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
                var json = JsonConvert.SerializeObject(
                    new DataDogEvent
                    {
                        DD_SOURCE = apiKey,
                        Event = eventName,
                        DDTAGS = properties,
                        PROJECT_ID = userUuid,
                    },
                    jsonSerializerSettings);

                var req = new UnityWebRequest(baseUrl + "/batch/", "POST");
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

        public class DataDogEvent
        {
            [JsonProperty(PropertyName = "ddsource")]
            public string DD_SOURCE { get; set; }

            [JsonProperty(PropertyName = "ddtags")]
            public object DDTAGS { get; set; }

            [JsonProperty(PropertyName = "hostname")]
            public string HOST_NAME { get; set; }

            [JsonProperty(PropertyName = "message")]
            public object MESSAGE { get; set; }

            [JsonProperty(PropertyName = "service")]
            public string SERVICE { get; set; }

            [JsonProperty(PropertyName = "type")]
            public string Type { get; set; }

            [JsonProperty(PropertyName = "event")]
            public string Event { get; set; }

            [JsonProperty(PropertyName = "projectID")]
            public string PROJECT_ID { get; set; }
        }
    }
}