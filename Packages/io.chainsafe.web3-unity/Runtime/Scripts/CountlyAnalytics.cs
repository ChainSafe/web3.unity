using System.Collections.Generic;
using ChainSafe.Gaming.Web3.Analytics;
using Plugins.CountlySDK;
using Plugins.CountlySDK.Models;
using UnityEngine;

public class CountlyAnalytics : IAnalyticsClient
{
    private const string AppKey = "9fad27b7383f5151a316505ca3245287fc44ba50";
    private const string ServerUrl = "https://trial-chainsafe.count.ly";

    #if UNITY_EDITOR
    //This is only used for editor analytics. 
    private static CountlyAnalytics _instance;

    public static CountlyAnalytics Instance
    {
        get
        {
            if (_instance == null) InitializeCountly();
            return _instance;
        }
    }
    #endif


    public async void CaptureEvent(AnalyticsEvent eventData)
    {
        await Countly.Instance.Events.RecordEventAsync(eventData.EventName, new Dictionary<string, object>()
        {
            { "chain", eventData.ChainId},
            { "network", eventData.Network},
            { "project-id", eventData.ProjectId},
            { "rpc ", eventData.Rpc},
            { "version", AnalyticsVersion},
        } );
        Debug.Log($"Event captured {eventData.EventName}");
    }

    public string AnalyticsVersion => "2.5.5";

    //Have to initialize Countly as soon as possible 
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnBeforeSceneLoaded()
    {
        InitializeCountly();
    }

    public static void InitializeCountly()
    {
        if (!Countly.Instance.IsSDKInitialized)
        {
            var config = new CountlyConfiguration
            {
                AppKey = AppKey,
                ServerUrl = ServerUrl,
            };

            Countly.Instance.Init(config);
            Debug.Log("Countly initialized");
            #if UNITY_EDITOR
            _instance = new CountlyAnalytics();
            #endif
        }
    }
}