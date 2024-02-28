using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plugins.CountlySDK.Enums;
using Plugins.CountlySDK.Helpers;
using Plugins.CountlySDK.Models;
using UnityEngine;

namespace Plugins.CountlySDK.Services
{
    public class DeviceIdCountlyService : AbstractBaseService
    {
        private readonly CountlyUtils _countlyUtils;
        private readonly EventCountlyService _eventCountlyService;
        internal readonly RequestCountlyHelper _requestCountlyHelper;
        private readonly SessionCountlyService _sessionCountlyService;

        private readonly int DEVICE_TYPE_FALLBACK_VALUE = -1;


        internal DeviceIdCountlyService(CountlyConfiguration configuration, CountlyLogHelper logHelper, SessionCountlyService sessionCountlyService,
            RequestCountlyHelper requestCountlyHelper, EventCountlyService eventCountlyService, CountlyUtils countlyUtils, ConsentCountlyService consentService) : base(configuration, logHelper, consentService)
        {
            Log.Debug("[DeviceIdCountlyService] Initializing.");

            _countlyUtils = countlyUtils;
            _eventCountlyService = eventCountlyService;
            _requestCountlyHelper = requestCountlyHelper;
            _sessionCountlyService = sessionCountlyService;
        }
        /// <summary>
        /// Returns the Device ID that is currently used by the SDK
        /// </summary>
        public string DeviceId { get; private set; }

        /// <summary>
        /// Returns the type Device ID that is currently used by the SDK
        /// </summary>
        public DeviceIdType DeviceIdType { get; private set; }

