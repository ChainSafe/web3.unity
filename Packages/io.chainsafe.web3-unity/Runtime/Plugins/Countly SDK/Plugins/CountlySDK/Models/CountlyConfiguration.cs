using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Notifications;
using Plugins.CountlySDK.Enums;
using Plugins.CountlySDK.Helpers;
using Plugins.CountlySDK.Services;
using UnityEngine;

namespace Plugins.CountlySDK.Models
{
    [Serializable]
    public class CountlyConfiguration
    {
        /// <summary>
        /// URL of the Countly server to submit data to.
        /// Mandatory field.
        /// </summary>
        [Obsolete("ServerUrl is deprecated. Use CountlyConfiguration(string appKey, string serverUrl) instead.")]
        public string ServerUrl = null;

        /// <summary>
        /// App key for the application being tracked.
        /// Mandatory field.
        /// </summary>
        [Obsolete("AppKey is deprecated. Use CountlyConfiguration(string appKey, string serverUrl) instead.")]
        public string AppKey = null;

        /// <summary>
        /// Unique ID for the device the app is running on.
        /// </summary>
        [Obsolete("DeviceId is deprecated. Use SetDeviceId(string deviceId) instead.")]
        public string DeviceId = null;

        /// <summary>
        /// Set to prevent parameter tampering.
        /// </summary>
        [Obsolete("Salt is deprecated. Use SetParameterTamperingProtectionSalt(string salt) instead.")]
        public string Salt = null;

        /// <summary>
        /// Set to send all requests made to the Countly server using HTTP POST.
        /// </summary>
        [Obsolete("EnablePost is deprecated. Use EnableForcedHttpPost() instead.")]
        public bool EnablePost = false;

        /// <summary>
        /// Set to true if you want the SDK to pretend that it's functioning.
        /// </summary>
        [Obsolete("EnableTestMode is deprecated. This is going to be removed in the future.")]
        public bool EnableTestMode = false;

        /// <summary>
        /// Set to true if you want to enable countly internal debugging logs.
        /// </summary>
        [Obsolete("EnableConsoleLogging is deprecated. Use EnableLogging() instead.")]
        public bool EnableConsoleLogging = false;

        /// <summary>
        /// Set mode of push notification.
        /// </summary>
        [Obsolete("NotificationMode is deprecated. Use SetNotificationMode(TestMode mode) instead.")]
        public TestMode NotificationMode = TestMode.None;

        /// <summary>
        /// Set to true to enable manual session handling.
        /// </summary>
        [Obsolete("EnableManualSessionHandling is deprecated. This is going to be removed in the future.")]
        public readonly bool EnableManualSessionHandling = false;

        /// <summary>
        /// Sets the interval for the automatic update calls
        /// min value 1 (1 second), max value 600 (10 minutes)
        /// </summary>
        [Obsolete("SessionDuration is deprecated. Use SetUpdateSessionTimerDelay(int duration) instead.")]
        public int SessionDuration = 60;

        /// <summary>
        /// Maximum size of all string keys
        /// </summary>
        [Obsolete("MaxKeyLength is deprecated. Use SetMaxKeyLength(int length) instead.")]
        public int MaxKeyLength = 128;

        /// <summary>
        /// Maximum size of all values in our key-value pairs
        /// </summary>
        [Obsolete("MaxValueSize is deprecated. Use SetMaxValueSize(int size) instead.")]
        public int MaxValueSize = 256;

        /// <summary>
        /// Max amount of custom (dev provided) segmentation in one event
        /// </summary>
        [Obsolete("MaxSegmentationValues is deprecated. Use SetMaxSegmentationValues(int values) instead.")]
        public int MaxSegmentationValues = 100;

        /// <summary>
        /// Limits how many stack trace lines would be recorded per thread
        /// </summary>
        [Obsolete("MaxStackTraceLinesPerThread is deprecated. Use SetMaxStackTraceLinesPerThread(int lines) instead.")]
        public int MaxStackTraceLinesPerThread = 30;

