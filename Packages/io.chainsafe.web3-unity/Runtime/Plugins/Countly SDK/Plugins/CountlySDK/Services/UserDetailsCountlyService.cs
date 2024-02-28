using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugins.CountlySDK.Enums;
using Plugins.CountlySDK.Helpers;
using Plugins.CountlySDK.Models;

namespace Plugins.CountlySDK.Services
{
    public class UserDetailsCountlyService : AbstractBaseService
    {
        internal Dictionary<string, object> CustomDataProperties { get; private set; }

        private readonly CountlyUtils _countlyUtils;
        internal readonly RequestCountlyHelper _requestCountlyHelper;
        internal UserDetailsCountlyService(CountlyConfiguration configuration, CountlyLogHelper logHelper, RequestCountlyHelper requestCountlyHelper, CountlyUtils countlyUtils, ConsentCountlyService consentService) : base(configuration, logHelper, consentService)
        {
            Log.Debug("[UserDetailsCountlyService] Initializing.");

            _countlyUtils = countlyUtils;
            _requestCountlyHelper = requestCountlyHelper;
            CustomDataProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Add user custom detail to request queue.
        /// </summary>
        /// <returns></returns>
        private void AddCustomDetailToRequestQueue(IDictionary<string, object> segments)
        {

            IDictionary<string, object> customDetail = FixSegmentKeysAndValues(segments);

            Dictionary<string, object> requestParams =
                new Dictionary<string, object>
                {
                    { "user_details",
                        JsonConvert.SerializeObject(
                            new Dictionary<string, object>
                            {
                                { "custom", customDetail }
                            })
                    }
                };
            _requestCountlyHelper.AddToRequestQueue(requestParams);
            _ = _requestCountlyHelper.ProcessQueue();
        }

        /// <summary>
        /// Sets information about user.
        /// </summary>
        /// <param name="userDetailsModel">User Model with the specified params</param>
        /// <returns></returns>
        public async Task SetUserDetailsAsync(CountlyUserDetailsModel userDetailsModel)
        {
            lock (LockObj)
            {
                Log.Info("[UserDetailsCountlyService] SetUserDetailsAsync " + (userDetailsModel != null));

                if (!_consentService.CheckConsentInternal(Consents.Users))
                {
                    return;
                }

                if (userDetailsModel == null)
                {
                    Log.Warning("[UserDetailsCountlyService] SetUserDetailsAsync : The parameter 'userDetailsModel' can't be null.");
                    return;
                }

                if (!_countlyUtils.IsPictureValid(userDetailsModel.PictureUrl))
                {
                    Log.Warning($"[UserDetailsCountlyService] SetUserDetailAsync: Picture format for URL '{userDetailsModel.PictureUrl}' is not as expected. Expected formats are .png, .gif, or .jpeg");
                }


                userDetailsModel.Name = TrimValue("Name", userDetailsModel.Name);
                userDetailsModel.Phone = TrimValue("Phone", userDetailsModel.Phone);
                userDetailsModel.Email = TrimValue("Email", userDetailsModel.Email);
                userDetailsModel.Gender = TrimValue("Gender", userDetailsModel.Gender);
                userDetailsModel.Username = TrimValue("Username", userDetailsModel.Username);
                userDetailsModel.BirthYear = TrimValue("BirthYear", userDetailsModel.BirthYear);
                userDetailsModel.Organization = TrimValue("Organization", userDetailsModel.Organization);

                if (userDetailsModel.PictureUrl != null && userDetailsModel.PictureUrl.Length > 4096)
                {
                    Log.Warning("[" + GetType().Name + "] TrimValue : Max allowed length of 'PictureUrl' is " + _configuration.MaxValueSize);
                    userDetailsModel.PictureUrl = userDetailsModel.PictureUrl.Substring(0, 4096);
                }

                userDetailsModel.Custom = FixSegmentKeysAndValues(userDetailsModel.Custom);
                Dictionary<string, object> requestParams =
                    new Dictionary<string, object>
                    {
                    { "user_details", JsonConvert.SerializeObject(userDetailsModel, Formatting.Indented,
                        new JsonSerializerSettings{ NullValueHandling = NullValueHandling.Ignore }) },
                    };

                _requestCountlyHelper.AddToRequestQueue(requestParams);
                _ = _requestCountlyHelper.ProcessQueue();
            }

            await Task.CompletedTask;
        }
        /// <summary>
        /// Sets information about user with custom properties.
        /// In custom properties you can provide any string key values to be stored with user.
        /// </summary>
        /// <param name="customDetail">User custom detail</param>
        /// <returns></returns>
        public void SetCustomUserDetails(Dictionary<string, object> customDetail)
        {
            Log.Info("[UserDetailsCountlyService] SetCustomUserDetails " + (customDetail != null));

            if (!_consentService.CheckConsentInternal(Consents.Users))
            {
                return;
            }

            if (customDetail == null || customDetail.Count == 0)
            {
                Log.Warning("[UserDetailsCountlyService] SetCustomUserDetails : Provided custom detail 'customDetail' can't be null or empty.");

                return;
            }

            AddCustomDetailToRequestQueue(customDetail);
        }

        /// <summary>
        /// Send provided values to server.
        /// </summary>
        /// <returns></returns>
        public async Task SaveAsync()
        {
            lock (LockObj)
            {
                if (!CustomDataProperties.Any())
                {
                    return;
                }

                Log.Info("[UserDetailsCountlyService] SaveAsync");


                CountlyUserDetailsModel model = new CountlyUserDetailsModel(CustomDataProperties);

                CustomDataProperties = new Dictionary<string, object> { };
                AddCustomDetailToRequestQueue(CustomDataProperties);
            }
            await Task.CompletedTask;
        }


        /// <summary>
        /// Sets custom provide key/value as custom property.
        /// </summary>
        /// <param name="key">string with key for the property</param>
        /// <param name="value">string with value for the property</param>
        public void Set(string key, string value)
        {

            if (string.IsNullOrEmpty(key))
            {
                Log.Warning("[UserDetailsCountlyService] Set : key '" + key + "'isn't valid.");

                return;
            }

            lock (LockObj)
            {
                Log.Info("[UserDetailsCountlyService] Set : key = " + key + ", value = " + value);

                AddToCustomData(key, TrimValue(key, value));
            }
        }

        /// <summary>
        /// Set value only if property does not exist yet.
        /// </summary>
        /// <param name="key">string with property name to set</param>
        /// <param name="value">string value to set</param>
        public void SetOnce(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                Log.Warning("[UserDetailsCountlyService] SetOnce : key '" + key + "'isn't valid.");

                return;
            }

            lock (LockObj)
            {
                Log.Info("[UserDetailsCountlyService] SetOnce : key = " + key + ", value = " + value);

                AddToCustomData(key, new Dictionary<string, object> { { "$setOnce", TrimValue(key, value) } });
            }
        }