        /// <summary>
        /// Initialize <code>DeviceId</code> field with device id provided in configuration or with Random generated Id and Cache it.
        /// </summary>
        /// <param name="deviceId">new device id provided in configuration</param>
        internal void InitDeviceId(string deviceId = null)
        {
            //**Priority is**
            //Cached DeviceID (remains even after app kill)
            //Static DeviceID (only when the app is running or in the background)
            //User provided DeviceID
            //Generate Random DeviceID
            string storedDeviceId = PlayerPrefs.GetString(Constants.DeviceIDKey);
            if (!_countlyUtils.IsNullEmptyOrWhitespace(storedDeviceId))
            {
                //SDK already has a device id

                //assign locally stored device id
                DeviceId = storedDeviceId;

                //Checking if device id type stored locally
                int storedDIDType = PlayerPrefs.GetInt(Constants.DeviceIDTypeKey, DEVICE_TYPE_FALLBACK_VALUE);

                //checking if we valid device id type
                if (storedDIDType == (int)DeviceIdType.SDKGenerated || storedDIDType == (int)DeviceIdType.DeveloperProvided)
                {
                    //SDK has a valid device id type in storage. SDK will be using it.
                    DeviceIdType = (DeviceIdType)storedDIDType;
                }
                else
                {
                    if (storedDIDType == DEVICE_TYPE_FALLBACK_VALUE)
                    {
                        Log.Error("[DeviceIdCountlyService] InitDeviceId: SDK doesn't have device ID type stored. There should have been one.");
                    }
                    else
                    {
                        Log.Error("[DeviceIdCountlyService] InitDeviceId: The stored device id type wasn't valid ['" + storedDeviceId + "']. SDK will assign a new type");
                    }

                    if (_countlyUtils.IsNullEmptyOrWhitespace(deviceId))
                    {
                        UpdateDeviceIdAndDeviceIdType(DeviceId, DeviceIdType.SDKGenerated);

                    }
                    else
                    {
                        UpdateDeviceIdAndDeviceIdType(DeviceId, DeviceIdType.DeveloperProvided);
                    }
                }
            }
            else
            {
                //SDK doesn't have a device id stored locally

                //checking if developer provided device id is null or empty.
                if (_countlyUtils.IsNullEmptyOrWhitespace(deviceId))
                {
                    UpdateDeviceIdAndDeviceIdType(CountlyUtils.GetUniqueDeviceId(), DeviceIdType.SDKGenerated);
                }
                else
                {
                    UpdateDeviceIdAndDeviceIdType(deviceId, DeviceIdType.DeveloperProvided);
                }
            }
        }
        /// <summary>
        /// Changes Device Id.
        /// Adds currently recorded but not queued events to request queue.
        /// Clears all started timed-events
        /// Ends current session with old Device Id.
        /// Begins a new session with new Device Id
        /// </summary>
        /// <param name="deviceId">new device id</param>
        public async Task ChangeDeviceIdWithoutMerge(string deviceId)
        {
            lock (LockObj)
            {
                Log.Info("[DeviceIdCountlyService] ChangeDeviceIdWithoutMerge: deviceId = " + deviceId);

                //Ignore call if new and old device id are same
                if (DeviceId == deviceId)
                {
                    return;
                }

                //Add currently recorded events to request queue
                _eventCountlyService.AddEventsToRequestQueue();

                //Cancel all timed events
                _eventCountlyService.CancelAllTimedEvents();

                //Ends current session
                //Do not dispose timer object
                if (!_configuration.IsAutomaticSessionTrackingDisabled)
                {
                    _ = _sessionCountlyService.EndSessionAsync();
                }

                //Update device id
                UpdateDeviceIdAndDeviceIdType(deviceId, DeviceIdType.DeveloperProvided);

                if (_consentService.RequiresConsent)
                {
                    _consentService.SetConsentInternal(_consentService.CountlyConsents.Keys.ToArray(), false, sendRequest: false, ConsentChangedAction.DeviceIDChangedNotMerged);
                }

                //Begin new session with new device id
                //Do not initiate timer again, it is already initiated
                if (!_configuration.IsAutomaticSessionTrackingDisabled)
                {
                    _ = _sessionCountlyService.BeginSessionAsync();
                }

                NotifyListeners(false);

                _ = _requestCountlyHelper.ProcessQueue();
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// Changes DeviceId.
        /// Continues with the current session.
        /// Merges data for old and new Device Id.
        /// </summary>
        /// <param name="deviceId">new device id</param>
        public async Task ChangeDeviceIdWithMerge(string deviceId)
        {
            lock (LockObj)
            {
                Log.Info("[DeviceIdCountlyService] ChangeDeviceIdWithMerge: deviceId = " + deviceId);

                //Ignore call if new and old device id are same
                if (DeviceId == deviceId)
                {
                    return;
                }

                //Keep old device id
                string oldDeviceId = DeviceId;

                //Update device id
                UpdateDeviceIdAndDeviceIdType(deviceId, DeviceIdType.DeveloperProvided);

                //Merge user data for old and new device
                Dictionary<string, object> requestParams =
                    new Dictionary<string, object> { { "old_device_id", oldDeviceId } };

                _requestCountlyHelper.AddToRequestQueue(requestParams);
                _ = _requestCountlyHelper.ProcessQueue();
                NotifyListeners(true);
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// Updates Device ID both in app and in cache
        /// </summary>
        /// <param name="newDeviceId">new device id</param>
        /// <param name="type">device id type</param>
        private void UpdateDeviceIdAndDeviceIdType(string newDeviceId, DeviceIdType type)
        {
            //Change device id and type
            DeviceId = newDeviceId;
            DeviceIdType = type;

            //Updating Cache
            PlayerPrefs.SetString(Constants.DeviceIDKey, DeviceId);
            PlayerPrefs.SetInt(Constants.DeviceIDTypeKey, (int)DeviceIdType);

            Log.Debug("[DeviceIdCountlyService] UpdateDeviceId: " + newDeviceId);
        }

        /// <summary>
        /// Call <code>DeviceIdChanged</code> on all listeners.
        /// </summary>
        /// <param name="merged">If passed "true" if will perform a device ID merge server side of the old and new device ID. This will merge their data</param>
        private void NotifyListeners(bool merged)
        {
            if (Listeners == null)
            {
                return;
            }

            foreach (AbstractBaseService listener in Listeners)
            {
                listener.DeviceIdChanged(DeviceId, merged);
            }
        }

        #region override Methods
        #endregion
    }
}