        /// <summary>
        /// Limits how many characters are allowed per stack trace line
        /// </summary>
        [Obsolete("MaxStackTraceLineLength is deprecated. Use SetMaxStackTraceLineLength(int length) instead.")]
        public int MaxStackTraceLineLength = 200;

        /// <summary>
        /// Set threshold value for the number of events that can be stored locally.
        /// </summary>
        [Obsolete("EventQueueThreshold is deprecated. Use SetEventQueueSizeToSend(int threshold) instead.")]
        public int EventQueueThreshold = 100;

        /// <summary>
        /// Set limit for the number of requests that can be stored locally.
        /// </summary>
        [Obsolete("StoredRequestLimit is deprecated. Use SetMaxRequestQueueSize(int limit) instead.")]
        public int StoredRequestLimit = 1000;

        /// <summary>
        /// Set the maximum amount of breadcrumbs.
        /// </summary>
        [Obsolete("TotalBreadcrumbsAllowed is deprecated. Use SetMaxBreadcrumbCount(int amount) instead.")]
        public int TotalBreadcrumbsAllowed = 100;

        /// <summary>
        /// Set true to enable uncaught crash reporting.
        /// </summary>
        [Obsolete("EnableAutomaticCrashReporting is deprecated. Use DisableAutomaticCrashReporting() instead.")]
        public bool EnableAutomaticCrashReporting = true;

        /// <summary>
        /// Set if consent should be required.
        /// </summary>
        [Obsolete("RequiresConsent is deprecated. Use SetRequiresConsent(bool enable) instead.")]
        public bool RequiresConsent = false;

        internal SafeIDGenerator SafeViewIDGenerator = null;
        internal SafeIDGenerator SafeEventIDGenerator = null;

        internal string City = null;
        internal string Location = null;
        internal string IPAddress = null;
        internal string CountryCode = null;
        internal bool IsLocationDisabled = false;
        internal bool IsAutomaticSessionTrackingDisabled = false;

        internal Consents[] GivenConsent { get; private set; }
        internal string[] EnabledConsentGroups { get; private set; }
        internal List<INotificationListener> NotificationEventListeners;
        internal Dictionary<string, Consents[]> ConsentGroups { get; private set; }
        internal Dictionary<string, string> overridenMetrics;
        internal MetricHelper metricHelper;

        /// <summary>
        /// Parent must be undestroyable
        /// </summary>
        public GameObject Parent = null;

        [Obsolete("CountlyConfiguration() is deprecated, this is going to be removed in the future. Use CountlyConfiguration(string appKey, string serverUrl) instead.")]
        public CountlyConfiguration()
        {
            ConsentGroups = new Dictionary<string, Consents[]>();
            NotificationEventListeners = new List<INotificationListener>();
        }

        public CountlyConfiguration(string appKey, string serverUrl)
        {
            AppKey = appKey;
            ServerUrl = serverUrl;

            ConsentGroups = new Dictionary<string, Consents[]>();
            NotificationEventListeners = new List<INotificationListener>();
        }

        internal CountlyConfiguration(CountlyAuthModel authModel, CountlyConfigModel config)
        {
            ConsentGroups = new Dictionary<string, Consents[]>();
            ServerUrl = authModel.ServerUrl;
            AppKey = authModel.AppKey;
            DeviceId = authModel.DeviceId;

            Salt = config.Salt;
            EnablePost = config.EnablePost;
            EnableManualSessionHandling = config.EnableManualSessionHandling;
            EnableTestMode = config.EnableTestMode;
            EnableConsoleLogging = config.EnableConsoleLogging;
            NotificationMode = config.NotificationMode;
            SessionDuration = config.SessionDuration;
            EventQueueThreshold = config.EventQueueThreshold;
            StoredRequestLimit = config.StoredRequestLimit;
            TotalBreadcrumbsAllowed = config.TotalBreadcrumbsAllowed;
            EnableAutomaticCrashReporting = config.EnableAutomaticCrashReporting;
            NotificationEventListeners = new List<INotificationListener>();
        }

