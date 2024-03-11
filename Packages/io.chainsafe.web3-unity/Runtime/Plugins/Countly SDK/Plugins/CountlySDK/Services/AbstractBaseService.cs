
using System;
using System.Collections;
using System.Collections.Generic;
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

            if (segments == null || segments.Count == 0)
            {
                return segments;
            }

            string moduleName = GetType().Name;
            int i = 0;
            List<string> toRemove = new List<string>();
            foreach (KeyValuePair<string, object> item in segments)
            {
                if (++i > _configuration.MaxSegmentationValues)
                {
                    toRemove.Add(item.Key);
                    continue;
                }
                Type type = item.Value?.GetType();
                bool isValidDataType = item.Value != null
                    && (type == typeof(int)
                    || type == typeof(bool)
                    || type == typeof(float)
                    || type == typeof(double)
                    || type == typeof(string));


                if (!isValidDataType)
                {
                    toRemove.Add(item.Key);
                    Log.Warning("[" + moduleName + "] RemoveSegmentInvalidDataTypes: In segmentation Data type '" + type + "' of item '" + item.Key + "' isn't valid.");
                }
            }

            foreach (string k in toRemove)
            {
                segments.Remove(k);
            }

            return segments;
        }

        protected string TrimKey(string k)
        {
            if (k.Length > _configuration.MaxKeyLength)
            {
                Log.Warning("[" + GetType().Name + "] TrimKey : Max allowed key length is " + _configuration.MaxKeyLength + ". " + k + " will be truncated.");
                k = k.Substring(0, _configuration.MaxKeyLength);
            }

            return k;
        }

        protected string[] TrimValues(string[] values)
        {
            for (int i = 0; i < values.Length; ++i)
            {
                if (values[i].Length > _configuration.MaxValueSize)
                {
                    Log.Warning("[" + GetType().Name + "] TrimKey : Max allowed value length is " + _configuration.MaxKeyLength + ". " + values[i] + " will be truncated.");
                    values[i] = values[i].Substring(0, _configuration.MaxValueSize);
                }
            }


            return values;
        }

        protected string TrimValue(string fieldName, string v)
        {
            if (v != null && v.Length > _configuration.MaxValueSize)
            {
                Log.Warning("[" + GetType().Name + "] TrimValue : Max allowed '" + fieldName + "' length is " + _configuration.MaxValueSize + ". " + v + " will be truncated.");
                v = v.Substring(0, _configuration.MaxValueSize);
            }

            return v;
        }

        protected IDictionary<string, object> FixSegmentKeysAndValues(IDictionary<string, object> segments)
        {
            if (segments == null || segments.Count == 0)
            {
                return segments;
            }

            IDictionary<string, object> segmentation = new Dictionary<string, object>();
            foreach (KeyValuePair<string, object> item in segments)
            {
                string k = item.Key;
                object v = item.Value;

                if (k == null || k.Length == 0 || v == null)
                {
                    continue;
                }

                k = TrimKey(k);

                if (v.GetType() == typeof(string))
                {
                    v = TrimValue(k, (string)v);
                }

                segmentation.Add(k, v);
            }

            return segmentation;
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
