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

public class CustomEvents : MonoBehaviour
{
    public async void BasicEvent()
    {
        await Countly.Instance.Events.RecordEventAsync("Sample App’ event");
    }

    public async void EventWithSum()
    {
        await Countly.Instance.Events.RecordEventAsync("Event With Sum", sum: 23);
    }

    public async void EventWithSegmentation()
    {

        Dictionary<string, object> segment = new Dictionary<string, object>
        {
            { "Class", "Wizard"},
            { "Level", "10"}
        };

        await Countly.Instance.Events.RecordEventAsync("Event With Segmentation", segmentation: segment);
    }

    public async void EventWithDuration()
    {
        await Countly.Instance.Events.RecordEventAsync("Event With Duration", duration: 10);
    }

    public async void EventWithSegmentationCountSumDuration()
    {
        Dictionary<string, object> segments = new Dictionary<string, object>{
            { "Application", "Tiktok"},
            { "Country", "Japan"}
        };

        await Countly.Instance.Events.RecordEventAsync("Event With Sum, Duration, Count And Segmentation", segmentation: segments, sum: 23, count: 2, duration: 10);
    }

    public async void EventWithSumAndSegmentation()
    {
        Dictionary<string, object> segments = new Dictionary<string, object>{
            { "Monthly Visit", "30"},
            { "Name", "John Doe"}
        };

        await Countly.Instance.Events.RecordEventAsync("Event With Sum And Segmentation", segmentation: segments, sum: 23);

    }

    public async void EventWithSegmentationSumAndCount()
    {
        Dictionary<string, object> segments = new Dictionary<string, object>{
            { "Blood Type", "B-"},
            { "GPA", "3.2"}
        };

        await Countly.Instance.Events.RecordEventAsync("Event With Sum And Segmentation", segmentation: segments, sum: 23, count: 5);

    }

    public async void EventWithInvalidSegmentation()
    {
        int moles = 1; //valid data type
        string name = "foo";// valid data type
        bool isMale = true; // valid data type
        float amount = 10000.75f; //valid data type
        double totalAmount = 100000.76363;
        long currentMillis = DateTime.UtcNow.Millisecond; // invalid data type
        DateTime date = DateTime.UtcNow; // invalid data type

        Dictionary<string, object> segment = new Dictionary<string, object>
        {
            { "name", name},
            { "moles", moles},
            { "male", isMale},
            { "amount", amount},
            { "total amount", totalAmount},
            { "dob", date},
            { "Current Millis", currentMillis},
        };

        await Countly.Instance.Events.RecordEventAsync("Event With Invalid Segmentation", segmentation: segment);
    }

    public void StartTimedEvent()
    {
        Countly.Instance.Events.StartEvent("Timed Event");
    }

    public void EndTimedEvent()
    {
        Countly.Instance.Events.EndEvent("Timed Event");
    }

    public void CancelTimedEvent()
    {
        Countly.Instance.Events.CancelEvent("Timed Event");
    }
}