        public override string ToString()
        {
            return $"{nameof(ServerUrl)}: {ServerUrl}, {nameof(AppKey)}: {AppKey}, {nameof(DeviceId)}: {DeviceId}, {nameof(EnableTestMode)}: {EnableTestMode}, {nameof(EnableConsoleLogging)}: {EnableConsoleLogging}, {nameof(EventQueueThreshold)}: {EventQueueThreshold}, {nameof(StoredRequestLimit)}: {StoredRequestLimit}";
        }

        /// <summary>
        /// Disabled the automatic session tracking.
        /// </summary>
        public void DisableAutomaticSessionTracking()
        {
            IsAutomaticSessionTrackingDisabled = true;
        }

        /// <summary>
        /// Disabled the location tracking on the Countly server
        /// </summary>
        public void DisableLocation()
        {
            IsLocationDisabled = true;
        }

        /// <summary>
        /// Give consent to features in case consent is required.
        /// </summary>
        /// <param name="consents">array of consent for which consent should be given</param>
        public void GiveConsent([NotNull] Consents[] consents)
        {
            GivenConsent = consents;
        }

        /// <summary>
        /// Group multiple consents into a consent group
        /// </summary>
        /// <param name="groupName">name of the consent group that will be created</param>
        /// <param name="consents">array of consent to be added to the consent group</param>
        /// <returns></returns>
        public void CreateConsentGroup([NotNull] string groupName, [NotNull] Consents[] consents)
        {
            ConsentGroups[groupName] = consents;
        }

        /// <summary>
        /// Give consent to the provided consent groups
        /// </summary>
        /// <param name="groupName">array of consent group for which consent should be given</param>
        /// <returns></returns>
        public void GiveConsentToGroup([NotNull] string[] groupName)
        {
            EnabledConsentGroups = groupName;
        }

        /// <summary>
        /// Add Notification listener.
        /// </summary>
        /// <param name="listener"></param>
        public void AddNotificationListener(INotificationListener listener)
        {
            NotificationEventListeners.Add(listener);
        }

        #region Setters
        /// <summary>
        /// Set metric overrides.
        /// </summary>
        public void SetMetricOverride(Dictionary<string, string> overridenMetrics)
        {
            this.overridenMetrics = overridenMetrics;
        }

        /// <summary>
        /// Set location parameters that will be used during init.
        /// </summary>
        /// <param name="countryCode">ISO Country code for the user's country</param>
        /// <param name="city">Name of the user's city</param>
        /// <param name="gpsCoordinates">comma separate lat and lng values.<example>"56.42345,123.45325"</example> </param>
        /// <param name="ipAddress">user's IP Address</param>
        /// <returns></returns>
        public CountlyConfiguration SetLocation(string countryCode, string city, string gpsCoordinates, string ipAddress)
        {
            City = city;
            IPAddress = ipAddress;
            CountryCode = countryCode;
            Location = gpsCoordinates;

            return this;
        }

        /// <summary>
        /// Set unique ID for the device the app is running on.
        /// </summary>
        /// <returns>Modified instance of the CountlyConfiguration</returns>
        public CountlyConfiguration SetDeviceId(string deviceId)
        {
            DeviceId = deviceId;

            return this;
        }

        /// <summary>
        /// Set to prevent parameter tampering.
        /// </summary>
        /// <returns>Modified instance of the CountlyConfiguration</returns>
        public CountlyConfiguration SetParameterTamperingProtectionSalt(string salt)
        {
            Salt = salt;

            return this;
        }

        /// <summary>
        /// Enables to send all requests made to the Countly server using HTTP POST.
        /// </summary>
        /// <returns>Modified instance of the CountlyConfiguration</returns>
        public CountlyConfiguration EnableForcedHttpPost()
        {
            EnablePost = true;

            return this;
        }

