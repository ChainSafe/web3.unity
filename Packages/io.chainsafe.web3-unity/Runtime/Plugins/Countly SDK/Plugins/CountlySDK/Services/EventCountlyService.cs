using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugins.CountlySDK.Enums;
using Plugins.CountlySDK.Helpers;
using Plugins.CountlySDK.Models;
using Plugins.CountlySDK.Persistance.Repositories.Impls;

namespace Plugins.CountlySDK.Services
{
    public class EventCountlyService : AbstractBaseService
    {
        private bool isQueueBeingProcessed = false;
        internal readonly NonViewEventRepository _eventRepo;
        private readonly RequestCountlyHelper _requestCountlyHelper;

        internal readonly IDictionary<string, DateTime> _timedEvents;

        internal EventCountlyService(CountlyConfiguration configuration, CountlyLogHelper logHelper, RequestCountlyHelper requestCountlyHelper, NonViewEventRepository nonViewEventRepo, ConsentCountlyService consentService) : base(configuration, logHelper, consentService)
        {
            Log.Debug("[EventCountlyService] Initializing.");

            _eventRepo = nonViewEventRepo;
            _requestCountlyHelper = requestCountlyHelper;
            _timedEvents = new Dictionary<string, DateTime>();
        }

        /// <summary>
        /// Add all recorded events to request queue
        /// </summary>
        internal void CancelAllTimedEvents()
        {
            _timedEvents.Clear();
        }

        /// <summary>
        /// Add all recorded events to request queue
        /// </summary>
        internal void AddEventsToRequestQueue()
        {

            Log.Debug("[EventCountlyService] AddEventsToRequestQueue: Start");

            if (_eventRepo.Models.Count == 0)
            {
                Log.Debug("[EventCountlyService] AddEventsToRequestQueue: Event queue is empty!");
                return;
            }

            if (isQueueBeingProcessed)
            {
                Log.Verbose("[EventCountlyService] AddEventsToRequestQueue: Event queue being processed!");
                return;
            }
            isQueueBeingProcessed = true;

            int count = _eventRepo.Models.Count;
            //Send all at once
            Dictionary<string, object> requestParams =
                new Dictionary<string, object>
                {
                    {
                        "events", JsonConvert.SerializeObject(_eventRepo.Models, Formatting.Indented,
                            new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore})
                    }
                };

            _requestCountlyHelper.AddToRequestQueue(requestParams);

            Log.Debug("[EventCountlyService] AddEventsToRequestQueue: Remove events from event queue, count: " + count);
            for (int i = 0; i < count; ++i)
            {
                _eventRepo.Dequeue();
            }

            isQueueBeingProcessed = false;
            Log.Debug("[EventCountlyService] AddEventsToRequestQueue: End");
        }

        // <summary>
        /// An internal method to record an event to the server with segmentation.
        /// </summary>
        /// <param name="key">event key</param>
        /// <param name="segmentation">custom segmentation you want to set, leave null if you don't want to add anything</param>
        /// <param name="count">how many of these events have occurred, default value is "1"</param>
        /// <param name="sum">set sum if needed, default value is "0"</param>
        /// <param name="duration">set sum if needed, default value is "0"</param>
        /// <returns></returns>
        internal async Task RecordEventInternal(string key, IDictionary<string, object> segmentation = null,
            int? count = 1, double? sum = 0, double? duration = null)
        {
            if (!CheckConsentOnKey(key))
            {
                return;
            }

            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                Log.Warning("[EventCountlyService] RecordEventInternal : The event key '" + key + "'isn't valid.");
                return;
            }

            if (_configuration.EnableTestMode)
            {
                return;
            }

            if (key.Length > _configuration.MaxKeyLength)
            {
                Log.Warning("[EventCountlyService] RecordEventInternal : Max allowed key length is " + _configuration.MaxKeyLength);
                key = key.Substring(0, _configuration.MaxKeyLength);
            }

            IDictionary<string, object> segments = RemoveSegmentInvalidDataTypes(segmentation);
            segments = FixSegmentKeysAndValues(segments);

            CountlyEventModel @event = new CountlyEventModel(key, segments, count, sum, duration);

