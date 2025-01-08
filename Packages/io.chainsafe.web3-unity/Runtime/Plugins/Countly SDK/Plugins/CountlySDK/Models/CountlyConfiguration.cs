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
        public string serverUrl = null;

        /// <summary>
        /// App key for the application being tracked.
        /// Mandatory field.
        /// </summary>
        public string appKey = null;

        /// <summary>
        /// Unique ID for the device the app is running on.
        /// </summary>
        public string deviceId = null;

        /// <summary>
        /// Set to prevent parameter tampering.
        /// </summary>
        public string salt = null;

        /// <summary>
        /// Set to send all requests made to the Countly server using HTTP POST.
        /// </summary>
        public bool enablePost = false;

        /// <summary>
        /// Set to true if you want the SDK to pretend that it's functioning.
        /// </summary>
        public bool enableTestMode = false;

        /// <summary>
        /// Set to true if you want to enable countly internal debugging logs.
        /// </summary>
        public bool enableConsoleLogging = false;

        /// <summary>
        /// Set mode of push notification.
        /// </summary>
        public TestMode notificationMode = TestMode.None;

        /// <summary>
        /// Set to true to enable manual session handling.
        /// </summary>
        public readonly bool EnableManualSessionHandling = false;

        /// <summary>
        /// Sets the interval for the automatic update calls
        /// min value 1 (1 second), max value 600 (10 minutes)
        /// </summary>
        public int sessionDuration = 60;

        /// <summary>
        /// Maximum size of all string keys
        /// </summary>
        public int maxKeyLength = 128;

        /// <summary>
        /// Maximum size of all values in our key-value pairs
        /// </summary>
        public int maxValueSize = 256;

        /// <summary>
        /// Max amount of custom (dev provided) segmentation in one event
        /// </summary>
        public int maxSegmentationValues = 100;

        /// <summary>
        /// Limits how many stack trace lines would be recorded per thread
        /// </summary>
        public int maxStackTraceLinesPerThread = 30;

        /// <summary>
        /// Limits how many characters are allowed per stack trace line
        /// </summary>
        public int maxStackTraceLineLength = 200;

        /// <summary>
        /// Set threshold value for the number of events that can be stored locally.
        /// </summary>
        public int eventQueueThreshold = 100;

        /// <summary>
        /// Set limit for the number of requests that can be stored locally.
        /// </summary>
        public int storedRequestLimit = 1000;

        /// <summary>
        /// Set the maximum amount of breadcrumbs.
        /// </summary>
        public int totalBreadcrumbsAllowed = 100;

        /// <summary>
        /// Set true to enable uncaught crash reporting.
        /// </summary>
        public bool enableAutomaticCrashReporting = true;

        /// <summary>
        /// Set if consent should be required.
        /// </summary>
        public bool requiresConsent = false;

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
        internal Dictionary<string, string> OverridenMetrics;
        internal MetricHelper MetricHelper;

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

        public CountlyConfiguration(string _appKey, string _serverUrl)
        {
            appKey = _appKey;
            serverUrl = _serverUrl;

            ConsentGroups = new Dictionary<string, Consents[]>();
            NotificationEventListeners = new List<INotificationListener>();
        }

        internal CountlyConfiguration(CountlyAuthModel authModel, CountlyConfigModel config)
        {
            ConsentGroups = new Dictionary<string, Consents[]>();
            serverUrl = authModel.ServerUrl;
            appKey = authModel.AppKey;
            deviceId = authModel.DeviceId;

            salt = config.Salt;
            enablePost = config.EnablePost;
            EnableManualSessionHandling = config.EnableManualSessionHandling;
            enableTestMode = config.EnableTestMode;
            enableConsoleLogging = config.EnableConsoleLogging;
            notificationMode = config.NotificationMode;
            sessionDuration = config.SessionDuration;
            eventQueueThreshold = config.EventQueueThreshold;
            storedRequestLimit = config.StoredRequestLimit;
            totalBreadcrumbsAllowed = config.TotalBreadcrumbsAllowed;
            enableAutomaticCrashReporting = config.EnableAutomaticCrashReporting;
            NotificationEventListeners = new List<INotificationListener>();
        }

        public override string ToString()
        {
            return $"{nameof(serverUrl)}: {serverUrl}, {nameof(appKey)}: {appKey}, {nameof(deviceId)}: {deviceId}, {nameof(enableTestMode)}: {enableTestMode}, {nameof(enableConsoleLogging)}: {enableConsoleLogging}, {nameof(eventQueueThreshold)}: {eventQueueThreshold}, {nameof(storedRequestLimit)}: {storedRequestLimit}";
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
            this.OverridenMetrics = overridenMetrics;
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
        public CountlyConfiguration SetDeviceId(string _deviceId)
        {
            deviceId = _deviceId;

            return this;
        }

        /// <summary>
        /// Set to prevent parameter tampering.
        /// </summary>
        /// <returns>Modified instance of the CountlyConfiguration</returns>
        public CountlyConfiguration SetParameterTamperingProtectionSalt(string _salt)
        {
            salt = _salt;

            return this;
        }

        /// <summary>
        /// Enables to send all requests made to the Countly server using HTTP POST.
        /// </summary>
        /// <returns>Modified instance of the CountlyConfiguration</returns>
        public CountlyConfiguration EnableForcedHttpPost()
        {
            enablePost = true;

            return this;
        }

        /// <summary>
        /// Enables Countly internal debugging logs.
        /// </summary>
        /// <returns>Modified instance of the CountlyConfiguration</returns>
        public CountlyConfiguration EnableLogging()
        {
            enableConsoleLogging = true;

            return this;
        }

        /// <summary>
        /// Set mode of push notification.
        /// </summary>
        /// <returns>Modified instance of the CountlyConfiguration</returns>
        public CountlyConfiguration SetNotificationMode(TestMode mode)
        {
            notificationMode = mode;

            return this;
        }

        /// <summary>
        /// Sets the interval for the automatic update calls. Min value 1 (1 second)
        /// </summary>
        /// <returns>Modified instance of the CountlyConfiguration</returns>
        public CountlyConfiguration SetUpdateSessionTimerDelay(int duration)
        {
            sessionDuration = duration;

            return this;
        }

        /// <summary>
        /// Set maximum size of all string keys
        /// </summary>
        /// <returns>Modified instance of the CountlyConfiguration</returns>
        public CountlyConfiguration SetMaxKeyLength(int length)
        {
            maxKeyLength = length;

            return this;
        }

        /// <summary>
        /// Set maximum size of all values in our key-value pairs
        /// </summary>
        /// <returns>Modified instance of the CountlyConfiguration</returns>
        public CountlyConfiguration SetMaxValueSize(int size)
        {
            maxValueSize = size;

            return this;
        }

        /// <summary>
        /// Set max amount of custom (dev provided) segmentation in one event
        /// </summary>
        /// <returns>Modified instance of the CountlyConfiguration</returns>
        public CountlyConfiguration SetMaxSegmentationValues(int values)
        {
            maxSegmentationValues = values;

            return this;
        }

        /// <summary>
        /// Set the limit of how many stack trace lines would be recorded per thread
        /// </summary>
        /// <returns>Modified instance of the CountlyConfiguration</returns>
        public CountlyConfiguration SetMaxStackTraceLinesPerThread(int lines)
        {
            maxStackTraceLinesPerThread = lines;

            return this;
        }

        /// <summary>
        /// Set the limit of how many characters are allowed per stack trace line
        /// </summary>
        /// <returns>Modified instance of the CountlyConfiguration</returns>
        public CountlyConfiguration SetMaxStackTraceLineLength(int length)
        {
            maxStackTraceLineLength = length;

            return this;
        }

        /// <summary>
        /// Set threshold value for the number of events that can be stored locally.
        /// </summary>
        /// <returns>Modified instance of the CountlyConfiguration</returns>
        public CountlyConfiguration SetEventQueueSizeToSend(int threshold)
        {
            eventQueueThreshold = threshold;

            return this;
        }

        /// <summary>
        /// Set limit for the number of requests that can be stored locally.
        /// </summary>
        /// <returns>Modified instance of the CountlyConfiguration</returns>
        public CountlyConfiguration SetMaxRequestQueueSize(int limit)
        {
            storedRequestLimit = limit;

            return this;
        }

        /// <summary>
        /// Set the maximum amount of breadcrumbs.
        /// </summary>
        /// <returns>Modified instance of the CountlyConfiguration</returns>
        public CountlyConfiguration SetMaxBreadcrumbCount(int amount)
        {
            totalBreadcrumbsAllowed = amount;

            return this;
        }

        /// <summary>
        /// Disables uncaught crash reporting.
        /// </summary>
        /// <returns>Modified instance of the CountlyConfiguration</returns>
        public CountlyConfiguration DisableAutomaticCrashReporting()
        {
            enableAutomaticCrashReporting = false;

            return this;
        }

        /// <summary>
        /// Enables requirement of the consent.
        /// </summary>
        /// <returns>Modified instance of the CountlyConfiguration</returns>
        public CountlyConfiguration SetRequiresConsent(bool enable)
        {
            requiresConsent = enable;

            return this;
        }
        #endregion
    }
}
