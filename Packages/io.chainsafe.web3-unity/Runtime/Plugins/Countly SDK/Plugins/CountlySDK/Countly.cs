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

[assembly: InternalsVisibleTo("PlayModeTests")]
namespace Plugins.CountlySDK
{
    public class Countly : MonoBehaviour
    {
        //SDK limit defaults
        internal const int MaxKeyLengthDefault = 128;
        internal const int MaxValueSizeDefault = 256;
        internal const int MaxSegmentationValuesDefault = 100;
        internal const int MaxBreadcrumbCountDefault = 100;
        internal const int MaxStackTraceLinesPerThreadDefault = 30;
        internal const int MaxStackTraceLineLengthDefault = 200;
        internal const int MaxStackTraceThreadCountDefault = 30;

        public CountlyAuthModel Auth;
        public CountlyConfigModel Config;
        internal RequestCountlyHelper RequestHelper;
        internal CountlyConfiguration Configuration;

        /// <summary>
        /// Check if SDK has been initialized.
        /// </summary>
        /// <returns>bool</returns>
        public bool IsSDKInitialized { get; private set; }

        private CountlyLogHelper _logHelper;
        private static Countly _instance = null;
        internal StorageAndMigrationHelper StorageHelper;
        internal readonly object lockObj = new object();
        private List<AbstractBaseService> _listeners = new List<AbstractBaseService>();

        /// <summary>
        /// Return countly shared instance.
        /// </summary>
        /// <returns>Countly</returns>
        public static Countly Instance
        {
            get
            {
                if (_instance == null)
                {

                    GameObject gameObject = new GameObject("_countly");
                    _instance = gameObject.AddComponent<Countly>();
                }

                return _instance;

            }
            internal set
            {
                _instance = value;
            }
        }

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

        internal InitializationCountlyService Initialization { get; private set; }

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
        public UserDetailsCountlyService UserDetails { get; private set; }

        /// <summary>
        /// Exposes functionality to start and stop recording views and report positions for heat-map.
        /// </summary>
        /// <returns>ViewCountlyService</returns>
        public ViewCountlyService Views { get; private set; }

        public MetricHelper MetricHelper { get; private set; }
        internal SessionCountlyService Session { get; set; }

        /// <summary>
        /// Add callbacks to listen to push notification events for when a notification is received and when it is clicked.
        /// </summary>
        /// <returns>NotificationsCallbackService</returns>
        public NotificationsCallbackService Notifications { get; set; }

        private bool _logSubscribed;
        private PushCountlyService _push;


        /// <summary>
        /// Initialize SDK at the start of your app
        /// </summary>
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;

            //Auth and Config will not be null in case initializing through countly prefab
            if (Auth != null && Config != null)
            {
                Init(new CountlyConfiguration(Auth, Config));
            }

        }

