using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugins.CountlySDK.Enums;
using Plugins.CountlySDK.Helpers;
using Plugins.CountlySDK.Models;
using UnityEngine;

namespace Plugins.CountlySDK.Services
{
    public class CrashReportsCountlyService : AbstractBaseService
    {
        internal bool IsApplicationInBackground { get; set; }
        internal readonly Queue<string> _crashBreadcrumbs = new Queue<string>();

        internal readonly RequestCountlyHelper _requestCountlyHelper;

        internal CrashReportsCountlyService(CountlyConfiguration configuration, CountlyLogHelper logHelper, RequestCountlyHelper requestCountlyHelper, ConsentCountlyService consentService) : base(configuration, logHelper, consentService)
        {
            Log.Debug("[CrashReportsCountlyService] Initializing.");
            _requestCountlyHelper = requestCountlyHelper;
        }

        #region Helper Methods
        private string ManipulateStackTrace(string stackTrace)
        {
            string result = null;
            if (!string.IsNullOrEmpty(stackTrace))
            {
                string[] lines = stackTrace.Split('\n');

                int limit = lines.Length;

                if (limit > _configuration.MaxStackTraceLinesPerThread)
                {
                    limit = _configuration.MaxStackTraceLinesPerThread;
                }

                for (int i = 0; i < limit; ++i)
                {
                    string line = lines[i];

                    if (line.Length > _configuration.MaxStackTraceLineLength)
                    {
                        line = line.Substring(0, _configuration.MaxStackTraceLineLength);
                    }

                    if (i + 1 != limit)
                    {
                        line += '\n';
                    }

                    result += line;
                }
            }

            return result;
        }
        #endregion

        /// <summary>
        /// Public method that sends crash details to the server. Set param "nonfatal" to true for Custom Logged errors
        /// </summary>
        /// <param name="message">a string that contain detailed description of the exception.</param>
        /// <param name="stackTrace">a string that describes the contents of the callstack.</param>
        /// <param name="type">the type of the log message</param>
        /// <param name="segments">custom key/values to be reported</param>
        /// <param name="nonfatal">Fof automatically captured errors, you should set to <code>false</code>, whereas on logged errors it should be <code>true</code></param>
        /// <returns></returns>
        [Obsolete("SendCrashReportAsync(string message, string stackTrace, LogType type, IDictionary<string, object> segments = null, bool nonfatal = true) is deprecated, this is going to be removed in the future.")]
        public async Task SendCrashReportAsync(string message, string stackTrace, LogType type,
            IDictionary<string, object> segments = null, bool nonfatal = true)
        {
            lock (LockObj)
            {
                Log.Info("[CrashReportsCountlyService] SendCrashReportAsync : message = " + message + ", stackTrace = " + stackTrace);

                if (!_consentService.CheckConsentInternal(Consents.Crashes))
                {
                    return;
                }

                if (string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message))
                {
                    Log.Warning("[CrashReportsCountlyService] SendCrashReportAsync : The parameter 'message' can't be null or empty");
                    return;
                }

                IDictionary<string, object> segmentation = RemoveSegmentInvalidDataTypes(segments);
                segmentation = FixSegmentKeysAndValues(segments);

                CountlyExceptionDetailModel model = ExceptionDetailModel(message, ManipulateStackTrace(stackTrace), nonfatal, segmentation);
                _ = SendCrashReportInternal(model);
            }

            await Task.CompletedTask;
        }
        /// <summary>
        /// Public method that sends crash details to the server. Set param "nonfatal" to true for Custom Logged errors
        /// </summary>
        /// <param name="message">a string that contain detailed description of the exception.</param>
        /// <param name="stackTrace">a string that describes the contents of the callstack.</param>
        /// <param name="segments">custom key/values to be reported</param>
        /// <param name="nonfatal">Fof automatically captured errors, you should set to <code>false</code>, whereas on logged errors it should be <code>true</code></param>
        /// <returns></returns>
        public async Task SendCrashReportAsync(string message, string stackTrace, IDictionary<string, object> segments = null, bool nonfatal = true)
        {
            if (_configuration.EnableAutomaticCrashReporting)
            {
                lock (LockObj)
                {
                    Log.Info("[CrashReportsCountlyService] SendCrashReportAsync : message = " + message + ", stackTrace = " + stackTrace);

                    if (!_consentService.CheckConsentInternal(Consents.Crashes))
                    {
                        return;
                    }

                    if (string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message))
                    {
                        Log.Warning("[CrashReportsCountlyService] SendCrashReportAsync : The parameter 'message' can't be null or empty");
                        return;
                    }

                    IDictionary<string, object> segmentation = RemoveSegmentInvalidDataTypes(segments);
                    segmentation = FixSegmentKeysAndValues(segments);

                    CountlyExceptionDetailModel model = ExceptionDetailModel(message, ManipulateStackTrace(stackTrace), nonfatal, segmentation);
                    _ = SendCrashReportInternal(model);
                }
            }

