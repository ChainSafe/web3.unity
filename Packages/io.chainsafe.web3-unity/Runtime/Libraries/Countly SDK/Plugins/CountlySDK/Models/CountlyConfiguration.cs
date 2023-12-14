using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Notifications;
using Plugins.CountlySDK.Enums;
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
        public string ServerUrl = null;

        /// <summary>
        /// App key for the application being tracked.
        /// Mandatory field.
        /// </summary>
        public string AppKey = null;

        /// <summary>
        /// Unique ID for the device the app is running on.
        /// </summary>
        public string DeviceId = null;

        /// <summary>
        /// Set to prevent parameter tampering.
        /// </summary>
        public string Salt = null;

        /// <summary>
        /// Set to send all requests made to the Countly server using HTTP POST.
        /// </summary>
        public bool EnablePost = false;

        /// <summary>
        /// Set to true if you want the SDK to pretend that it's functioning.
        /// </summary>
        public bool EnableTestMode = false;

        /// <summary>
        /// Set to true if you want to enable countly internal debugging logs.
        /// </summary>
        public bool EnableConsoleLogging = false;

        /// <summary>
        /// Set mode of push notification.
        /// </summary>
        public TestMode NotificationMode = TestMode.None;

        /// <summary>
        /// Set to true to enable manual session handling.
        /// </summary>
        public readonly bool EnableManualSessionHandling = false;

        /// <summary>
        /// Sets the interval for the automatic update calls
        /// min value 1 (1 second), max value 600 (10 minutes)
        /// </summary>
        public int SessionDuration = 60;

        /// <summary>
        /// Maximum size of all string keys
        /// </summary>
        public int MaxKeyLength = 128;

        /// <summary>
        /// Maximum size of all values in our key-value pairs
        /// </summary>
        public int MaxValueSize = 256;

        /// <summary>
        /// Max amount of custom (dev provided) segmentation in one event
        /// </summary>
        public int MaxSegmentationValues = 100;

        /// <summary>
        /// Limits how many stack trace lines would be recorded per thread
        /// </summary>
        public int MaxStackTraceLinesPerThread = 30;

        /// <summary>
        /// Limits how many characters are allowed per stack trace line
        /// </summary>
        public int MaxStackTraceLineLength = 200;

        /// <summary>
        /// Set threshold value for the number of events that can be stored locally.
        /// </summary>
        public int EventQueueThreshold = 100;

        /// <summary>
        /// Set limit for the number of requests that can be stored locally.
        /// </summary>
        public int StoredRequestLimit = 1000;

        /// <summary>
        /// Set the maximum amount of breadcrumbs.
        /// </summary>
        public int TotalBreadcrumbsAllowed = 100;

        /// <summary>
        /// Set true to enable uncaught crash reporting.
        /// </summary>
        public bool EnableAutomaticCrashReporting = true;

        internal string City = null;
        internal string Location = null;
        internal string IPAddress = null;
        internal string CountryCode = null;
        internal bool IsLocationDisabled = false;
        internal bool IsAutomaticSessionTrackingDisabled = false;


        /// <summary>
        /// Set if consent should be required.
        /// </summary>
        public bool RequiresConsent = false;

        internal Consents[] GivenConsent { get; private set; }
        internal string[] EnabledConsentGroups { get; private set; }
        internal List<INotificationListener> NotificationEventListeners;
        internal Dictionary<string, Consents[]> ConsentGroups { get; private set; }

        /// <summary>
        /// Parent must be undestroyable
        /// </summary>
        public GameObject Parent = null;

        public CountlyConfiguration()
        {
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
        /// Set location parameters that will be used during init.
        /// </summary>
        /// <param name="countryCode">ISO Country code for the user's country</param>
        /// <param name="city">Name of the user's city</param>
        /// <param name="gpsCoordinates">comma separate lat and lng values.<example>"56.42345,123.45325"</example> </param>
        /// <param name="ipAddress">user's IP Address</param>
        /// <returns></returns>
        public void SetLocation(string countryCode, string city, string gpsCoordinates, string ipAddress)
        {
            City = city;
            IPAddress = ipAddress;
            CountryCode = countryCode;
            Location = gpsCoordinates;
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
    }
}
