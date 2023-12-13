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

public class DeviceId : MonoBehaviour
{
    public async void ChangeDeviceIdWithMerge()
    {
        await Countly.Instance.Device.ChangeDeviceIdWithMerge("device-id");

    }

    public async void ChangeDeviceIdWithoutMerge()
    {
        await Countly.Instance.Device.ChangeDeviceIdWithoutMerge("new_device_id");

    }

}