        /// <summary>
        /// Increment custom property value by 1.
        /// </summary>
        /// <param name="key">string with property name to increment</param>
        public void Increment(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                Log.Warning("[UserDetailsCountlyService] Increment : key '" + key + "'isn't valid.");

                return;
            }

            lock (LockObj)
            {
                Log.Info("[UserDetailsCountlyService] Increment : key = " + key);

                AddToCustomData(key, new Dictionary<string, object> { { "$inc", 1 } });
            }
        }

        /// <summary>
        /// Increment custom property value by provided value.
        /// </summary>
        /// <param name="key">string with property name to increment</param>
        /// <param name="value">double value by which to increment</param>
        public void IncrementBy(string key, double value)
        {
            if (string.IsNullOrEmpty(key))
            {
                Log.Warning("[UserDetailsCountlyService] IncrementBy : key '" + key + "'isn't valid.");

                return;
            }
            lock (LockObj)
            {
                Log.Info("[UserDetailsCountlyService] IncrementBy : key = " + key + ", value = " + value);

                AddToCustomData(key, new Dictionary<string, object> { { "$inc", value } });
            }
        }

        /// <summary>
        /// Multiply custom property value by provided value.
        /// </summary>
        /// <param name="key">string with property name to multiply</param>
        /// <param name="value">double value by which to multiply</param>
        public void Multiply(string key, double value)
        {
            if (string.IsNullOrEmpty(key))
            {
                Log.Warning("[UserDetailsCountlyService] Multiply : key '" + key + "'isn't valid.");

                return;
            }

            lock (LockObj)
            {
                Log.Info("[UserDetailsCountlyService] Multiply : key = " + key + ", value = " + value);

                AddToCustomData(key, new Dictionary<string, object> { { "$mul", value } });
            }
        }

