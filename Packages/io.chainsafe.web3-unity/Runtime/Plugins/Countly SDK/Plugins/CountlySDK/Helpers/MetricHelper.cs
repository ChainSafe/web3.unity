using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Plugins.CountlySDK.Helpers
{
    public class MetricHelper
    {
        internal Dictionary<string, string> overridenMetrics;
        readonly string unityPlatform;

        public MetricHelper() : this(null)
        {

        }

        public MetricHelper(Dictionary<string, string> overridenMetrics)
        {
            string platform = Application.platform.ToString().ToLower();
            unityPlatform = (Application.platform == RuntimePlatform.IPhonePlayer) ? "iOS" : platform;

            this.overridenMetrics = overridenMetrics;
        }

        public string OS
        {
            get {
                if (overridenMetrics != null && overridenMetrics.ContainsKey("_os")) {
                    return overridenMetrics["_os"];
                }
                return unityPlatform;
            }
        }

        public string OSVersion
        {
            get {
                if (overridenMetrics != null && overridenMetrics.ContainsKey("_os_version")) {
                    return overridenMetrics["_os_version"];
                }
                return SystemInfo.operatingSystem;
            }
        }

        public string Device
        {
            get {
                if (overridenMetrics != null && overridenMetrics.ContainsKey("_device")) {
                    return overridenMetrics["_device"];
                }
                return SystemInfo.deviceModel;
            }
        }

        public string Resolution
        {
            get {
                if (overridenMetrics != null && overridenMetrics.ContainsKey("_resolution")) {
                    return overridenMetrics["_resolution"];
                }
                return Screen.currentResolution.ToString();
            }
        }

        public string AppVersion
        {
            get {
                if (overridenMetrics != null && overridenMetrics.ContainsKey("_app_version")) {
                    return overridenMetrics["_app_version"];
                }
                return Application.version;
            }
        }

        public string Density
        {
            get {
                if (overridenMetrics != null && overridenMetrics.ContainsKey("_density")) {
                    return overridenMetrics["_density"];
                }
                return Screen.dpi.ToString();
            }
        }

        public string Locale
        {
            get {
                if (overridenMetrics != null && overridenMetrics.ContainsKey("_locale")) {
                    return overridenMetrics["_locale"];
                }
                return Application.systemLanguage.ToString();
            }
        }

        /// <summary>
        /// Generates a JSON representation of device and application metrics, combining default and overridden metrics.
        /// </summary>
        public string buildMetricJSON()
        {
            Dictionary<string, string> metrics = new Dictionary<string, string>
            {
                { "_os", OS },
                { "_os_version", OSVersion},
                { "_device", Device},
                { "_resolution", Resolution},
                { "_app_version", AppVersion},
                { "_density", Density},
                { "_locale", Locale}
            };

            if (overridenMetrics != null) {
                foreach (KeyValuePair<string, string> kvp in overridenMetrics) {
                    if (!metrics.ContainsKey(kvp.Key)) {
                        metrics[kvp.Key] = kvp.Value;
                    }
                }
            }

            return JsonConvert.SerializeObject(metrics, Formatting.Indented,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }
    }
}

