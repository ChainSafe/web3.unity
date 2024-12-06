using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Plugins.CountlySDK.Persistance;
#nullable enable

namespace Plugins.CountlySDK.Models
{
    [Serializable]
    public class CountlyEventModel : IModel
    {
        /// <summary>
        /// Initializes a new instance of event model.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="segmentation"></param>
        /// <param name="count"></param>
        /// <param name="sum"></param>
        /// <param name="duration"></param>
        public CountlyEventModel(string key, IDictionary<string, object>? segmentation = null, int? count = 1, double? sum = null, double? duration = null, string? eventId = null, string? pvid = null, string? cvid = null, string? peid = null)
        {
            Key = key;
            Count = count ?? 1;
            if (segmentation != null) {
                Segmentation = new SegmentModel(segmentation);
            }
            Duration = duration;
            Sum = sum;

            TimeMetricModel timeModel = TimeMetricModel.GetTimeZoneInfoForRequest();

            Hour = timeModel.Hour;
            DayOfWeek = timeModel.DayOfWeek;
            Timestamp = timeModel.Timestamp;

            EventID = eventId;
            PreviousEventID = peid;
            PreviousViewID = pvid;
            CurrentViewID = cvid;
        }

        public CountlyEventModel()
        {
        }

        [JsonIgnore]
        public long Id { get; set; }
        [JsonProperty("key")] public string? Key { get; set; }

        [JsonProperty("count")] public int? Count { get; set; }

        [JsonProperty("sum")] public double? Sum { get; set; }

        [JsonProperty("dur")] public double? Duration { get; set; }

        [JsonProperty("segmentation")] public SegmentModel? Segmentation { get; set; }

        [JsonProperty("timestamp")] public long Timestamp { get; set; }

        [JsonProperty("hour")] public int Hour { get; set; }

        [JsonProperty("dow")] public int DayOfWeek { get; set; }

        [JsonProperty("id")] public string? EventID { get; set; }

        [JsonProperty("pvid")] public string? PreviousViewID { get; set; }

        [JsonProperty("cvid")] public string? CurrentViewID { get; set; }

        [JsonProperty("peid")] public string? PreviousEventID { get; set; }

        #region Reserved Event Names
        [JsonIgnore] public const string NPSEvent = "[CLY]_nps";

        [JsonIgnore] public const string ViewEvent = "[CLY]_view";

        [JsonIgnore] public const string SurveyEvent = "[CLY]_survey";

        [JsonIgnore] public const string ViewActionEvent = "[CLY]_action";

        [JsonIgnore] public const string StarRatingEvent = "[CLY]_star_rating";

        [JsonIgnore] public const string PushActionEvent = "[CLY]_push_action";

        [JsonIgnore] public const string OrientationEvent = "[CLY]_orientation";
        #endregion

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Key)}: {Key}, {nameof(Count)}: {Count}, {nameof(Sum)}: {Sum}, {nameof(Duration)}: {Duration}, {nameof(Segmentation)}: {Segmentation}, {nameof(Timestamp)}: {Timestamp}, {nameof(Hour)}: {Hour}, {nameof(DayOfWeek)}: {DayOfWeek}, {nameof(EventID)}: {EventID}, {nameof(PreviousViewID)}: {PreviousViewID}, {nameof(CurrentViewID)}: {CurrentViewID}, {nameof(PreviousEventID)}: {PreviousEventID},";
        }
    }
}