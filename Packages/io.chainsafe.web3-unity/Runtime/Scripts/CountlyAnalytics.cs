using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Analytics;
using Plugins.CountlySDK;
using Plugins.CountlySDK.Models;
using UnityEngine;

public class CountlyAnalytics : IAnalyticsClient
{
    private const string AppKey = "9fad27b7383f5151a316505ca3245287fc44ba50";
    private const string ServerUrl = "https://trial-chainsafe.count.ly";

    
    public async Task CaptureEvent(AnalyticsEvent eventData)
    {
        await Countly.Instance.Events.RecordEventAsync(eventData.EventName, new Dictionary<string, object>()
        {
            { "chain", eventData.ChainId},
            { "network", eventData.Network},
            { "project-id", eventData.ProjectId},
            { "rpc ", eventData.Rpc},
            { "version", AnalyticsVersion},
        } );
    }

    public string AnalyticsVersion => "2.5.5";

    public CountlyAnalytics()
    {
        InitializeCountly();
    }
    
    private void InitializeCountly()
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
        }
    }
}