using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Analytics;
using Plugins.CountlySDK;
using Plugins.CountlySDK.Models;
using UnityEngine;

public class CountlyAnalytics : IAnalyticsClient
{
    private const string AppKey = "9fad27b7383f5151a316505ca3245287fc44ba50";
    private const string ServerUrl = "https://chainsafe-40aca7b26551e.flex.countly.com/";

    
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
    public IChainConfig ChainConfig { get; }
    public IProjectConfig ProjectConfig { get; }

    public CountlyAnalytics(IChainConfig chainConfig, IProjectConfig projectConfig)
    {
        Countly.Instance.Init(new CountlyConfiguration(AppKey, ServerUrl).EnableLogging());
        Debug.Log("Countly initialized");

        ChainConfig = chainConfig;
        ProjectConfig = projectConfig;
    }
    
}