            await Task.CompletedTask;
        }
        internal async Task SendCrashReportInternal(CountlyExceptionDetailModel model)
        {
            Log.Debug("[CrashReportsCountlyService] SendCrashReportInternal : model = " + model.ToString());

            Dictionary<string, object> requestParams = new Dictionary<string, object>
            {
                {
                    "crash", JsonConvert.SerializeObject(model, Formatting.Indented,
                        new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore})
                }
            };

            _requestCountlyHelper.AddToRequestQueue(requestParams);
            await _requestCountlyHelper.ProcessQueue();

        }

        /// <summary>
        /// Adds string value to a list which is later sent over as logs whenever a cash is reported by system.
        /// </summary>
        /// <param name="value">a bread crumb for the crash report</param>
        public void AddBreadcrumbs(string value)
        {
            Log.Info("[CrashReportsCountlyService] AddBreadcrumbs : " + value);

            if (!_consentService.CheckConsentInternal(Consents.Crashes))
            {
                return;
            }

            if (_configuration.EnableTestMode)
            {
                return;
            }

            string validBreadcrumb = value.Length > _configuration.MaxValueSize ? value.Substring(0, _configuration.MaxValueSize) : value;

            if (_crashBreadcrumbs.Count == _configuration.TotalBreadcrumbsAllowed)
            {
                _crashBreadcrumbs.Dequeue();
            }

            _crashBreadcrumbs.Enqueue(validBreadcrumb);
        }

        /// <summary>
        /// Create an CountlyExceptionDetailModel object from parameters.
        /// </summary>
        /// <param name="message">a string that contain detailed description of the exception.</param>
        /// <param name="stackTrace">a string that describes the contents of the callstack.</param>
        /// <param name="nonfatal">for automatically captured errors, you should set to <code>false</code>, whereas on logged errors it should be <code>true</code></param>
        /// <param name="segments">custom key/values to be reported</param>
        /// <returns>CountlyExceptionDetailModel</returns>
        internal CountlyExceptionDetailModel ExceptionDetailModel(string message, string stackTrace, bool nonfatal, IDictionary<string, object> segments)
        {
            return new CountlyExceptionDetailModel
            {
                OS = _configuration.metricHelper.OS,
                OSVersion = SystemInfo.operatingSystem,
                Device = SystemInfo.deviceName,
                Resolution = Screen.currentResolution.ToString(),
                AppVersion = Application.version,
                Cpu = SystemInfo.processorType,
                Opengl = SystemInfo.graphicsDeviceVersion,
                RamTotal = SystemInfo.systemMemorySize.ToString(),
                Battery = SystemInfo.batteryLevel.ToString(),
                Orientation = Screen.orientation.ToString(),
                Online = (Application.internetReachability > 0).ToString(),

                Name = message,
                Error = stackTrace,
                Nonfatal = nonfatal,
                RamCurrent = null,
                DiskCurrent = null,
                DiskTotal = null,
                Muted = null,
                Background = IsApplicationInBackground.ToString(),
                Root = null,
                Logs = string.Join("\n", _crashBreadcrumbs),
                Custom = segments as Dictionary<string, object>,
                Run = Time.realtimeSinceStartup.ToString(),
#if UNITY_IOS
                Manufacture = UnityEngine.iOS.Device.generation.ToString()
#endif
#if UNITY_ANDROID
                Manufacture = SystemInfo.deviceModel
#endif
            };
        }
        #region override Methods
        internal override void DeviceIdChanged(string deviceId, bool merged)
        {

        }
        #endregion
    }
}