        /// <summary>
        /// Save maximal value between existing and provided.
        /// </summary>
        /// <param name="key">String with property name to check for max</param>
        /// <param name="value">double value to check for max</param>
        public void Max(string key, double value)
        {
            if (string.IsNullOrEmpty(key))
            {
                Log.Warning("[UserDetailsCountlyService] Max : key '" + key + "'isn't valid.");

                return;
            }

            lock (LockObj)
            {
                Log.Info("[UserDetailsCountlyService] Max : key = " + key + ", value = " + value);

                AddToCustomData(key, new Dictionary<string, object> { { "$max", value } });
            }
        }

        /// <summary>
        /// Save minimal value between existing and provided.
        /// </summary>
        /// <param name="key">string with property name to check for min</param>
        /// <param name="value">double value to check for min</param>
        public void Min(string key, double value)
        {
            if (string.IsNullOrEmpty(key))
            {
                Log.Warning("[UserDetailsCountlyService] Min : key '" + key + "'isn't valid.");

                return;
            }

            lock (LockObj)
            {
                Log.Info("[UserDetailsCountlyService] Min : key = " + key + ", value = " + value);

                AddToCustomData(key, new Dictionary<string, object> { { "$min", value } });
            }
        }

        /// <summary>
        /// Create array property, if property does not exist and add value to array
        /// You can only use it on array properties or properties that do not exist yet.
        /// </summary>
        /// <param name="key">string with property name for array property</param>
        /// <param name="value">array with values to add</param>
        public void Push(string key, string[] value)
        {
            if (string.IsNullOrEmpty(key))
            {
                Log.Warning("[UserDetailsCountlyService] Push : key '" + key + "'isn't valid.");

                return;
            }


            lock (LockObj)
            {
                Log.Info("[UserDetailsCountlyService] Push : key = " + key + ", value = " + value);

                AddToCustomData(key, new Dictionary<string, object> { { "$push", TrimValues(value) } });
            }
        }

        /// <summary>
        /// Create array property, if property does not exist and add value to array, only if value is not yet in the array
        /// You can only use it on array properties or properties that do not exist yet.
        /// </summary>
        /// <param name="key">string with property name for array property</param>
        /// <param name="value">array with values to add</param>
        public void PushUnique(string key, string[] value)
        {
            if (string.IsNullOrEmpty(key))
            {
                Log.Warning("[UserDetailsCountlyService] PushUnique : key '" + key + "'isn't valid.");

                return;
            }


            lock (LockObj)
            {
                Log.Info("[UserDetailsCountlyService] PushUnique : key = " + key + ", value = " + value);
                AddToCustomData(key, new Dictionary<string, object> { { "$addToSet", TrimValues(value) } });
            }
        }

        /// <summary>
        /// Create array property, if property does not exist and remove value from array.
        /// </summary>
        /// <param name="key">String with property name for array property</param>
        /// <param name="value">array with values to remove from array</param>
        public void Pull(string key, string[] value)
        {
            if (string.IsNullOrEmpty(key))
            {
                Log.Warning("[UserDetailsCountlyService] Pull : key '" + key + "'isn't valid.");

                return;
            }

            lock (LockObj)
            {
                Log.Info("[UserDetailsCountlyService] Pull : key = " + key + ", value = " + value);

                value = TrimValues(value);
                AddToCustomData(key, new Dictionary<string, object> { { "$pull", value } });
            }
        }


        /// <summary>
        /// Create a property
        /// </summary>
        /// <param name="key">property name</param>
        /// <param name="value">property value</param>
        private void AddToCustomData(string key, object value)
        {
            Log.Debug("[UserDetailsCountlyService] AddToCustomData: " + key + ", " + value);

            if (!_consentService.CheckConsentInternal(Consents.Users))
            {
                return;
            }

            key = TrimKey(key);

            if (CustomDataProperties.ContainsKey(key))
            {
                string item = CustomDataProperties.Select(x => x.Key).FirstOrDefault(x => x.Equals(key, StringComparison.OrdinalIgnoreCase));
                if (item != null)
                {
                    CustomDataProperties.Remove(item);
                }
            }

            CustomDataProperties.Add(key, value);
        }

        #region override Methods
        #endregion
    }
}