        /// <summary>
        /// Enables Countly internal debugging logs.
        /// </summary>
        /// <returns>Modified instance of the CountlyConfiguration</returns>
        public CountlyConfiguration EnableLogging()
        {
            EnableConsoleLogging = true;

            return this;
        }

        /// <summary>
        /// Set mode of push notification.
        /// </summary>
        /// <returns>Modified instance of the CountlyConfiguration</returns>
        public CountlyConfiguration SetNotificationMode(TestMode mode)
        {
            NotificationMode = mode;

            return this;
        }

        /// <summary>
        /// Sets the interval for the automatic update calls. Min value 1 (1 second)
        /// </summary>
        /// <returns>Modified instance of the CountlyConfiguration</returns>
        public CountlyConfiguration SetUpdateSessionTimerDelay(int duration)
        {
            SessionDuration = duration;

            return this;
        }

        /// <summary>
        /// Set maximum size of all string keys
        /// </summary>
        /// <returns>Modified instance of the CountlyConfiguration</returns>
        public CountlyConfiguration SetMaxKeyLength(int length)
        {
            MaxKeyLength = length;

            return this;
        }

        /// <summary>
        /// Set maximum size of all values in our key-value pairs
        /// </summary>
        /// <returns>Modified instance of the CountlyConfiguration</returns>
        public CountlyConfiguration SetMaxValueSize(int size)
        {
            MaxValueSize = size;

            return this;
        }

        /// <summary>
        /// Set max amount of custom (dev provided) segmentation in one event
        /// </summary>
        /// <returns>Modified instance of the CountlyConfiguration</returns>
        public CountlyConfiguration SetMaxSegmentationValues(int values)
        {
            MaxSegmentationValues = values;

            return this;
        }

        /// <summary>
        /// Set the limit of how many stack trace lines would be recorded per thread
        /// </summary>
        /// <returns>Modified instance of the CountlyConfiguration</returns>
        public CountlyConfiguration SetMaxStackTraceLinesPerThread(int lines)
        {
            MaxStackTraceLinesPerThread = lines;

            return this;
        }

        /// <summary>
        /// Set the limit of how many characters are allowed per stack trace line
        /// </summary>
        /// <returns>Modified instance of the CountlyConfiguration</returns>
        public CountlyConfiguration SetMaxStackTraceLineLength(int length)
        {
            MaxStackTraceLineLength = length;

            return this;
        }

        /// <summary>
        /// Set threshold value for the number of events that can be stored locally.
        /// </summary>
        /// <returns>Modified instance of the CountlyConfiguration</returns>
        public CountlyConfiguration SetEventQueueSizeToSend(int threshold)
        {
            EventQueueThreshold = threshold;

            return this;
        }

        /// <summary>
        /// Set limit for the number of requests that can be stored locally.
        /// </summary>
        /// <returns>Modified instance of the CountlyConfiguration</returns>
        public CountlyConfiguration SetMaxRequestQueueSize(int limit)
        {
            StoredRequestLimit = limit;

            return this;
        }

        /// <summary>
        /// Set the maximum amount of breadcrumbs.
        /// </summary>
        /// <returns>Modified instance of the CountlyConfiguration</returns>
        public CountlyConfiguration SetMaxBreadcrumbCount(int amount)
        {
            TotalBreadcrumbsAllowed = amount;

            return this;
        }

        /// <summary>
        /// Disables uncaught crash reporting.
        /// </summary>
        /// <returns>Modified instance of the CountlyConfiguration</returns>
        public CountlyConfiguration DisableAutomaticCrashReporting()
        {
            EnableAutomaticCrashReporting = false;

            return this;
        }

        /// <summary>
        /// Enables requirement of the consent.
        /// </summary>
        /// <returns>Modified instance of the CountlyConfiguration</returns>
        public CountlyConfiguration SetRequiresConsent(bool enable)
        {
            RequiresConsent = enable;

            return this;
        }
        #endregion
    }
}
