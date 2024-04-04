using Newtonsoft.Json.Linq;
using Plugins.CountlySDK.Models;
using System;
using UnityEngine;

namespace Notifications.Impls.Android
{
    public class AndroidBridge : MonoBehaviour
    {
        private Action<string> _onTokenResult;
        private Action<string> _OnNotificationReceiveResult;
        private Action<string, int> _OnNotificationClickResult;

        public CountlyLogHelper Log { get; set; }

        public void ListenTokenResult(Action<string> result) => _onTokenResult = result;
        public void ListenReceiveResult(Action<string> result) => _OnNotificationReceiveResult = result;
        public void ListenClickResult(Action<string, int> result) => _OnNotificationClickResult = result;


        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void OnTokenResult(string token)
        {
            _onTokenResult?.Invoke(token);
            Log.Debug("[AndroidBridge] AndroidBridge Firebase token: " + token);

        }

        public void OnNotificationReceived(string data)
        {
            _OnNotificationReceiveResult?.Invoke(data);
            Log.Debug("[AndroidBridge] onMessageReceived");

        }

        public void OnNotificationClicked(string data)
        {
            int index = 0;

            JObject jObject = JObject.Parse(data);

            if (jObject != null) {
                index = (int)jObject.GetValue("click_index");
            }
            _OnNotificationClickResult?.Invoke(data, index);
            Log.Debug("[AndroidBridge] OnNotificationClicked");
        }
    }
}