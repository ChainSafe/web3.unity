using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Notifications;
using Notifications.Impls;
using Plugins.CountlySDK.Helpers;
using Plugins.CountlySDK.Models;
using Plugins.CountlySDK.Persistance.Entities;
using Plugins.CountlySDK.Persistance.Repositories;
using Plugins.CountlySDK.Persistance.Repositories.Impls;
using Plugins.CountlySDK.Services;
using Plugins.iBoxDB;
using UnityEngine;
using Plugins.CountlySDK.Enums;
#pragma warning disable CS0618

[assembly: InternalsVisibleTo("PlayModeTests")]
namespace Plugins.CountlySDK
{
    public class Countly : MonoBehaviour
    {
        /// <summary>
        /// Return Countly shared instance.
        /// </summary>
        /// <returns>Countly</returns>
        public static Countly Instance
        {
            get {
                if (_instance == null) {
                    GameObject gameObject = new GameObject("_countly");
                    _instance = gameObject.AddComponent<Countly>();
                }
                return _instance;
            }
            internal set {
                _instance = value;
            }
        }

        /// <summary>
        /// Check if SDK has been initialized.
        /// </summary>
        /// <returns>bool</returns>
        public bool IsSDKInitialized { get; private set; }

        /// <summary>
        /// Check/Update consent for a particular feature.
        /// </summary>
        ///<returns>ConsentCountlyService</returns>
        public ConsentCountlyService Consents { get; private set; }

        /// <summary>
        /// Exposes functionality to record crashes/errors and record breadcrumbs.
        /// </summary>
        /// <returns>CrashReportsCountlyService</returns>
        public CrashReportsCountlyService CrashReports { get; private set; }

        /// <summary>
        /// Exposes functionality to get the current device ID and change id.
        /// </summary>
        /// <returns>DeviceIdCountlyService</returns>
        public DeviceIdCountlyService Device { get; private set; }

        /// <summary>
        /// Exposes functionality to record custom events.
        /// </summary>
        /// <returns>EventCountlyService</returns>
        public EventCountlyService Events { get; private set; }

        /// <summary>
        /// Exposes functionality to set location parameters.
        /// </summary>
        /// <returns>LocationService</returns>
        public Services.LocationService Location { get; private set; }

        /// <summary>
        /// Exposes functionality to update the remote config values. It also provides a way to access the currently downloaded ones.
        /// </summary>
        /// <returns>RemoteConfigCountlyService</returns>
        public RemoteConfigCountlyService RemoteConfigs { get; private set; }

        /// <summary>
        /// Exposes functionality to report start rating.
        /// </summary>
        /// <returns>StarRatingCountlyService</returns>
        public StarRatingCountlyService StarRating { get; private set; }

        /// <summary>
        /// Exposes functionality to set and change custom user properties and interact with custom property modifiers.
        /// </summary>
        /// <returns>UserDetailsCountlyService</returns>
        [Obsolete("UserDetailsCountlyService is deprecated and will be removed in the future. Please use UserProfile instead.")]
        public UserDetailsCountlyService UserDetails { get; private set; }

        /// <summary>
        /// Exposes functionality for managing view lifecycle with segmentation options. Includes global view segmentation and adding segmentation to ongoing views.
        /// </summary>
        public IViewModule Views { get; private set; }

        /// <summary>
        /// Exposes functionality to set and change custom user properties and interact with custom property modifiers.
        /// </summary>
        public IUserProfileModule UserProfile { get; private set; }

