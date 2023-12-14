using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;

namespace Plugins.CountlySDK.Models
{
    internal class TimeMetricModel
    {
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
        [JsonProperty("hour")]
        public int Hour { get; set; }
        [JsonProperty("dow")]
        public int DayOfWeek { get; set; }
        [JsonProperty("tz")]
        public string Timezone { get; set; }

        //variable to hold last used timestamp
        private DateTimeOffset _lastMilliSecTimeStamp = DateTimeOffset.UtcNow;

        static TimeMetricModel() { }
        private TimeMetricModel() { }

        internal static Dictionary<string, object> GetTimeMetricModel()
        {
            TimeMetricModel model = TimeMetricModel.GetTimeZoneInfoForRequest();
            return new Dictionary<string, object>
            {
                {"timestamp", model.Timestamp },
                {"hour", model.Hour },
                {"dow", model.DayOfWeek },
                {"tz", model.Timezone },
            };
        }

        private long GetUniqueMilliSecTimeStamp(DateTime? requestedDatetime = null)
        {
            //get current timestamp in miliseconds
            DateTimeOffset currentMilliSecTimeStamp = DateTimeOffset.UtcNow;

            if (requestedDatetime.HasValue) {
                currentMilliSecTimeStamp = requestedDatetime.Value;

                _lastMilliSecTimeStamp = _lastMilliSecTimeStamp >= currentMilliSecTimeStamp
                                        ? _lastMilliSecTimeStamp.AddMilliseconds(1)
                                        : _lastMilliSecTimeStamp = currentMilliSecTimeStamp;
            } else {
                _lastMilliSecTimeStamp = currentMilliSecTimeStamp;
            }

            return _lastMilliSecTimeStamp.ToUnixTimeMilliseconds();
        }

        internal static TimeMetricModel GetTimeZoneInfoForRequest()
        {
            DateTime currentDateTime = DateTime.Now;
            TimeMetricModel model =
                new TimeMetricModel {
                    Hour = currentDateTime.TimeOfDay.Hours,
                    DayOfWeek = (int)currentDateTime.DayOfWeek,
                    Timezone = TimeZone.CurrentTimeZone.GetUtcOffset(currentDateTime).TotalMinutes.ToString(CultureInfo.InvariantCulture)
                };

            model.Timestamp = model.GetUniqueMilliSecTimeStamp(currentDateTime);
            return model;
        }
    }
}
