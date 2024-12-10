using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using Plugins.CountlySDK.Enums;
using Plugins.CountlySDK.Helpers;
using Plugins.CountlySDK.Models;
using UnityEngine;

namespace Plugins.CountlySDK.Services
{
    public class SessionCountlyService : AbstractBaseService
    {
        internal Timer _sessionTimer;
        internal DateTime _lastSessionRequestTime;

        /// <summary>
        /// Check if session has been initiated.
        /// </summary>
        /// <returns>bool</returns>
        internal bool IsSessionInitiated { get; private set; }
        private readonly LocationService _locationService;
        private readonly EventCountlyService _eventService;
        internal readonly RequestCountlyHelper _requestCountlyHelper;
        private readonly MonoBehaviour _monoBehaviour;
        bool isInternalTimerStopped;

        internal SessionCountlyService(CountlyConfiguration configuration, CountlyLogHelper logHelper, EventCountlyService eventService,
            RequestCountlyHelper requestCountlyHelper, LocationService locationService, ConsentCountlyService consentService, MonoBehaviour monoBehaviour) : base(configuration, logHelper, consentService)
        {
            Log.Debug("[SessionCountlyService] Initializing.");
            if (configuration.IsAutomaticSessionTrackingDisabled) {
                Log.Debug("[SessionCountlyService] Disabling automatic session tracking");
            }

            _eventService = eventService;
            _locationService = locationService;
            _requestCountlyHelper = requestCountlyHelper;
            _monoBehaviour = monoBehaviour;

            if (_configuration.IsAutomaticSessionTrackingDisabled) {
                Log.Verbose("[Countly][CountlyConfiguration] Automatic session tracking disabled!");
            }
        }

        /// <summary>
        /// Run session startup logic and start timer with the specified interval
        /// </summary>
        internal async Task StartSessionService()
        {
            if (_configuration.IsAutomaticSessionTrackingDisabled || !_consentService.CheckConsentInternal(Consents.Sessions)) {
                /* If location is disabled in init
                and no session consent is given. Send empty location as separate request.*/
                if (_locationService.IsLocationDisabled || !_consentService.CheckConsentInternal(Consents.Location)) {
                    await _locationService.SendRequestWithEmptyLocation();
                } else {
                    /*
                 * If there is no session consent or automatic session tracking is disabled, 
                 * location values set in init should be sent as a separate location request.
                 */
                    await _locationService.SendIndependantLocationRequest();
                }
            } else {
                //Start Session
                await BeginSessionAsync();
            }

            InitSessionTimer();
        }

        /// <summary>
        /// Initializes the timer for extending session with specified interval
        /// </summary>
        private void InitSessionTimer()
        {
#if UNITY_WEBGL
            _monoBehaviour.StartCoroutine(SessionTimerCoroutine());
#else
            _sessionTimer = new Timer { Interval = _configuration.GetUpdateSessionTimerDelay() * 1000 };
            _sessionTimer.Elapsed += SessionTimerOnElapsedAsync;
            _sessionTimer.AutoReset = true;
            _sessionTimer.Start();
#endif
        }

        private void SendRequestsAndExtendSession()
        {
            Countly.Instance.UserProfile.Save();
            _eventService.AddEventsToRequestQueue();
            _ = _requestCountlyHelper.ProcessQueue();

            if (!_configuration.IsAutomaticSessionTrackingDisabled) {
                _ = ExtendSessionAsync();
            }
        }

        private IEnumerator SessionTimerCoroutine()
        {
            Log.Debug("[SessionCountlyService] SessionTimerCoroutine, Start");

            if (isInternalTimerStopped) {
                yield break;
            }

            yield return new WaitForSeconds(_configuration.GetUpdateSessionTimerDelay());
            SendRequestsAndExtendSession();
            Log.Debug("[SessionCountlyService] SessionTimerCoroutine, Coroutine completed.");
        }

        /// <summary>
        /// Stops the timer and unsubscribes from the Elapsed event.
        /// This exists for preventing session extending after tests.
        /// </summary>
        internal void StopSessionTimer()
        {
            isInternalTimerStopped = true;

            #if UNITY_WEBGL
            _monoBehaviour.StopCoroutine(SessionTimerCoroutine());
            #else
            if (_sessionTimer != null) {
                // Unsubscribe from the Elapsed event
                _sessionTimer.Elapsed -= SessionTimerOnElapsedAsync;

                // Stop and dispose the timer
                _sessionTimer.Stop();
                _sessionTimer.Dispose();
            }
            #endif
        }