        /// <summary>
        /// Add callbacks to listen to push notification events for when a notification is received and when it is clicked.
        /// </summary>
        /// <returns>NotificationsCallbackService</returns>
        public NotificationsCallbackService Notifications { get; set; }
        public CountlyAuthModel Auth;
        public CountlyConfigModel Config;
        internal SessionCountlyService Session { get; set; }
        internal InitializationCountlyService Initialization { get; private set; }
        internal RequestCountlyHelper RequestHelper;
        internal CountlyConfiguration Configuration;
        internal StorageAndMigrationHelper StorageHelper;
        internal readonly object lockObj = new object();
        private bool _logSubscribed;
        private List<AbstractBaseService> _listeners = new List<AbstractBaseService>();
        private CountlyLogHelper _logHelper;
        private static Countly _instance = null;
        private PushCountlyService _push;
        private CountlyMainThreadHandler countlyMainThreadHandler;

        #region SDK limit defaults
        internal const int MaxKeyLengthDefault = 128;
        internal const int MaxValueSizeDefault = 256;
        internal const int MaxSegmentationValuesDefault = 100;
        internal const int MaxBreadcrumbCountDefault = 100;
        internal const int MaxStackTraceLinesPerThreadDefault = 30;
        internal const int MaxStackTraceLineLengthDefault = 200;
        internal const int MaxStackTraceThreadCountDefault = 30;
        #endregion

        /// <summary>
        /// Initialize SDK at the start of your app
        /// </summary>
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            countlyMainThreadHandler = CountlyMainThreadHandler.Instance;
            //Auth and Config will not be null in case initializing through countly prefab
            if (Auth != null && Config != null) {
                Init(new CountlyConfiguration(Auth, Config));
            }
        }

        public void Init(CountlyConfiguration configuration)
        {
            // Check if the current thread is the main thread
            if (countlyMainThreadHandler.IsMainThread()) {
                // If on the main thread, initialize directly
                InitInternal(configuration);
            } else {
                // If not on the main thread, schedule initialization on the main thread
                // This ensures that SDK is initialized on the main thread
                countlyMainThreadHandler.RunOnMainThread(() => { InitInternal(configuration); });

                // Avoid potential issues with SDK initialization on non-main threads
                Debug.LogWarning("[Countly][Init] Initialization process is being moved to the main thread. Ensure this is intended behavior.");
            }
        }

