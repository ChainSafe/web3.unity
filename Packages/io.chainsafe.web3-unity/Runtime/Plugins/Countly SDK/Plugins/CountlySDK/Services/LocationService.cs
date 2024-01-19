using System.Collections.Generic;
using System.Threading.Tasks;
using Plugins.CountlySDK.Enums;
using Plugins.CountlySDK.Helpers;
using Plugins.CountlySDK.Models;
using UnityEngine;

namespace Plugins.CountlySDK.Services
{
    public class LocationService : AbstractBaseService
    {
        internal string City { get; private set; }
        internal string Location { get; private set; }
        internal string IPAddress { get; private set; }
        internal string CountryCode { get; private set; }
        internal bool IsLocationDisabled { get; private set; }

        private readonly RequestCountlyHelper _requestCountlyHelper;

        internal LocationService(CountlyConfiguration configuration, CountlyLogHelper logHelper, RequestCountlyHelper requestCountlyHelper, ConsentCountlyService consentService) : base(configuration, logHelper, consentService)
        {
            Log.Debug("[LocationService] Initializing.");
            if (configuration.IsLocationDisabled) {
                Log.Debug("[LocationService] Disabling location");
            }

            if (configuration.CountryCode != null || configuration.City != null || configuration.Location != null || configuration.IPAddress != null) {
                Log.Debug("[LocationService] location: countryCode = [" + configuration.CountryCode + "], city = [" + configuration.City + "], gpsCoordinates = [" + configuration.Location + ",] ipAddress = [" + configuration.IPAddress + "]");
            }

            _requestCountlyHelper = requestCountlyHelper;
            IsLocationDisabled = configuration.IsLocationDisabled;

            if (IsLocationDisabled || !_consentService.CheckConsentInternal(Consents.Location)) {
                City = null;
                Location = null;
                IPAddress = null;
                CountryCode = null;
            } else {
                City = configuration.City;
                Location = configuration.Location;
                IPAddress = configuration.IPAddress;
                CountryCode = configuration.CountryCode;
            }
        }

        /// <summary>
        /// Sends a request with an empty "location" parameter.
        /// </summary>
        internal async Task SendRequestWithEmptyLocation()
        {
            Dictionary<string, object> requestParams =
               new Dictionary<string, object> {
                   { "location", string.Empty }
               };

            _requestCountlyHelper.AddToRequestQueue(requestParams);
            await _requestCountlyHelper.ProcessQueue();
        }

        /// <summary>
        /// An internal function to add user's location request into request queue.
        /// </summary>
        internal async Task SendIndependantLocationRequest()
        {
            Log.Debug("[LocationService] SendIndependantLocationRequest");

            if (!_consentService.CheckConsentInternal(Consents.Location)) {
                return;
            }

            Dictionary<string, object> requestParams =
                new Dictionary<string, object>();

            /*
             * Empty country code, city and IP address can not be sent.
             */

            if (!string.IsNullOrEmpty(IPAddress)) {
                requestParams.Add("ip_address", IPAddress);
            }

            if (!string.IsNullOrEmpty(CountryCode)) {
                requestParams.Add("country_code", CountryCode);
            }

            if (!string.IsNullOrEmpty(City)) {
                requestParams.Add("city", City);
            }

            if (!string.IsNullOrEmpty(Location)) {
                requestParams.Add("location", Location);
            }

            if (requestParams.Count > 0) {
                _requestCountlyHelper.AddToRequestQueue(requestParams);
                await _requestCountlyHelper.ProcessQueue();
            }
        }



        /// <summary>
        /// Disabled the location tracking on the Countly server
        /// </summary>
        public async void DisableLocation()
        {
            lock (LockObj) {
                Log.Info("[LocationService] DisableLocation");

                if (!_consentService.CheckConsentInternal(Consents.Location)) {
                    return;
                }

                IsLocationDisabled = true;
                City = null;
                Location = null;
                IPAddress = null;
                CountryCode = null;

                /*
                 *If the location feature gets disabled or location consent is removed,
                 *the SDK sends a request with an empty "location". 
                 */

                _ = SendRequestWithEmptyLocation();
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// Set Country code (ISO Country code), City, Location and IP address to be used for future requests.
        /// </summary>
        /// <param name="countryCode">ISO Country code for the user's country</param>
        /// <param name="city">Name of the user's city</param>
        /// <param name="gpsCoordinates">comma separate lat and lng values. For example, "56.42345,123.45325"</param>
        /// <param name="ipAddress">ipAddress like "192.168.88.33"</param>
        /// <returns></returns>
        public async void SetLocation(string countryCode, string city, string gpsCoordinates, string ipAddress)
        {
            lock (LockObj) {
                Log.Info("[LocationService] SetLocation : countryCode = " + countryCode + ", city = " + city + ", gpsCoordinates = " + gpsCoordinates + ", ipAddress = " + ipAddress);

                if (!_consentService.CheckConsentInternal(Consents.Location)) {
                    return;
                }

                /*If city is not paired together with country,
                 * a warning should be printed that they should be set together.
                 */
                if ((!string.IsNullOrEmpty(CountryCode) && string.IsNullOrEmpty(City))
                    || (!string.IsNullOrEmpty(City) && string.IsNullOrEmpty(CountryCode))) {
                    Log.Warning("[LocationService] In \"SetLocation\" both country code and city should be set together");
                }

                City = city;
                IPAddress = ipAddress;
                CountryCode = countryCode;
                Location = gpsCoordinates;

                /*
                 * If location consent is given and location gets re-enabled (previously was disabled), 
                 * we send that set location information in a separate request and save it in the internal location cache.
                 */
                if (countryCode != null || city != null || gpsCoordinates != null || ipAddress != null) {
                    IsLocationDisabled = false;
                    _ = SendIndependantLocationRequest();
                }
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// If location consent is removed, the SDK sends a request with an empty "location" parameter.
        /// </summary>
        private async void OnLocationConsentRemoved()
        {
            City = null;
            Location = null;
            IPAddress = null;
            CountryCode = null;

            await SendRequestWithEmptyLocation();
        }

        #region override Methods
        internal override void ConsentChanged(List<Consents> updatedConsents, bool newConsentValue, ConsentChangedAction action)
        {
            if (action != ConsentChangedAction.DeviceIDChangedNotMerged && updatedConsents.Contains(Consents.Location) && !newConsentValue) {
                OnLocationConsentRemoved();
            }

        }
        #endregion
    }
}
