using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugins.CountlySDK;
using Plugins.CountlySDK.Enums;
using Plugins.CountlySDK.Helpers;
using Plugins.CountlySDK.Models;
using Plugins.CountlySDK.Services;

public class UserProfile : AbstractBaseService, IUserProfileModule
{
    #region UserProfileData Parameters and Keys
    internal string Name { get; set; }
    internal string Username { get; set; }
    internal string Email { get; set; }
    internal string Organization { get; set; }
    internal string Phone { get; set; }
    internal string PictureUrl { get; set; }
    internal string Gender { get; set; }
    internal int BirthYear { get; set; }
    static string NAME_KEY = "name";
    static string USERNAME_KEY = "username";
    static string EMAIL_KEY = "email";
    static string ORG_KEY = "organization";
    static string PHONE_KEY = "phone";
    static string PICTURE_KEY = "picture";
    static string GENDER_KEY = "gender";
    static string BYEAR_KEY = "byear";
    static string CUSTOM_KEY = "custom";
    readonly string[] NamedFields = { NAME_KEY, USERNAME_KEY, EMAIL_KEY, ORG_KEY, PHONE_KEY, PICTURE_KEY, GENDER_KEY, BYEAR_KEY };
    #endregion

    internal Dictionary<string, object> CustomDataProperties = new Dictionary<string, object>();
    internal Dictionary<string, JObject> CustomMods = new Dictionary<string, JObject>();
    private readonly Countly cly;
    private readonly CountlyConfiguration config;
    private readonly CountlyUtils utils;
    internal readonly RequestCountlyHelper requestHelper;
    internal readonly EventCountlyService eventService;
    internal UserProfile(Countly countly, CountlyConfiguration configuration, CountlyLogHelper logHelper, RequestCountlyHelper requestCountlyHelper, CountlyUtils countlyUtils, ConsentCountlyService consentService, EventCountlyService events) : base(configuration, logHelper, consentService)
    {
        Log.Debug("[UserProfile] Initializing.");

        cly = countly;
        config = configuration;
        utils = countlyUtils;
        requestHelper = requestCountlyHelper;
        eventService = events;
    }

    #region PublicAPI
    /// <summary>
    /// Increment custom property value by 1.
    /// </summary>
    /// <param name="key">string with property name to increment</param>
    public void Increment(string key)
    {
        Log.Info($"[UserProfile] Increment, with key: [{key}]");
        IncrementInternal(key, 1);
    }

    /// <summary>
    /// Increment custom property value by provided value.
    /// </summary>
    /// <param name="key">string with property name to increment</param>
    /// <param name="value">value by which to increment</param>
    public void IncrementBy(string key, double value)
    {
        Log.Info($"[UserProfile] IncrementBy, with key: [{key}] and value: [{value}]");
        IncrementInternal(key, value);
    }

    /// <summary>
    /// Save maximal value between existing and provided.
    /// </summary>
    /// <param name="key">string with property name to check for max</param>
    /// <param name="value">value to check for max</param>
    public void SaveMax(string key, double value)
    {
        Log.Info($"[UserProfile] Max, with key: [{key}] and value: [{value}]");
        MaxInternal(key, value);
    }

    /// <summary>
    /// Save minimal value between existing and provided.
    /// </summary>
    /// <param name="key">string with property name to check for min</param>
    /// <param name="value">value to check for min</param>
    public void SaveMin(string key, double value)
    {
        Log.Info($"[UserProfile] Min, with key: [{key}] and value: [{value}]");
        MinInternal(key, value);
    }

    /// <summary>
    /// Multiply custom property value by provided value.
    /// </summary>
    /// <param name="key">string with property name to multiply</param>
    /// <param name="value">value by which to multiply</param>
    public void Multiply(string key, double value)
    {
        Log.Info($"[UserProfile] Multiply, with key: [{key}] and value: [{value}]");
        MultiplyInternal(key, value);
    }

    /// <summary>
    /// Removes existing property from array.
    /// You can only use it on array properties
    /// </summary>
    /// <param name="key">string with property name for array property</param>
    /// <param name="value">string with value to remove from array</param>
    public void Pull(string key, string value)
    {
        Log.Info($"[UserProfile] Pull, with key: [{key}] and value: [{value}]");
        PullInternal(key, value);
    }