        public void InitInternal(CountlyConfiguration configuration)
        {
            if (IsSDKInitialized) {
                _logHelper.Error("[Countly][InitInternal] SDK has already been initialized, 'Init' should not be called a second time!");
                return;
            }

            Configuration = configuration;
            _logHelper = new CountlyLogHelper(Configuration);
            _logHelper.Info("[Countly][InitInternal] Initializing Countly [SdkName: " + Constants.SdkName + " SdkVersion: " + Constants.SdkVersion + "]");
            configuration.metricHelper = new MetricHelper(configuration.overridenMetrics);

            if (configuration.Parent != null) {
                transform.parent = configuration.Parent.transform;
            }

            if (string.IsNullOrEmpty(configuration.GetServerUrl())) {
                throw new ArgumentNullException(configuration.GetServerUrl(), "Server URL is required.");
            }

            if (string.IsNullOrEmpty(configuration.GetAppKey())) {
                throw new ArgumentNullException(configuration.GetAppKey(), "App Key is required.");
            }

            if (configuration.GetServerUrl()[configuration.GetServerUrl().Length - 1] == '/') {
                configuration.ServerUrl = configuration.GetServerUrl().Remove(configuration.GetServerUrl().Length - 1);
            }

            _logHelper.Debug("[Countly][InitInternal] SDK initialized with the URL:[" + configuration.GetServerUrl() + "] and the appKey:[" + configuration.GetAppKey() + "]");

            if (configuration.GetUpdateSessionTimerDelay() < 1) {
                _logHelper.Error("[Countly][InitInternal] provided session duration is less than 1. Replacing it with 1.");
                configuration.SetUpdateSessionTimerDelay(1);
            }
            _logHelper.Debug("[Countly][InitInternal] session duration set to [" + configuration.GetUpdateSessionTimerDelay() + "]");

            if (configuration.IsForcedHttpPostEnabled()) {
                _logHelper.Debug("[Countly][InitInternal] Setting HTTP POST to be forced");
            }

            if (configuration.GetParameterTamperingProtectionSalt() != null) {
                _logHelper.Debug("[Countly][InitInternal] Enabling tamper protection");
            }

            if (configuration.GetNotificationMode() != TestMode.None) {
                _logHelper.Debug("[Countly][InitInternal] Enabling push notification");
            }

            if (configuration.EnableTestMode) {
                _logHelper.Warning("[Countly][InitInternal] Enabling test mode");
            }

            if (configuration.IsAutomaticCrashReportingEnabled()) {
                _logHelper.Debug("[Countly][InitInternal] Enabling automatic crash reporting");
            }

            // Have a look at the SDK limit values
            if (configuration.GetEventQueueSizeToSend() < 1) {
                _logHelper.Error("[Countly][InitInternal] provided event queue size is less than 1. Replacing it with 1.");
                configuration.SetEventQueueSizeToSend(1);
            }
            _logHelper.Debug("[Countly][InitInternal] event queue size set to [" + configuration.GetEventQueueSizeToSend() + "]");

            if (configuration.GetMaxRequestQueueSize() < 1) {
                _logHelper.Error("[Countly][InitInternal] provided request queue size is less than 1. Replacing it with 1.");
                configuration.SetMaxRequestQueueSize(1);
            }
            _logHelper.Debug("[Countly][InitInternal] request queue size set to [" + configuration.GetMaxRequestQueueSize() + "]");

            if (configuration.GetMaxKeyLength() != MaxKeyLengthDefault) {
                if (configuration.GetMaxKeyLength() < 1) {
                    configuration.SetMaxKeyLength(1);
                    _logHelper.Warning("[Countly][InitInternal] provided 'maxKeyLength' is less than '1'. Setting it to '1'.");
                }
                _logHelper.Info("[Countly][InitInternal] provided 'maxKeyLength' override:[" + configuration.GetMaxKeyLength() + "]");
            }

            if (configuration.GetMaxValueSize() != MaxValueSizeDefault) {
                if (configuration.GetMaxValueSize() < 1) {
                    configuration.SetMaxValueSize(1);
                    _logHelper.Warning("[Countly][InitInternal] provided 'maxValueSize' is less than '1'. Setting it to '1'.");
                }
                _logHelper.Info("[Countly][InitInternal] provided 'maxValueSize' override:[" + configuration.GetMaxValueSize() + "]");
            }

            if (configuration.GetMaxSegmentationValues() != MaxSegmentationValuesDefault) {
                if (configuration.GetMaxSegmentationValues() < 1) {
                    configuration.SetMaxSegmentationValues(1);
                    _logHelper.Warning("[Countly][InitInternal] provided 'maxSegmentationValues' is less than '1'. Setting it to '1'.");
                }
                _logHelper.Info("[Countly][InitInternal] provided 'maxSegmentationValues' override:[" + configuration.GetMaxSegmentationValues() + "]");
            }

            if (configuration.GetMaxBreadcrumbCount() != MaxBreadcrumbCountDefault) {
                if (configuration.GetMaxBreadcrumbCount() < 1) {
                    configuration.SetMaxBreadcrumbCount(1);
                    _logHelper.Warning("[Countly][InitInternal] provided 'maxBreadcrumbCount' is less than '1'. Setting it to '1'.");
                }
                _logHelper.Info("[Countly][InitInternal] provided 'maxBreadcrumbCount' override:[" + configuration.GetMaxBreadcrumbCount() + "]");
            }

            if (configuration.GetMaxStackTraceLinesPerThread() != MaxStackTraceLinesPerThreadDefault) {
                if (configuration.GetMaxStackTraceLinesPerThread() < 1) {
                    configuration.SetMaxStackTraceLinesPerThread(1);
                    _logHelper.Warning("[Countly][InitInternal] provided 'maxStackTraceLinesPerThread' is less than '1'. Setting it to '1'.");
                }
                _logHelper.Info("[Countly][InitInternal] provided 'maxStackTraceLinesPerThread' override:[" + configuration.GetMaxStackTraceLinesPerThread() + "]");
            }

            if (configuration.GetMaxStackTraceLineLength() != MaxStackTraceLineLengthDefault) {
                if (configuration.GetMaxStackTraceLineLength() < 1) {
                    configuration.SetMaxStackTraceLineLength(1);
                    _logHelper.Warning("[Countly][InitInternal] provided 'maxStackTraceLineLength' is less than '1'. Setting it to '1'.");
                }
                _logHelper.Info("[Countly][InitInternal] provided 'maxStackTraceLineLength' override:[" + configuration.GetMaxStackTraceLineLength() + "]");
            }

            if (configuration.SafeEventIDGenerator == null) {
                configuration.SafeEventIDGenerator = new SafeIDGenerator();
            }

            if (configuration.SafeViewIDGenerator == null) {
                configuration.SafeViewIDGenerator = new SafeIDGenerator();
            }

            FirstLaunchAppHelper.Process();
            RequestBuilder requestBuilder = new RequestBuilder();
            StorageHelper = new StorageAndMigrationHelper(_logHelper, requestBuilder);
            StorageHelper.OpenDB();

            IDictionary<string, object> migrationParams = new Dictionary<string, object>()
            {
                {StorageAndMigrationHelper.key_from_2_to_3_custom_id_set, configuration.GetDeviceId() != null },
            };

            StorageHelper.RunMigration(migrationParams);
            Init(requestBuilder, StorageHelper.RequestRepo, StorageHelper.EventRepo, StorageHelper.ConfigDao);
            Device.InitDeviceId(configuration.GetDeviceId());
            OnInitialisationComplete();
            _logHelper.Debug("[Countly][InitInternal] Finished Initializing SDK.");
        }

