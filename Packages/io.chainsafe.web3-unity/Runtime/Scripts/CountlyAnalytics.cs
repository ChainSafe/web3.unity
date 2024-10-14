using System.Collections.Generic;
using System.Linq;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Analytics;
using ChainSafe.Gaming.Web3.Core.Chains;
using Plugins.CountlySDK;
using Plugins.CountlySDK.Models;

public class CountlyAnalytics : IAnalyticsClient
{
    private const string AppKey = "4d2f30cecf1b7e2b8cd909103c1fac971872aa3f";
    private const string ServerUrl = "https://chainsafe-40aca7b26551e.flex.countly.com";

    private readonly IChainManager _chainManager;

    public async void CaptureEvent(AnalyticsEvent eventData)
    {
        await Countly.Instance.Events.RecordEventAsync(eventData.EventName);
    }

    public string AnalyticsVersion => "2.6";
    public IChainConfig ChainConfig => _chainManager.Current;
    public IProjectConfig ProjectConfig { get; }

    public CountlyAnalytics(IChainConfigSet chainConfigSet, IChainManager chainManager, IProjectConfig projectConfig)
    {
        Countly.Instance.Init(new CountlyConfiguration(AppKey, ServerUrl));

        _chainManager = chainManager;

        IChainConfig chainConfig = chainConfigSet.Configs.First();

        var userDetails = new Dictionary<string, object>
        {
            { "chainId", chainConfig.ChainId },
            { "rpc", chainConfig.Rpc },
            { "network", chainConfig.Network },
            { "projectId", projectConfig.ProjectId },
            { "analyticsVersion", AnalyticsVersion }
        };

        Countly.Instance.UserDetails.SetCustomUserDetails(userDetails);

        ProjectConfig = projectConfig;
    }

}