    /// <summary>
    /// Create array property, if property does not exist and add value to array.
    /// You can only use it on array properties or properties that do not exist yet
    /// </summary>
    /// <param name="key">string with property name for array property</param>
    /// <param name="value">string with value to add to array</param>
    public void Push(string key, string value)
    {
        Log.Info($"[UserProfile] Push, with key: [{key}] and value: [{value}]");
        PushInternal(key, value);
    }

    /// <summary>
    /// Create array property, if property does not exist and add value to array, only if value is not yet in the array.
    /// You can only use it on array properties or properties that do not exist yet
    /// </summary>
    /// <param name="key">string with property name for array property</param>
    /// <param name="value">string with value to add to array</param>
    public void PushUnique(string key, string value)
    {
        Log.Info($"[UserProfile] PushUnique, with key: [{key}] and value: [{value}]");
        PushUniqueInternal(key, value);
    }

    /// <summary>
    /// Send provided values to server.
    /// </summary>
    public void Save()
    {
        Log.Info("[UserProfile] Save");
        SaveInternal();
    }

    /// <summary>
    /// Set value only if property does not exist yet.
    /// </summary>
    /// <param name="key">string with property name to set</param>
    /// <param name="value">value string value to set</param>
    public void SetOnce(string key, string value)
    {
        Log.Info($"[UserProfile] SetOnce, with key: [{key}] and value: [{value}]");
        SetOnceInternal(key, value);
    }

    /// <summary>
    /// Provide a Dictionary of user properties to set.
    /// Those can be either custom user properties or predefined user properties.
    /// </summary>
    /// <param name="data">Dictionary with data to set</param>
    public void SetProperties(Dictionary<string, object> data)
    {
        LogSegmentation(data, "[UserProfile] SetProperties,");
        SetPropertiesInternal(data);
    }

    /// <summary>
    /// Set a single user property. It can be either a custom one or one of the predefined ones.
    /// </summary>
    /// <param name="key">string with key for the user property</param>
    /// <param name="value">value for the user property to be set. The value should be the allowed data type.</param>
    public void SetProperty(string key, object value)
    {
        Log.Info($"[UserProfile] SetProperty, with key: [{key}] and value: [{value}]");
        SetPropertyInternal(key, value);
    }
    #endregion
    #region Internal Calls
    private void IncrementInternal(string key, double value)
    {
        if (!ValidateConsentAndKey(key, "IncrementInternal")) {
            return;
        }

        Log.Info($"[UserProfile] IncrementInternal, key: [{key}] and value: [{value}]");
        ModifyCustomData(key, value, "$inc");
    }

    private void MaxInternal(string key, double value)
    {
        if (!ValidateConsentAndKey(key, "MaxInternal")) {
            return;
        }

        Log.Info($"[UserProfile] MaxInternal, key: [{key}] and value: [{value}]");
        ModifyCustomData(key, value, "$max");
    }

    private void MinInternal(string key, double value)
    {
        if (!ValidateConsentAndKey(key, "MinInternal")) {
            return;
        }

        Log.Info($"[UserProfile] MinInternal, key: [{key}] and value: [{value}]");
        ModifyCustomData(key, value, "$min");
    }

    private void MultiplyInternal(string key, double value)
    {
        if (!ValidateConsentAndKey(key, "MultiplyInternal")) {
            return;
        }

        Log.Info($"[UserProfile] MultiplyInternal, key: [{key}] and value: [{value}]");
        ModifyCustomData(key, value, "$mul");
    }

    private void PullInternal(string key, string value)
    {
        if (!ValidateConsentAndKey(key, "PullInternal")) {
            return;
        }

        Log.Info($"[UserProfile] PullInternal, key: [{key}] and value: [{value}]");
        ModifyCustomData(key, value, "$pull");
    }

    private void PushInternal(string key, string value)
    {
        if (!ValidateConsentAndKey(key, "PushInternal")) {
            return;
        }

        Log.Info($"[UserProfile] PushInternal, key: [{key}] and value: [{value}]");
        ModifyCustomData(key, value, "$push");
    }