            await RecordEventAsync(@event);

        }

        /// <summary>
        /// An internal function to add an event to event queue.
        /// </summary>
        /// <param name="event">an event</param>
        /// <returns></returns>
        internal async Task RecordEventAsync(CountlyEventModel @event)
        {
            Log.Debug("[EventCountlyService] RecordEventAsync : " + @event.ToString());

            if (_configuration.EnableTestMode)
            {
                return;
            }

            _eventRepo.Enqueue(@event);

            if (_eventRepo.Count >= _configuration.EventQueueThreshold)
            {
                AddEventsToRequestQueue();
                await _requestCountlyHelper.ProcessQueue();
            }
        }

        private bool CheckConsentOnKey(string key)
        {
            if (key.Equals(CountlyEventModel.ViewEvent))
            {
                return _consentService.CheckConsentInternal(Consents.Views);
            }
            else if (key.Equals(CountlyEventModel.StarRatingEvent))
            {
                return _consentService.CheckConsentInternal(Consents.StarRating);
            }
            else if (key.Equals(CountlyEventModel.PushActionEvent))
            {
                return _consentService.CheckConsentInternal(Consents.Push);
            }
            else if (key.Equals(CountlyEventModel.ViewActionEvent))
            {
                return _consentService.CheckConsentInternal(Consents.Clicks);
            }
            else if (key.Equals(CountlyEventModel.NPSEvent))
            {
                return _consentService.CheckConsentInternal(Consents.Feedback);
            }
            else if (key.Equals(CountlyEventModel.SurveyEvent))
            {
                return _consentService.CheckConsentInternal(Consents.Feedback);
            }
            else if (key.Equals(CountlyEventModel.OrientationEvent))
            {
                return _consentService.CheckConsentInternal(Consents.Users);
            }
            else { return _consentService.CheckConsentInternal(Consents.Events); }
        }

        /// <summary>
        /// Start a timed event.
        /// </summary>
        /// <param name="key">event key</param>
        /// <returns></returns>
        public void StartEvent(string key)
        {
            lock (LockObj)
            {
                Log.Info("[EventCountlyService] StartEvent : key = " + key);

                if (!_consentService.CheckConsentInternal(Consents.Events))
                {
                    return;
                }

                if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                {
                    Log.Warning("[EventCountlyService] StartEvent : The event key '" + key + "' isn't valid.");
                    return;
                }

                if (_timedEvents.ContainsKey(key))
                {
                    Log.Warning("[EventCountlyService] StartEvent : Event with key '" + key + "' has already started.");
                    return;
                }

                TimeMetricModel timeModel = TimeMetricModel.GetTimeZoneInfoForRequest();
                _timedEvents.Add(key, DateTime.Now);
            }

        }

        /// <summary>
        /// Cancel a timed event.
        /// </summary>
        /// <param name="key">event key</param>
        /// <returns></returns>
        public void CancelEvent(string key)
        {
            lock (LockObj)
            {
                Log.Info("[EventCountlyService] CancelEvent : key = " + key);

                if (!_consentService.CheckConsentInternal(Consents.Events))
                {
                    return;
                }

                if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                {
                    Log.Warning("[EventCountlyService] CancelEvent : The event key '" + key + "' isn't valid.");
                    return;
                }

                if (!_timedEvents.ContainsKey(key))
                {
                    Log.Warning("[EventCountlyService] CancelEvent : Time event with key '" + key + "' doesn't exist.");
                    return;
                }

                _timedEvents.Remove(key);
            }

        }

        /// <summary>
        /// End a timed event.
        /// </summary>
        /// <param name="key">event key</param>
        /// <param name="segmentation">custom segmentation you want to set, leave null if you don't want to add anything</param>
        /// <param name="count">how many of these events have occurred, default value is "1"</param>
        /// <param name="sum">set sum if needed, default value is "0"</param>
        /// <returns></returns>
        public void EndEvent(string key, IDictionary<string, object> segmentation = null, int? count = 1, double? sum = 0)
        {
            lock (LockObj)
            {
                Log.Info("[EventCountlyService] EndEvent : key = " + key + ", segmentation = " + segmentation + ", count = " + count + ", sum = " + sum);

                if (!_consentService.CheckConsentInternal(Consents.Events))
                {
                    return;
                }

                if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                {
                    Log.Warning("[EventCountlyService] EndEvent : The event key '" + key + "' isn't valid.");
                    return;
                }

                if (!_timedEvents.ContainsKey(key))
                {
                    Log.Warning("[EventCountlyService] EndEvent : Time event with key '" + key + "' doesn't exist.");
                    return;
                }

                DateTime startTime = _timedEvents[key];
                double duration = (DateTime.Now - startTime).TotalSeconds;

                CountlyEventModel @event = new CountlyEventModel(key, segmentation, count, sum, duration);
                _ = RecordEventAsync(@event);

                _timedEvents.Remove(key);
            }

        }

        /// <summary>
        /// Report an event to the server with segmentation.
        /// </summary>
        /// <param name="key">event key</param>
        /// <param name="segmentation">custom segmentation you want to set, leave null if you don't want to add anything</param>
        /// <param name="count">how many of these events have occurred, default value is "1"</param>
        /// <param name="sum">set sum if needed, default value is "0"</param>
        /// <param name="duration">set sum if needed, default value is "0"</param>
        /// <returns></returns>
        public async Task RecordEventAsync(string key, IDictionary<string, object> segmentation = null,
            int? count = 1, double? sum = 0, double? duration = null)
        {
            lock (LockObj)
            {
                Log.Info("[EventCountlyService] RecordEventAsync : key = " + key + ", segmentation = " + segmentation + ", count = " + count + ", sum = " + sum + ", duration = " + duration);

                _ = RecordEventInternal(key, segmentation, count, sum, duration);
            }

            await Task.CompletedTask;
        }

        #region override Methods
        internal override void ConsentChanged(List<Consents> updatedConsents, bool newConsentValue, ConsentChangedAction action)
        {
            if (updatedConsents.Contains(Consents.Events) && !newConsentValue)
            {
                CancelAllTimedEvents();
            }
        }
        #endregion
    }
}