        private void Init(RequestBuilder requestBuilder, RequestRepository requestRepo,
            NonViewEventRepository nonViewEventRepo, Dao<ConfigEntity> configDao)
        {
            CountlyUtils countlyUtils = new CountlyUtils(this);
            RequestHelper = new RequestCountlyHelper(Configuration, _logHelper, countlyUtils, requestBuilder, requestRepo, this);
            Consents = new ConsentCountlyService(Configuration, _logHelper, Consents, RequestHelper);
            Events = new EventCountlyService(Configuration, _logHelper, RequestHelper, nonViewEventRepo, Consents, countlyUtils);
            Location = new Services.LocationService(Configuration, _logHelper, RequestHelper, Consents);
            Notifications = new NotificationsCallbackService(Configuration, _logHelper);
            ProxyNotificationsService notificationsService = new ProxyNotificationsService(transform, Configuration, _logHelper, InternalStartCoroutine, Events);
            _push = new PushCountlyService(Configuration, _logHelper, RequestHelper, notificationsService, Notifications, Consents);
            Session = new SessionCountlyService(Configuration, _logHelper, Events, RequestHelper, Location, Consents, this);
            CrashReports = new CrashReportsCountlyService(Configuration, _logHelper, RequestHelper, Consents);
            Initialization = new InitializationCountlyService(Configuration, _logHelper, Location, Session, Consents);
            RemoteConfigs = new RemoteConfigCountlyService(Configuration, _logHelper, RequestHelper, countlyUtils, configDao, Consents, requestBuilder);
            StarRating = new StarRatingCountlyService(Configuration, _logHelper, Consents, Events);
            UserDetails = new UserDetailsCountlyService(Configuration, _logHelper, RequestHelper, countlyUtils, Consents);
            UserProfile = new UserProfile(this, Configuration, _logHelper, RequestHelper, countlyUtils, Consents, Events);
            Views = new ViewCountlyService(this, countlyUtils, Configuration, _logHelper, Events, Consents);
            Device = new DeviceIdCountlyService(Configuration, _logHelper, Session, RequestHelper, Events, countlyUtils, Consents);

            CreateListOfIBaseService();
            RegisterListenersToServices();
        }

