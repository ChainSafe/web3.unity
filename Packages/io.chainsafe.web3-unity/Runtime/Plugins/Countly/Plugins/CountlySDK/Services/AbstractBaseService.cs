using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugins.CountlySDK.Enums;
using Plugins.CountlySDK.Models;

namespace Plugins.CountlySDK.Services
{
    public abstract class AbstractBaseService
    {
        internal object LockObj { get; set; }
        internal List<AbstractBaseService> Listeners { get; set; }
        protected CountlyLogHelper Log { get; private set; }
        protected readonly CountlyConfiguration _configuration;
        protected readonly ConsentCountlyService _consentService;

        protected AbstractBaseService(CountlyConfiguration configuration, CountlyLogHelper logHelper, ConsentCountlyService consentService)
        {
            Log = logHelper;
            _configuration = configuration;
            _consentService = consentService;
        }

        protected IDictionary<string, object> RemoveSegmentInvalidDataTypes(IDictionary<string, object> segments)
        {
            if (segments == null || segments.Count == 0) {
                return segments;
            }

            string moduleName = GetType().Name;
            int i = 0;
            List<string> toRemove = new List<string>();
            foreach (KeyValuePair<string, object> item in segments) {
                if (++i > _configuration.GetMaxSegmentationValues()) {
                    toRemove.Add(item.Key);
                    continue;
                }

                Type type = item.Value?.GetType();
                bool isValidDataType = item.Value != null
                    && (type == typeof(int)
                    || type == typeof(bool)
                    || type == typeof(float)
                    || type == typeof(double)
                    || type == typeof(string)
                    || type == typeof(long)
                    || (type.IsArray && IsValidElementType(type.GetElementType()))
                    || (IsListType(type) && IsValidElementType(type.GetGenericArguments()[0])));

                if (!isValidDataType) {
                    toRemove.Add(item.Key);
                    Log.Warning($"[{moduleName}][RemoveSegmentInvalidDataTypes]: In provided segmentation, value: [{item.Value}], for key: [{item.Key}], is not an allowed data type. Will be removed.");
                }
            }

            foreach (string k in toRemove) {
                segments.Remove(k);
            }

            return segments;
        }

        private bool IsValidElementType(Type type)
        {
            return type == typeof(int)
                || type == typeof(bool)
                || type == typeof(float)
                || type == typeof(double)
                || type == typeof(string)
                || type == typeof(long);
        }

        private bool IsValidDataType(object value)
        {
            if (value == null) {
                return false;
            }

            Type type = value.GetType();

            return type == typeof(int)
                || type == typeof(bool)
                || type == typeof(float)
                || type == typeof(double)
                || type == typeof(string)
                || type == typeof(long)
                || (type.IsArray && IsValidElementType(type.GetElementType()))
                || (IsListType(type) && IsValidElementType(type.GetGenericArguments()[0]));
        }

        private bool IsListType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }

        protected string TrimKey(string k)
        {
            if (k.Length > _configuration.GetMaxKeyLength()) {
                Log.Warning($"[{GetType().Name}][TrimKey]: Max allowed key length is {_configuration.GetMaxKeyLength()}. Key: [{k}] will be truncated.");
                k = k.Substring(0, _configuration.GetMaxKeyLength());
            }

            return k;
        }

        protected string[] TrimValues(string[] values)
        {
            for (int i = 0; i < values.Length; ++i) {
                if (values[i].Length > _configuration.GetMaxValueSize()) {
                    Log.Warning($"[{GetType().Name}][TrimValues]: Max allowed value length is {_configuration.GetMaxValueSize()}. Value: [{values[i]}] will be truncated.");
                    values[i] = values[i].Substring(0, _configuration.GetMaxValueSize());
                }
            }

            return values;
        }

        protected string TrimValue(string fieldName, string v)
        {
            if (v != null && v.Length > _configuration.GetMaxValueSize()) {
                Log.Warning($"[{GetType().Name}][TrimValue]: Max allowed length for [{fieldName}] is {_configuration.GetMaxValueSize()}. Value: [{v}] will be truncated.");
                v = v.Substring(0, _configuration.GetMaxValueSize());
            }

            return v;
        }

        protected IDictionary<string, object> FixSegmentKeysAndValues(IDictionary<string, object> segments)
        {
            if (segments == null || segments.Count == 0) {
                return segments;
            }

            IDictionary<string, object> segmentation = new Dictionary<string, object>();
            foreach (KeyValuePair<string, object> item in segments) {
                string k = item.Key;
                object v = item.Value;

                if (k == null || k.Length == 0) {
                    Log.Warning($"[{GetType().Name}][FixSegmentKeysAndValues]: Provided key is {(k == null ? "null" : "empty")}, will be skipped.");
                    continue;
                }
                if (v == null) {
                    Log.Warning($"[{GetType().Name}][FixSegmentKeysAndValues]: Provided value for '[{k}]' is null, will be skipped.");
                    continue;
                }

                k = TrimKey(k);

                if (v.GetType() == typeof(string)) {
                    v = TrimValue(k, (string)v);
                }

                segmentation.Add(k, v);
            }

            return segmentation;
        }

        protected void LogSegmentation(Dictionary<string, object> data, string prefix)
        {
            if (data != null && data.Count > 0) {
                StringBuilder validEntries = new StringBuilder();
                StringBuilder invalidEntries = new StringBuilder();

                foreach (var kvp in data) {
                    string valueType = kvp.Value?.GetType().ToString() ?? "null";
                    string valueStr = kvp.Value?.ToString() ?? "null";

                    if (IsValidDataType(kvp.Value)) {
                        if (kvp.Value is Array array) {
                            valueStr = "{" + string.Join(", ", array.Cast<object>()) + "}";
                        } else if (IsListType(kvp.Value.GetType())) {
                            var list = kvp.Value as System.Collections.IEnumerable;
                            valueStr = "{" + string.Join(", ", list.Cast<object>()) + "}";
                        }

                        validEntries.AppendLine($"Key: {kvp.Key}, Value: {valueStr}, Type: {valueType}");
                    } else {
                        invalidEntries.AppendLine($"Key: {kvp.Key}, Value: {valueStr}, Type: {valueType}");
                    }
                }

                if (validEntries.Length > 0) {
                    Log.Info($"{prefix} Valid Entries:\n{validEntries}");
                }

                if (invalidEntries.Length > 0) {
                    Log.Warning($"{prefix} Invalid Entries (skipped):\n{invalidEntries}");
                }
            } else {
                Log.Info($"{prefix} No values provided");
            }
        }

        internal virtual void OnInitializationCompleted() { }
        internal virtual void DeviceIdChanged(string deviceId, bool merged) { }
        internal virtual void ConsentChanged(List<Consents> updatedConsents, bool newConsentValue, ConsentChangedAction action) { }
    }

    internal enum ConsentChangedAction
    {
        Initialization,
        ConsentUpdated,
        DeviceIDChangedNotMerged,
    }
}