        /// <summary>
        /// Extends the session after the session duration is elapsed
        /// </summary>
        /// <param name="sender">reference of caller</param>
        /// <param name="elapsedEventArgs"> Provides data for <code>Timer.Elapsed</code>event.</param>
        private async void SessionTimerOnElapsedAsync(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            lock (LockObj) {

                if (isInternalTimerStopped) {
                    return;
                }

                Log.Debug("[SessionCountlyService] SessionTimerOnElapsedAsync");
                SendRequestsAndExtendSession();
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// Initiates a session
        /// </summary>
        internal async Task BeginSessionAsync()
        {
            Log.Debug("[SessionCountlyService] BeginSessionAsync");

            if (!_consentService.CheckConsentInternal(Consents.Sessions)) {
                return;
            }

            if (IsSessionInitiated) {
                Log.Warning("[SessionCountlyService] BeginSessionAsync: The session has already started!");
                return;
            }

            _lastSessionRequestTime = DateTime.Now;
            //Session initiated
            IsSessionInitiated = true;

            Dictionary<string, object> requestParams = new Dictionary<string, object>();
            requestParams.Add("begin_session", 1);

            /* If location is disabled or no location consent is given,
            the SDK adds an empty location entry to every "begin_session" request. */
            if (_locationService.IsLocationDisabled || !_consentService.CheckConsentInternal(Consents.Location)) {
                requestParams.Add("location", string.Empty);
            } else {
                if (!string.IsNullOrEmpty(_locationService.IPAddress)) {
                    requestParams.Add("ip_address", _locationService.IPAddress);
                }

                if (!string.IsNullOrEmpty(_locationService.CountryCode)) {
                    requestParams.Add("country_code", _locationService.CountryCode);
                }

                if (!string.IsNullOrEmpty(_locationService.City)) {
                    requestParams.Add("city", _locationService.City);
                }

                if (!string.IsNullOrEmpty(_locationService.Location)) {
                    requestParams.Add("location", _locationService.Location);
                }
            }

            string metricsJSON = _configuration.metricHelper.buildMetricJSON();
            requestParams.Add("metrics", metricsJSON);

            _requestCountlyHelper.AddToRequestQueue(requestParams);
            await _requestCountlyHelper.ProcessQueue();
        }

        /// <summary>
        /// Ends a session
        /// </summary>
        internal async Task EndSessionAsync()
        {
            Log.Debug("[SessionCountlyService] EndSessionAsync");

            if (!_consentService.CheckConsentInternal(Consents.Sessions)) {
                return;
            }

            if (!IsSessionInitiated) {
                Log.Warning("[SessionCountlyService] EndSessionAsync: The session isn't started yet!");
                return;
            }

            IsSessionInitiated = false;
            _eventService.AddEventsToRequestQueue();
            Countly.Instance.UserProfile.Save();
            Dictionary<string, object> requestParams = new Dictionary<string, object>
                {
                    {"end_session", 1},
                    {"session_duration",  Convert.ToInt32((DateTime.Now - _lastSessionRequestTime).TotalSeconds)}
                };

            _requestCountlyHelper.AddToRequestQueue(requestParams);
            await _requestCountlyHelper.ProcessQueue();
        }

        /// <summary>
        /// Extends a session by another session duration provided in configuration. By default session duration is 60 seconds.
        /// </summary>
        internal async Task ExtendSessionAsync()
        {
            Log.Debug("[SessionCountlyService] ExtendSessionAsync");

            if (!_consentService.CheckConsentInternal(Consents.Sessions)) {
                return;
            }

            if (!IsSessionInitiated) {
                Log.Warning("[SessionCountlyService] ExtendSessionAsync: The session isn't started yet!");
                return;
            }

            Dictionary<string, object> requestParams = new Dictionary<string, object>
                {
                    {
                        "session_duration",  Convert.ToInt32((DateTime.Now - _lastSessionRequestTime).TotalSeconds)
                    }
                };

            _lastSessionRequestTime = DateTime.Now;

            _requestCountlyHelper.AddToRequestQueue(requestParams);
            await _requestCountlyHelper.ProcessQueue();
            
            #if UNITY_WEBGL
            _monoBehaviour.StartCoroutine(SessionTimerCoroutine());
            #endif
        }

        #region override Methods
        internal override async void ConsentChanged(List<Consents> updatedConsents, bool newConsentValue, ConsentChangedAction action)
        {
            if (updatedConsents.Contains(Consents.Sessions) && newConsentValue) {
                if (!_configuration.IsAutomaticSessionTrackingDisabled) {
                    IsSessionInitiated = false;
                    await BeginSessionAsync();
                }
            }
        }
        #endregion
    }
}