        public void Init(CountlyConfiguration configuration)
        {
            if (IsSDKInitialized)
            {
                _logHelper.Error("SDK has already been initialized, 'Init' should not be called a second time!");
                return;
            }

            Configuration = configuration;
            _logHelper = new CountlyLogHelper(Configuration);

            _logHelper.Info("[Init] Initializing Countly [SdkName: " + Constants.SdkName + " SdkVersion: " + Constants.SdkVersion + "]");

            configuration.metricHelper = new MetricHelper(configuration.overridenMetrics);

            if (configuration.Parent != null)
            {
                transform.parent = configuration.Parent.transform;
            }

            if (string.IsNullOrEmpty(configuration.ServerUrl))
            {
                throw new ArgumentNullException(configuration.ServerUrl, "Server URL is required.");
            }

            if (string.IsNullOrEmpty(configuration.AppKey))
            {
                throw new ArgumentNullException(configuration.AppKey, "App Key is required.");
            }

            if (configuration.ServerUrl[configuration.ServerUrl.Length - 1] == '/')
            {
                configuration.ServerUrl = configuration.ServerUrl.Remove(configuration.ServerUrl.Length - 1);
            }

            _logHelper.Debug("[Init] SDK initialized with the URL:[" + configuration.ServerUrl + "] and the appKey:[" + configuration.AppKey + "]");


            if (configuration.SessionDuration < 1)
            {
                _logHelper.Error("[Init] provided session duration is less than 1. Replacing it with 1.");
                configuration.SessionDuration = 1;
            }
            _logHelper.Debug("[Init] session duration set to [" + configuration.SessionDuration + "]");

            if (configuration.EnablePost)
            {
                _logHelper.Debug("[Init] Setting HTTP POST to be forced");
            }

            if (configuration.Salt != null)
            {
                _logHelper.Debug("[Init] Enabling tamper protection");
            }

            if (configuration.NotificationMode != TestMode.None)
            {
                _logHelper.Debug("[Init] Enabling push notification");
            }

            if (configuration.EnableTestMode)
            {
                _logHelper.Warning("[Init] Enabling test mode");
            }

            if (configuration.EnableAutomaticCrashReporting)
            {
                _logHelper.Debug("[Init] Enabling automatic crash reporting");
            }

            // Have a look at the SDK limit values
            if (configuration.EventQueueThreshold < 1)
            {
                _logHelper.Error("[Init] provided event queue size is less than 1. Replacing it with 1.");
                configuration.EventQueueThreshold = 1;
            }
            _logHelper.Debug("[Init] event queue size set to [" + configuration.EventQueueThreshold + "]");

            if (configuration.StoredRequestLimit < 1)
            {
                _logHelper.Error("[Init] provided request queue size is less than 1. Replacing it with 1.");
                configuration.StoredRequestLimit = 1;
            }
            _logHelper.Debug("[Init] request queue size set to [" + configuration.StoredRequestLimit + "]");

            if (configuration.MaxKeyLength != MaxKeyLengthDefault)
            {
                if (configuration.MaxKeyLength < 1)
                {
                    configuration.MaxKeyLength = 1;
                    _logHelper.Warning("[Init] provided 'maxKeyLength' is less than '1'. Setting it to '1'.");
                }
                _logHelper.Info("[Init] provided 'maxKeyLength' override:[" + configuration.MaxKeyLength + "]");
            }

            if (configuration.MaxValueSize != MaxValueSizeDefault)
            {
                if (configuration.MaxValueSize < 1)
                {
                    configuration.MaxValueSize = 1;
                    _logHelper.Warning("[Init] provided 'maxValueSize' is less than '1'. Setting it to '1'.");
                }
                _logHelper.Info("[Init] provided 'maxValueSize' override:[" + configuration.MaxValueSize + "]");
            }

            if (configuration.MaxSegmentationValues != MaxSegmentationValuesDefault)
            {
                if (configuration.MaxSegmentationValues < 1)
                {
                    configuration.MaxSegmentationValues = 1;
                    _logHelper.Warning("[Init] provided 'maxSegmentationValues' is less than '1'. Setting it to '1'.");
                }
                _logHelper.Info("[Init] provided 'maxSegmentationValues' override:[" + configuration.MaxSegmentationValues + "]");
            }

            if (configuration.TotalBreadcrumbsAllowed != MaxBreadcrumbCountDefault)
            {
                if (configuration.TotalBreadcrumbsAllowed < 1)
                {
                    configuration.TotalBreadcrumbsAllowed = 1;
                    _logHelper.Warning("[Init] provided 'maxBreadcrumbCount' is less than '1'. Setting it to '1'.");
                }
                _logHelper.Info("[Init] provided 'maxBreadcrumbCount' override:[" + configuration.TotalBreadcrumbsAllowed + "]");
            }

            if (configuration.MaxStackTraceLinesPerThread != MaxStackTraceLinesPerThreadDefault)
            {
                if (configuration.MaxStackTraceLinesPerThread < 1)
                {
                    configuration.MaxStackTraceLinesPerThread = 1;
                    _logHelper.Warning("[Init] provided 'maxStackTraceLinesPerThread' is less than '1'. Setting it to '1'.");
                }
                _logHelper.Info("[Init] provided 'maxStackTraceLinesPerThread' override:[" + configuration.MaxStackTraceLinesPerThread + "]");
            }

            if (configuration.MaxStackTraceLineLength != MaxStackTraceLineLengthDefault)
            {
                if (configuration.MaxStackTraceLineLength < 1)
                {
                    configuration.MaxStackTraceLineLength = 1;
                    _logHelper.Warning("[Init] provided 'maxStackTraceLineLength' is less than '1'. Setting it to '1'.");
                }
                _logHelper.Info("[Init] provided 'maxStackTraceLineLength' override:[" + configuration.MaxStackTraceLineLength + "]");
            }

            if (configuration.SafeEventIDGenerator == null)
            {
                configuration.SafeEventIDGenerator = new SafeIDGenerator();
            }

            if (configuration.SafeViewIDGenerator == null)
            {
                configuration.SafeViewIDGenerator = new SafeIDGenerator();
            }

            FirstLaunchAppHelper.Process();

            RequestBuilder requestBuilder = new RequestBuilder();
            StorageHelper = new StorageAndMigrationHelper(_logHelper, requestBuilder);
            StorageHelper.OpenDB();

            IDictionary<string, object> migrationParams = new Dictionary<string, object>()
            {
                {StorageAndMigrationHelper.key_from_2_to_3_custom_id_set, configuration.DeviceId != null },
            };

            StorageHelper.RunMigration(migrationParams);

            Init(requestBuilder, StorageHelper.RequestRepo, StorageHelper.EventRepo, StorageHelper.ConfigDao);

            Device.InitDeviceId(configuration.DeviceId);
            OnInitialisationComplete();

            _logHelper.Debug("[Countly] Finished Initializing SDK.");
        }

