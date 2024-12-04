using System;
using System.Collections.Generic;
using Plugins.CountlySDK.Enums;
using Plugins.CountlySDK.Helpers;

namespace Plugins.CountlySDK.Models
{
    [Serializable]
    public class CountlyConfigModel
    {
        public string Salt;
        public bool EnablePost;
        public bool EnableTestMode;
        public bool EnableConsoleLogging;
        public TestMode NotificationMode;
        public readonly bool EnableManualSessionHandling;
        public int SessionDuration;
        public int EventQueueThreshold;
        public int StoredRequestLimit;
        public int TotalBreadcrumbsAllowed;
        public bool EnableAutomaticCrashReporting;

        /// <summary>
        /// Initializes the SDK configurations
        /// </summary>
        /// <param name="salt"></param>
        /// <param name="enablePost"></param>
        /// <param name="enableConsoleErrorLogging"></param>
        /// <param name="ignoreSessionCooldown"></param>
        /// <param name="enableManualSessionHandling"></param>
        /// <param name="sessionDuration">Session is updated after each interval passed.
        /// This interval is also used to process request queue. The interval must be in seconds</param>
        /// <param name="notificationMode"></param>
        public CountlyConfigModel(string salt = null, bool enablePost = false, bool enableTestMode = false, bool enableConsoleErrorLogging = false,
                                    bool ignoreSessionCooldown = false, bool enableManualSessionHandling = false,
                                    int sessionDuration = 60, int eventQueueThreshold = 100,
                                    int storedRequestLimit = 1000, int totalBreadcrumbsAllowed = 100,
                                    TestMode notificationMode = TestMode.None, bool enableAutomaticCrashReporting = true)

        {
            this.Salt = salt;
            EnablePost = enablePost;
            EnableTestMode = enableTestMode;
            EnableConsoleLogging = enableConsoleErrorLogging;
            NotificationMode = notificationMode;
            SessionDuration = sessionDuration;
            //EnableManualSessionHandling = enableManualSessionHandling;
            EnableManualSessionHandling = false;
            EventQueueThreshold = eventQueueThreshold;
            StoredRequestLimit = storedRequestLimit;
            TotalBreadcrumbsAllowed = totalBreadcrumbsAllowed;
            EnableAutomaticCrashReporting = enableAutomaticCrashReporting;
        }

        public override string ToString()
        {
            return $"{nameof(Salt)}: {Salt}, {nameof(EnablePost)}: {EnablePost}, {nameof(EnableConsoleLogging)}: {EnableConsoleLogging}, {nameof(NotificationMode)}: {NotificationMode}, {nameof(EnableManualSessionHandling)}: {EnableManualSessionHandling}, {nameof(SessionDuration)}: {SessionDuration}, {nameof(EventQueueThreshold)}: {EventQueueThreshold}, {nameof(StoredRequestLimit)}: {StoredRequestLimit}, {nameof(TotalBreadcrumbsAllowed)}: {TotalBreadcrumbsAllowed}, {nameof(EnableAutomaticCrashReporting)}: {EnableAutomaticCrashReporting}";
        }
    }
}
