using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Reown.AppKit.Unity.Http;
using Reown.Core.Common.Logging;
using UnityEngine;

namespace Reown.AppKit.Unity
{
    public class EventsController
    {
        private const string BasePath = "https://pulse.walletconnect.org/";
        private const int TimoutSeconds = 5;

        private readonly UnityHttpClient _httpClient = new(new Uri(BasePath), TimeSpan.FromSeconds(TimoutSeconds), new PulseApiHeaderDecorator());

        private AnalyticsState _state = AnalyticsState.Loading;

        public async Task InitializeAsync(AppKitConfig config, ApiController apiController)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            _state = AnalyticsState.Disabled;
            return;
#endif

            if (!config.enableAnalytics)
            {
                _state = AnalyticsState.Disabled;
                return;
            }

            await LoadRemoteAnalyticsConfig(apiController);
        }


        private async Task LoadRemoteAnalyticsConfig(ApiController apiController)
        {
            try
            {
                var response = await apiController.GetAnalyticsConfigAsync();

                _state = response.isAnalyticsEnabled
                    ? AnalyticsState.Enabled
                    : AnalyticsState.Disabled;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                _state = AnalyticsState.Disabled;
            }
        }

        public async void SendEvent(Event @event)
        {
            try
            {
                Debug.Log($"Send Event {@event.name}. State: {_state.ToString()}");
                if (_state == AnalyticsState.Disabled)
                    return;

                var request = new EventRequest
                {
                    eventId = Guid.NewGuid().ToString(),
                    timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
#if UNITY_ANDROID || UNITY_IOS || UNITY_VISIONOS
                    bundleId = Application.identifier,
#endif
                    props = @event
                };

                var requestJson = JsonConvert.SerializeObject(request);

                ReownLogger.Log($"[EventsController] Sending event: {@event.name}.\n\nRequest payload:\n {requestJson}");

                await _httpClient.PostAsync("e", requestJson);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private enum AnalyticsState
        {
            Loading,
            Enabled,
            Disabled
        }
    }

    [Serializable]
    internal struct EventRequest
    {
        public string eventId;
        public long timestamp;
        public string bundleId;
        public Event props;
    }

    [Serializable]
    public struct Event
    {
        [JsonProperty("type")]
        public const string type = "track";

        [JsonProperty("event")]
        public string name;

        [JsonProperty("properties", NullValueHandling = NullValueHandling.Ignore)]
        public IDictionary<string, object> properties;
    }
}