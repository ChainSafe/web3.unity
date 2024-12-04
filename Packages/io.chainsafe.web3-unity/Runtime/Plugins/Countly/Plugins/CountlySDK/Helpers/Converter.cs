using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Plugins.CountlySDK.Models;
using Plugins.CountlySDK.Persistance.Entities;

namespace Plugins.CountlySDK.Helpers
{
    /// Providing methods for converting data between different models and JSON representations.
    public static class Converter
    {
        /// <summary>
        /// Converts an EventEntity object to a CountlyEventModel object.
        /// </summary>
        /// <returns>
        /// CountlyEventModel object representing the converted data, or null if the input entity is null or invalid.
        /// </returns>
        public static CountlyEventModel ConvertEventEntityToEventModel(EventEntity entity, CountlyLogHelper L)
        {
            // Check if the input EventEntity is null
            if (entity == null) {
                L?.Warning("[Converter] 'ConvertEventEntityToEventModel': EventEntity variable is null");
                return null;
            }
            if (string.IsNullOrEmpty(entity.Json)) {
                L?.Warning("[Converter] 'ConvertEventEntityToEventModel': EventEntity.Json variable is null or empty");
                return null;
            }

            try {

                CountlyEventModel model = JsonConvert.DeserializeObject<CountlyEventModel>(entity.Json);
                model.Id = entity.Id;

                return model;

            } catch (Exception ex) {
                // Handle JSON serialization error
                L?.Warning($"[Converter] 'ConvertEventEntityToEventModel': JSON serialization error: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Converts a CountlyEventModel object to an EventEntity object.
        /// </summary>
        /// <returns>EventEntity object representing the converted data.</returns>
        public static EventEntity ConvertEventModelToEventEntity(CountlyEventModel model, long id)
        {
            string json = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            model.Id = id;

            return new EventEntity {
                Id = id,
                Json = json
            };
        }

        /// <summary>
        /// Converts a SegmentEntity object to a SegmentModel object.
        /// </summary>
        /// <returns>SegmentModel object representing the converted data.</returns>
        public static SegmentModel ConvertSegmentEntityToSegmentModel(SegmentEntity entity)
        {
            SegmentModel model = JsonConvert.DeserializeObject<SegmentModel>(entity.Json);
            model.Id = entity.Id;
            return model;
        }

        /// <summary>
        /// Converts a SegmentModel object to a SegmentEntity object.
        /// </summary>
        /// <returns>SegmentEntity object representing the converted data.</returns>
        public static SegmentEntity ConvertSegmentModelToSegmentEntity(SegmentModel model, long id)
        {
            string json = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            model.Id = id;

            return new SegmentEntity {
                Id = id,
                Json = json
            };
        }

        /// <summary>
        /// Converts a RequestEntity object to a CountlyRequestModel object.
        /// </summary>
        /// <returns>CountlyRequestModel object representing the converted data.</returns>
        public static CountlyRequestModel ConvertRequestEntityToRequestModel(RequestEntity entity)
        {
            CountlyRequestModel model = JsonConvert.DeserializeObject<CountlyRequestModel>(entity.Json);
            model.Id = entity.Id;
            return model;
        }

        /// <summary>
        /// Converts a CountlyRequestModel object to a RequestEntity object.
        /// </summary>
        /// <returns>RequestEntity object representing the converted data.</returns>
        public static RequestEntity ConvertRequestModelToRequestEntity(CountlyRequestModel model, long id)
        {
            string json = JsonConvert.SerializeObject(model, Formatting.Indented,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            model.Id = id;

            return new RequestEntity {
                Id = id,
                Json = json
            };
        }

        /// <summary>
        /// Converts a JSON string to a Dictionary of string and object.
        /// </summary>
        /// <returns>Dictionary of string and object representing the converted JSON data, or null if the input JSON string is null.</returns>
        public static Dictionary<string, object> ConvertJsonToDictionary(string json, CountlyLogHelper L)
        {
            if (json == null) {
                L?.Warning("[Converter] 'ConvertJsonToDictionary': Provided Json is null");
                return null;
            }

            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }
    }
}