        private void Init(RequestBuilder requestBuilder, RequestRepository requestRepo,
            NonViewEventRepository nonViewEventRepo, Dao<ConfigEntity> configDao)
        {
            CountlyUtils countlyUtils = new CountlyUtils(this);
            RequestHelper = new RequestCountlyHelper(Configuration, _logHelper, countlyUtils, requestBuilder, requestRepo, this);

            Consents = new ConsentCountlyService(Configuration, _logHelper, Consents, RequestHelper);
            Events = new EventCountlyService(Configuration, _logHelper, RequestHelper, nonViewEventRepo, Consents);

            Location = new Services.LocationService(Configuration, _logHelper, RequestHelper, Consents);
            Notifications = new NotificationsCallbackService(Configuration, _logHelper);
            ProxyNotificationsService notificationsService = new ProxyNotificationsService(transform, Configuration, _logHelper, InternalStartCoroutine, Events);
            _push = new PushCountlyService(Configuration, _logHelper, RequestHelper, notificationsService, Notifications, Consents);
            Session = new SessionCountlyService(Configuration, _logHelper, Events, RequestHelper, Location, Consents);

            CrashReports = new CrashReportsCountlyService(Configuration, _logHelper, RequestHelper, Consents);
            Initialization = new InitializationCountlyService(Configuration, _logHelper, Location, Session, Consents);
            RemoteConfigs = new RemoteConfigCountlyService(Configuration, _logHelper, RequestHelper, countlyUtils, configDao, Consents, requestBuilder);

            StarRating = new StarRatingCountlyService(Configuration, _logHelper, Consents, Events);
            UserDetails = new UserDetailsCountlyService(Configuration, _logHelper, RequestHelper, countlyUtils, Consents);
            Views = new ViewCountlyService(Configuration, _logHelper, Events, Consents);
            Device = new DeviceIdCountlyService(Configuration, _logHelper, Session, RequestHelper, Events, countlyUtils, Consents);

            CreateListOfIBaseService();
            RegisterListenersToServices();
        }

        private async void OnInitialisationComplete()
        {
            lock (lockObj)
            {
                IsSDKInitialized = true;
                _ = Initialization.OnInitialisationComplete();
                foreach (AbstractBaseService listener in _listeners)
                {
                    listener.OnInitializationCompleted();
                }
            }

        }

        private void CreateListOfIBaseService()
        {
            _listeners.Clear();

            _listeners.Add(_push);
            _listeners.Add(Views);
            _listeners.Add(Events);
            _listeners.Add(Device);
            _listeners.Add(Session);
            _listeners.Add(Location);
            _listeners.Add(Consents);
            _listeners.Add(StarRating);
            _listeners.Add(UserDetails);
            _listeners.Add(CrashReports);
            _listeners.Add(RemoteConfigs);
            _listeners.Add(Initialization);
        }

        private void RegisterListenersToServices()
        {
            Device.Listeners = _listeners;
            Consents.Listeners = _listeners;

            foreach (AbstractBaseService listener in _listeners)
            {
                listener.LockObj = lockObj;
            }
        }

        /// <summary>
        /// End session on application close/quit
        /// </summary>
        private void OnApplicationQuit()
        {
            if (!IsSDKInitialized)
            {
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
            if (!IsSDKInitialized)
            {
                return;
            }

            _logHelper.Debug("[Countly] ClearStorage");

            PlayerPrefs.DeleteAll();
            StorageHelper?.ClearDBData();

            StorageHelper?.CloseDB();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!IsSDKInitialized)
            {
                return;
            }

            _logHelper?.Debug("[Countly] OnApplicationFocus: " + hasFocus);

            if (hasFocus)
            {
                SubscribeAppLog();
            }
            else
            {
                HandleAppPauseOrFocus();
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            lock (lockObj)
            {
                if (!IsSDKInitialized)
                {
                    return;
                }

                _logHelper?.Debug("[Countly] OnApplicationPause: " + pauseStatus);

                if (CrashReports != null)
                {
                    CrashReports.IsApplicationInBackground = pauseStatus;
                }

                if (pauseStatus)
                {
                    HandleAppPauseOrFocus();
                    if (!Configuration.IsAutomaticSessionTrackingDisabled)
                    {
                        _ = Session?.EndSessionAsync();
                    }
                }
                else
                {
                    SubscribeAppLog();
                    if (!Configuration.IsAutomaticSessionTrackingDisabled)
                    {
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
            if (type == LogType.Exception)
            {
                CrashReports?.SendCrashReportAsync(condition, stackTrace);
            }

        }

        private void SubscribeAppLog()
        {
            if (_logSubscribed)
            {
                return;
            }

            Application.logMessageReceived += LogCallback;
            _logSubscribed = true;
        }

        private void UnsubscribeAppLog()
        {
            if (!_logSubscribed)
            {
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