        private void OnInitialisationComplete()
        {
            lock (lockObj) {
                IsSDKInitialized = true;
                _ = Initialization.OnInitialisationComplete();
                foreach (AbstractBaseService listener in _listeners) {
                    listener.OnInitializationCompleted();
                }
            }
        }

        private void CreateListOfIBaseService()
        {
            _listeners.Clear();

            _listeners.Add(_push);
            _listeners.Add((ViewCountlyService)Views);
            _listeners.Add(Events);
            _listeners.Add(Device);
            _listeners.Add(Session);
            _listeners.Add(Location);
            _listeners.Add(Consents);
            _listeners.Add(StarRating);
            _listeners.Add(UserDetails);
            _listeners.Add((UserProfile)UserProfile);
            _listeners.Add(CrashReports);
            _listeners.Add(RemoteConfigs);
            _listeners.Add(Initialization);
        }

        private void RegisterListenersToServices()
        {
            Device.Listeners = _listeners;
            Consents.Listeners = _listeners;

            foreach (AbstractBaseService listener in _listeners) {
                listener.LockObj = lockObj;
            }
        }

        /// <summary>
        /// End session on application close/quit
        /// </summary>
        private void OnApplicationQuit()
        {
            if (!IsSDKInitialized) {
                return;
            }

            _logHelper.Debug("[Countly] OnApplicationQuit");
            Session?._sessionTimer?.Dispose();
            StorageHelper?.CloseDB();
        }

        internal void CloseDBConnection()
        {
            StorageHelper?.CloseDB();
        }

        internal void ClearStorage()
        {
            if (!IsSDKInitialized) {
                return;
            }

            _logHelper.Debug("[Countly] ClearStorage");
            PlayerPrefs.DeleteAll();
            StorageHelper?.ClearDBData();
            StorageHelper?.CloseDB();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!IsSDKInitialized) {
                return;
            }

            _logHelper?.Debug("[Countly] OnApplicationFocus: " + hasFocus);

            if (hasFocus) {
                SubscribeAppLog();
            } else {
                HandleAppPauseOrFocus();
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            lock (lockObj) {
                if (!IsSDKInitialized) {
                    return;
                }

                _logHelper?.Debug("[Countly] OnApplicationPause: " + pauseStatus);

                if (CrashReports != null) {
                    CrashReports.IsApplicationInBackground = pauseStatus;
                }

                if (pauseStatus) {
                    HandleAppPauseOrFocus();
                    if (!Configuration.IsAutomaticSessionTrackingDisabled) {
                        _ = Session?.EndSessionAsync();
                    }
                } else {
                    SubscribeAppLog();
                    if (!Configuration.IsAutomaticSessionTrackingDisabled) {
                        _ = Session?.BeginSessionAsync();
                    }
                }
            }
        }

        private void HandleAppPauseOrFocus()
        {
            UnsubscribeAppLog();
        }

        // Whenever app is enabled
        private void OnEnable()
        {
            SubscribeAppLog();
        }

        // Whenever app is disabled
        private void OnDisable()
        {
            UnsubscribeAppLog();
        }

        private void LogCallback(string condition, string stackTrace, LogType type)
        {
            if (type == LogType.Exception) {
                CrashReports?.SendCrashReportAsync(condition, stackTrace);
            }
        }

        private void SubscribeAppLog()
        {
            if (_logSubscribed) {
                return;
            }

            Application.logMessageReceived += LogCallback;
            _logSubscribed = true;
        }

        private void UnsubscribeAppLog()
        {
            if (!_logSubscribed) {
                return;
            }

            Application.logMessageReceived -= LogCallback;
            _logSubscribed = false;
        }

        private void InternalStartCoroutine(IEnumerator enumerator)
        {
            StartCoroutine(enumerator);
        }
    }
}