    private void PushUniqueInternal(string key, string value)
    {
        if (!ValidateConsentAndKey(key, "PushUniqueInternal")) {
            return;
        }

        Log.Info($"[UserProfile] PushUniqueInternal, key: [{key}] and value: [{value}]");
        ModifyCustomData(key, value, "$addToSet");
    }

    private void SaveInternal()
    {
        if (!cly.IsSDKInitialized) {
            Log.Warning("[UserProfile] SaveInternal, Countly.Instance.Init() must be called before SaveInternal");
            return;
        }

        if (!_consentService.CheckConsentInternal(Consents.Users)) {
            Log.Debug("[UserProfile] SaveInternal, Consent is not given, ignoring the request.");
            return;
        }

        Dictionary<string, object> cachedUserData = GetDataForRequest();

        if (cachedUserData.Count <= 0) {
            Log.Debug("[UserProfile] SaveInternal, No user data to save");
            return;
        }

        // Serialize the dictionary to a JSON string
        string jsonData;
        try {
            jsonData = JsonConvert.SerializeObject(cachedUserData);
        } catch (JsonException ex) {
            Log.Error($"[UserProfile] SaveInternal, failed to serialize cached user data: {ex.Message}");
            return;
        }

        // Create a new dictionary to hold the modified data
        var modifiedData = new Dictionary<string, object> {
            { "user_details", jsonData }
        };

        cly.Events.AddEventsToRequestQueue();
        requestHelper.AddToRequestQueue(modifiedData);

        _ = requestHelper.ProcessQueue();
        ClearInternal();
    }

    private void SetDataInternal(Dictionary<string, object> userData)
    {
        if (!cly.IsSDKInitialized) {
            Log.Warning("[UserProfile] SetDataInternal, Countly.Instance.Init() must be called before SetDataInternal");
            return;
        }

        if (!_consentService.CheckConsentInternal(Consents.Users)) {
            Log.Debug("[UserProfile] SetDataInternal, Consent is not given, ignoring the request.");
            return;
        }

        if (userData == null || userData.Count <= 0) {
            Log.Debug("[UserProfile] SetDataInternal, Provided userData is empty, ignoring the request.");
            return;
        }

        LogSegmentation(userData, "[UserProfile] SetDataInternal,");

        if (userData.TryGetValue(NAME_KEY, out object nameValue) && nameValue is string name) {
            Name = TrimValue(NAME_KEY, name.Trim());
        }

        if (userData.TryGetValue(USERNAME_KEY, out object usernameValue) && usernameValue is string username) {
            Username = TrimValue(USERNAME_KEY, username.Trim());
        }

        if (userData.TryGetValue(EMAIL_KEY, out object emailValue) && emailValue is string email) {
            Email = TrimValue(EMAIL_KEY, email.Trim());
        }

        if (userData.TryGetValue(ORG_KEY, out object orgValue) && orgValue is string organization) {
            Organization = TrimValue(ORG_KEY, organization.Trim());
        }

        if (userData.TryGetValue(PHONE_KEY, out object phoneValue) && phoneValue is string phone) {
            Phone = TrimValue(PHONE_KEY, phone.Trim());
        }

        if (userData.TryGetValue(PICTURE_KEY, out object pictureValue) && pictureValue is string pictureUrl) {
            if (utils.IsPictureValid(pictureUrl)) {
                if (pictureUrl.Length > 4096) {
                    Log.Warning($"[UserDetailsCountlyService] SetDataInternal, Provided PictureURL length is more than 4096. Will be cropped.");
                    PictureUrl = pictureUrl.Substring(0, 4096);
                } else {
                    PictureUrl = pictureUrl.Trim();
                }
            } else {
                Log.Warning($"[UserDetailsCountlyService] SetDataInternal, Picture format for URL '{pictureUrl}' is not as expected. Expected formats are .png, .gif, or .jpeg");
            }
        }

        if (userData.TryGetValue(GENDER_KEY, out object genderValue) && genderValue is string gender) {
            Gender = TrimValue(GENDER_KEY, gender.Trim());
        }

        if (userData.TryGetValue(BYEAR_KEY, out object birthYearValue) && birthYearValue is int birthYear) {
            BirthYear = birthYear;
        }
    }

