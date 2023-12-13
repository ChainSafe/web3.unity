using Notifications;
using Plugins.CountlySDK;
using Plugins.CountlySDK.Enums;
using Plugins.CountlySDK.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewTracking : MonoBehaviour
{
    public async void RecordViewA()
    {
        await Countly.Instance.Views.RecordOpenViewAsync("View A");

    }

    public async void RecordViewAWithSeg()
    {
        Dictionary<string, object> segments = new Dictionary<string, object>{
            { "Platform", "Windows"},
            { "Engine", "Unity"}
        };

        await Countly.Instance.Views.RecordOpenViewAsync("View A with segmentation", segmentation: segments);

    }

    public async void RecordViewB()
    {
        await Countly.Instance.Views.RecordOpenViewAsync("View B");

    }

    public async void RecordViewBWithSeg()
    {
        Dictionary<string, object> segments = new Dictionary<string, object>{
            { "Musician", "Album"},
            { "Director", "Movie"}
        };

        await Countly.Instance.Views.RecordOpenViewAsync("View B with segmentation", segmentation: segments);

    }

    public async void RecordViewC()
    {
        await Countly.Instance.Views.RecordOpenViewAsync("View C");

    }

    public async void RecordViewCWithSeg()
    {
        Dictionary<string, object> segments = new Dictionary<string, object>{
            { "Backpack", "12L"},
            { "Handbag", "10L"}
        };

        await Countly.Instance.Views.RecordOpenViewAsync("View C with segmentation", segmentation: segments);

    }


}
