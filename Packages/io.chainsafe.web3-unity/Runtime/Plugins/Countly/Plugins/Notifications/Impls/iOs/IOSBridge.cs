using UnityEngine;
using System;
using Plugins.CountlySDK.Models;
using Newtonsoft.Json.Linq;

public class IOSBridge : MonoBehaviour
{
    public string MessageId { get; set; }
    public string ButtonIndex { get; set; }

    public CountlyLogHelper Log { get; set; }

    private Action<string> _onTokenResult;
    private Action<string> _OnNotificationReceiveResult;
    private Action<string, int> _OnNotificationClickResult;

    public void ListenTokenResult(Action<string> result) => _onTokenResult = result;
    public void ListenReceiveResult(Action<string> result) => _OnNotificationReceiveResult = result;
    public void ListenClickResult(Action<string, int> result) => _OnNotificationClickResult = result;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

#if !UNITY_EDITOR && COUNTLY_ENABLE_IOS_PUSH
    [System.Runtime.InteropServices.DllImport("__Internal")]
    extern static public void registerForRemoteNotifications();
#endif

    public void GetToken()
    {

#if !UNITY_EDITOR && COUNTLY_ENABLE_IOS_PUSH
        registerForRemoteNotifications();
#endif

    }


    //Sent when the application successfully registered with Apple Push Notification Service (APNS).
    void OnDidRegisterForRemoteNotificationsWithDeviceToken(string deviceToken)
    {
        Log.Debug("[IOSBridge] OnDidRegisterForRemoteNotificationsWithDeviceToken Token: " + deviceToken);

        if (deviceToken != null && deviceToken.Length != 0) {
            _onTokenResult?.Invoke(deviceToken);
        }
    }

    //Sent when the application failed to be registered with Apple Push Notification Service (APNS).
    void OnDidFailToRegisterForRemoteNotificcallBackationsWithError(string error)
    {
        Log.Debug("[IOSBridge] OnDidFailToRegisterForRemoteNotificcallBackationsWithError error: " + error);

    }


    void OnPushNotificationsReceived(string pushData)
    {
        Log.Debug("[IOSBridge] OnPushNotificationsReceived: " + pushData);

        _OnNotificationReceiveResult?.Invoke(pushData);

    }

    void OnPushNotificationsClicked(string pushData)
    {
        Log.Debug("[IOSBridge] OnPushNotificationsClicked: " + pushData);

        JObject item = JObject.Parse(pushData);

        MessageId = (string)item["c"]["i"];
        ButtonIndex = (string)item["action_index"];

        _OnNotificationClickResult?.Invoke(pushData, int.Parse(ButtonIndex));

    }


    void OnDidRegisterUserNotificationSettings(string setting)
    {
        Log.Debug("[IOSBridge] OnDidRegisterUserNotificationSettings error: " + setting);
    }

}