    private void SetPropertiesInternal(Dictionary<string, object> data)
    {
        if (!cly.IsSDKInitialized) {
            Log.Warning("[UserProfile] SetPropertiesInternal, Countly.Instance.Init() must be called before SetPropertiesInternal");
            return;
        }

        if (!_consentService.CheckConsentInternal(Consents.Users)) {
            Log.Debug("[UserProfile] SetPropertiesInternal, Consent is not given, ignoring the request.");
            return;
        }

        if (data == null || data.Count <= 0) {
            Log.Debug("[UserProfile] SetPropertiesInternal, Provided data is empty, or null ignoring the request.");
            return;
        }

        LogSegmentation(data, "[UserProfile] SetPropertiesInternal,");

        Dictionary<string, object> namedFields = new Dictionary<string, object>();
        Dictionary<string, object> customFields = new Dictionary<string, object>();

        // separate named user fields and custom fields
        foreach (KeyValuePair<string, object> kvp in data) {
            if (NamedFields.Contains(value: kvp.Key) && kvp.Value != null) {
                namedFields.Add(kvp.Key, kvp.Value);
            } else if (!NamedFields.Contains(kvp.Key) && kvp.Value != null) {
                customFields.Add(kvp.Key, kvp.Value);
            }
        }

        // set user data
        if (namedFields.Count > 0) {
            FixSegmentKeysAndValues(namedFields);
            SetDataInternal(namedFields);
        }

        // set custom data
        if (customFields.Count > 0) {
            FixSegmentKeysAndValues(customFields);
            foreach (KeyValuePair<string, object> item in customFields) {
                CustomDataProperties[item.Key] = item.Value;
            }
        }
    }

    private void SetPropertyInternal(string key, object value)
    {
        if (!ValidateConsentAndKey(key, "SetPropertyInternal")) {
            return;
        }

        if (utils.IsNullEmptyOrWhitespace(key)) {
            Log.Warning($"[UserProfile] SetPropertyInternal, Provided key is null, empty or whitespace. Ignoring.");
            return;
        }

        Dictionary<string, object> data = new Dictionary<string, object> { { key, value } };
        Log.Info($"[UserProfile] SetPropertyInternal, key: [{key}] and value: [{value}]");
        SetPropertiesInternal(data);
    }

    private void SetOnceInternal(string key, string value)
    {
        if (!ValidateConsentAndKey(key, "SetOnceInternal")) {
            return;
        }

        Log.Info($"[UserProfile] SetOnceInternal, key: [{key}] and value: [{value}]");
        ModifyCustomData(key, value, "$setOnce");
    }
    #endregion
    #region Helper Methods
    void ModifyCustomData(string key, object value, string mod)
    {
        try {
            if (utils.IsNullEmptyOrWhitespace(key)) {
                Log.Warning($"[UserProfile] ModifyCustomData, Provided key is null, empty or whitespace. Will be ignored.");
                return;
            }

            if (value == null || !(value is double || value is int || value is string || value is bool)) {
                Log.Warning($"[UserProfile] ModifyCustomData, Provided 'value' is {(value == null ? "null" : "an unsupported type")}");
                return;
            }

            string trimmedKey = key.Trim();
            if (key != trimmedKey) {
                Log.Verbose($"[UserProfile] ModifyCustomData, Provided key: [{key}], for value: [{value}] has been trimmed.");
            }

            string truncatedKey = TrimKey(trimmedKey);
            object truncatedValue;

            if (value is string stringValue) {
                string trimmed = stringValue.Trim();
                if (stringValue != trimmed) {
                    Log.Verbose($"[UserProfile] ModifyCustomData, Provided value: [{value}], for key: [{key}] has been trimmed. Current value: [{trimmed}]");
                }
                truncatedValue = TrimValue(truncatedKey, trimmed);
            } else {
                truncatedValue = value;
            }

            CustomMods ??= new Dictionary<string, JObject>();

            JObject ob;
            if (mod != "$pull" && mod != "$push" && mod != "$addToSet") {
                ob = new JObject { [mod] = JToken.FromObject(truncatedValue) };
            } else {
                if (CustomMods.ContainsKey(truncatedKey)) {
                    ob = CustomMods[truncatedKey];
                } else {
                    ob = new JObject();
                }

                if (ob[mod] is JArray existingArray) {
                    existingArray.Add(JToken.FromObject(truncatedValue));
                } else {
                    ob[mod] = new JArray(truncatedValue);
                }
            }
            CustomMods[truncatedKey] = ob;
        } catch (Exception e) {
            Log.Error($"[UserProfile] ModifyCustomData, An exception occured during modifying the custom data. Exception: {e}");
        }
    }

