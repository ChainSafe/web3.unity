using System;
using Newtonsoft.Json;
using Plugins.CountlySDK.Helpers;
using UnityEngine;

namespace Plugins.CountlySDK.Models
{
    [Serializable]
    internal class CountlyMetricModel
    {
        public CountlyMetricModel()
        {
            OS = Constants.UnityPlatform;
            OSVersion = SystemInfo.operatingSystem;
            Device = SystemInfo.deviceModel;
            Resolution = Screen.currentResolution.ToString();
            AppVersion = GetAppVersion();
            Density = Screen.dpi.ToString();
            Locale = Application.systemLanguage.ToString();

            // Not found metrics data
            Carrier = null;
            Store = null;
            Browser = null;
            BrowserVersion = null;
        }
        public static string GetAppVersion()
        {
            return Application.version;
        }

        [JsonProperty("_os")] public string OS { get; set; }

        [JsonProperty("_os_version")] public string OSVersion { get; set; }

        [JsonProperty("_device")] public string Device { get; set; }

        [JsonProperty("_resolution")] public string Resolution { get; set; }

        [JsonProperty("_carrier")] public string Carrier { get; set; }

        [JsonProperty("_app_version")] public string AppVersion { get; set; }

        [JsonProperty("_density")] public string Density { get; set; }

        [JsonProperty("_store")] public string Store { get; set; }

        [JsonProperty("_browser")] public string Browser { get; set; }

        [JsonProperty("_browser_version")] public string BrowserVersion { get; set; }

        [JsonProperty("_locale")] public string Locale { get; set; }
    }
}
