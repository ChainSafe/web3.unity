using Newtonsoft.Json.Linq;
using Plugins.CountlySDK.Helpers;
using Plugins.CountlySDK.Models;
using Plugins.CountlySDK.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Notifications.Impls.Android
{
    public class AndroidNotificationsService : INotificationsService
    {
        private readonly Transform _countlyGameObject;
        private const string BridgeName = "[Android] Bridge";
        private const string StorePackageName = "ly.count.unity.push_fcm.MessageStore";
        private const string CountlyPushPluginPackageName = "ly.count.unity.push_fcm.CountlyPushPlugin";
        private const string NotficationServicePackageName = "ly.count.unity.push_fcm.RemoteNotificationsService";

        private readonly CountlyLogHelper Log;
        private readonly AndroidBridge _bridge;
        private readonly EventCountlyService _eventCountlyService;

        public bool IsInitializedWithoutError { get; set; }

        internal AndroidNotificationsService(Transform countlyGameObject, CountlyConfiguration config, CountlyLogHelper log, EventCountlyService eventCountlyService)
        {
            Log = log;

            Log.Debug("[AndroidNotificationsService] Initializing.");

            _countlyGameObject = countlyGameObject;
            _eventCountlyService = eventCountlyService;

            GameObject gameObject = new GameObject(BridgeName);
            gameObject.transform.parent = _countlyGameObject;
            _bridge = gameObject.AddComponent<AndroidBridge>();
            _bridge.Log = Log;

            try {
                AndroidJavaClass countlyPushPlugin = new AndroidJavaClass(CountlyPushPluginPackageName);
                countlyPushPlugin.CallStatic("setEnableLog", config.EnableConsoleLogging);
                IsInitializedWithoutError = true;
            } catch (Exception ex) {
                Log.Error("[AndroidNotificationsService] Exception in initializing service: " + ex.Message);
                IsInitializedWithoutError = false;
            }
        }

        public void GetToken(Action<string> result)
        {

            Log.Debug("[AndroidNotificationsService] GetToken");

#if !UNITY_EDITOR
            _bridge.ListenTokenResult(result);

            using (AndroidJavaObject jc = new AndroidJavaObject(NotficationServicePackageName)) {
                jc.Call("getToken");
            }
#endif
        }

        public void OnNotificationClicked(Action<string, int> result)
        {
            Log.Debug("[AndroidNotificationsService] OnNotificationClicked");

            _bridge.ListenClickResult(result);
        }

        public void OnNotificationReceived(Action<string> result)
        {
            Log.Debug("[AndroidNotificationsService] OnNotificationReceived");

            _bridge.ListenReceiveResult(result);
        }

        public async Task<CountlyResponse> ReportPushActionAsync()
        {
            Log.Debug("[AndroidNotificationsService] ReportPushActionAsync");

            AndroidJavaClass store = new AndroidJavaClass(StorePackageName);

            bool isInitialized = store.CallStatic<bool>("isInitialized");
            if (!isInitialized) {
                AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaObject applicationContext = activity.Call<AndroidJavaObject>("getApplicationContext");

                store.CallStatic("init", applicationContext);
            }

            string data = store.CallStatic<string>("getMessagesData");
            if (string.IsNullOrEmpty(data)) {
                return new CountlyResponse {
                    IsSuccess = false,
                    ErrorMessage = "Key is required."
                };
            }

            JArray jArray = JArray.Parse(data);

            if (jArray != null) {
                foreach (JObject item in jArray) {
                    string mesageId = item.GetValue("messageId").ToString();
                    string identifier = item.GetValue("action_index").ToString();
                    Dictionary<string, object> segment = new Dictionary<string, object>()
                    {
                        {"b", mesageId},
                        {"i", identifier},
                        {"p", "a"}
                    };

                    Log.Debug("[AndroidNotificationsService] ReportPushActionAsync key: " + CountlyEventModel.PushActionEvent + ", segments: " + segment);


                    CountlyEventModel eventModel = new CountlyEventModel(CountlyEventModel.PushActionEvent, segment);
                    await _eventCountlyService.RecordEventAsync(eventModel);
                }

                store.CallStatic("clearMessagesData");

            }

            return new CountlyResponse {
                IsSuccess = true,
            };
        }
    }
}