    private void ClearInternal()
    {
        Log.Debug("[UserProfile] ClearInternal, Start");

        Name = null;
        Username = null;
        Email = null;
        Organization = null;
        Phone = null;
        PictureUrl = null;
        Gender = null;
        BirthYear = 0;
        CustomDataProperties = new Dictionary<string, object>();
        CustomMods = new Dictionary<string, JObject>();
    }

    private void AddStringPropertyToJSON(JObject json, string key, object value)
    {
        if (value != null && value is string val) {
            json.Add(key, val);
        }
        if (value != null && value is int v) {
            if (v > 0) {
                json.Add(BYEAR_KEY, v);
            }
        }
    }

    private string ConvertToJSON()
    {
        JObject json = new JObject();

        try {
            AddStringPropertyToJSON(json, NAME_KEY, Name);
            AddStringPropertyToJSON(json, USERNAME_KEY, Username);
            AddStringPropertyToJSON(json, EMAIL_KEY, Email);
            AddStringPropertyToJSON(json, ORG_KEY, Organization);
            AddStringPropertyToJSON(json, PHONE_KEY, Phone);
            AddStringPropertyToJSON(json, GENDER_KEY, Gender);
            AddStringPropertyToJSON(json, PICTURE_KEY, PictureUrl);
            AddStringPropertyToJSON(json, BYEAR_KEY, BirthYear);

            JObject ob = new JObject();

            if (CustomDataProperties.Count > 0) {
                utils.TruncateSegmentationValues(CustomDataProperties, config.GetMaxSegmentationValues(), "[UserProfile] ConvertToJSON", Log);
                ob = JObject.FromObject(CustomDataProperties);
            }

            if (CustomMods.Count > 0) {
                foreach (KeyValuePair<string, JObject> entry in CustomMods) {
                    ob[entry.Key] = entry.Value;
                }
            }

            if (ob != null && ob.Count > 0) {
                json.Add(CUSTOM_KEY, ob);
            }

        } catch (JsonException e) {
            Log.Warning($"[UserProfile] ConvertToJSON, Got exception converting an UserData to JSON. Exception:{e}");
        }

        if (json.Count > 0) {
            return json.ToString();
        } else {
            return null;
        }
    }

    private Dictionary<string, object> GetDataForRequest()
    {
        string json = ConvertToJSON();
        if (json != null) {
            try {
                Dictionary<string, object> data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                return data;
            } catch (JsonException ex) {
                Log.Error($"[UserProfile] GetDataForRequest, Failed to deserialize cached user data: {ex.Message}");
                return new Dictionary<string, object>();
            }
        }
        return new Dictionary<string, object>();
    }

    private bool ValidateConsentAndKey(string key, string caller = null)
    {
        if (!cly.IsSDKInitialized) {
            Log.Warning($"[UserProfile] {caller}, Countly.Instance.Init() must be called before first.");
            return false;
        }

        if (!_consentService.CheckConsentInternal(Consents.Users)) {
            Log.Debug($"[UserProfile] {caller}, Consent is not given, ignoring the request.");
            return false;
        }

        if (string.IsNullOrEmpty(key)) {
            Log.Warning($"[UserProfile] {caller}, Provided key isn't valid.");
            return false;
        }

        return true;
    }
    #endregion
    #region Override Methods
    internal override void OnInitializationCompleted()
    {
        if (config.GetUserProperties().Count() > 0) {
            LogSegmentation(config.GetUserProperties(), "[UserProfile] OnInitializationCompleted,");
            SetPropertiesInternal(config.GetUserProperties());
            SaveInternal();
        }
    }

    internal override void DeviceIdChanged(string deviceId, bool merged)
    {
        Log.Info($"[UserProfile] DeviceIdChanged, DeviceId change has occured. New DeviceId: [{deviceId}], Merged: [{merged}]. Calling Save");
        SaveInternal();
    }
    #endregion
}