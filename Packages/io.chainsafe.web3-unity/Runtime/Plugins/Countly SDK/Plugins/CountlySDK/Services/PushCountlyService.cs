using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Notifications;
using Plugins.CountlySDK.Enums;
using Plugins.CountlySDK.Helpers;
using Plugins.CountlySDK.Models;

namespace Plugins.CountlySDK.Services
{
    public class PushCountlyService : AbstractBaseService
    {
        private string _token;
        private TestMode? _mode;
        private bool _isDeviceRegistered;
        private readonly RequestCountlyHelper _requestCountlyHelper;
        private readonly INotificationsService _notificationsService;
        private readonly NotificationsCallbackService _notificationsCallbackService;

        internal PushCountlyService(CountlyConfiguration configuration, CountlyLogHelper logHelper, RequestCountlyHelper requestCountlyHelper, INotificationsService notificationsService, NotificationsCallbackService notificationsCallbackService, ConsentCountlyService consentService) : base(configuration, logHelper, consentService)
        {
            Log.Debug("[PushCountlyService] Initializing.");
            if (configuration.NotificationEventListeners.Count > 0) {
                Log.Debug("[PushCountlyService] Registering " + configuration.NotificationEventListeners.Count + "  notification event listeners.");
            }

            _requestCountlyHelper = requestCountlyHelper;
            _notificationsService = notificationsService;
            _notificationsCallbackService = notificationsCallbackService;
        }

        /// <summary>
        /// A private method to register device for receiving Push Notifications.
        /// </summary>
        private void EnableNotification()
        {
            Log.Debug("[PushCountlyService] EnableNotification");

            //Enables push notification on start
            if (_configuration.EnableTestMode || !_consentService.CheckConsentInternal(Consents.Push) || _configuration.NotificationMode == TestMode.None) {
                return;
            }


            EnablePushNotificationAsync(_configuration.NotificationMode);
        }

        /// <summary>
        /// Registers device for receiving Push Notifications
        /// </summary>
        /// <param name="mode">Application mode</param>
        private void EnablePushNotificationAsync(TestMode mode)
        {
            Log.Debug("[PushCountlyService] EnablePushNotificationAsync : mode = " + mode);

            _mode = mode;
            _isDeviceRegistered = true;
            _notificationsService.GetToken(async result => {
                _token = result;
                /*
                 * When the push notification service gets enabled successfully for the device, 
                 * we send a request to the Countly server that the user is ready to receive push notifications.
               */
                await PostToCountlyAsync(_mode, _token);
                await ReportPushActionAsync();
            });

            _notificationsService.OnNotificationClicked(async (data, index) => {
                _notificationsCallbackService.NotifyOnNotificationClicked(data, index);
                await ReportPushActionAsync();
            });

            _notificationsService.OnNotificationReceived(data => {
                _notificationsCallbackService.NotifyOnNotificationReceived(data);
            });

        }

        /// <summary>
        /// Notifies Countly that the device is capable of receiving Push Notifications
        /// </summary>
        /// <returns></returns>
        private async Task PostToCountlyAsync(TestMode? mode, string token)
        {
            Log.Debug("[PushCountlyService] PostToCountlyAsync : token = " + token);

            if (!_mode.HasValue || !_consentService.CheckConsentInternal(Consents.Push)) {
                return;
            }

            Dictionary<string, object> requestParams =
                new Dictionary<string, object>
                {
                    { "token_session", 1 },
                    { "test_mode", (int)mode.Value },
                    { $"{_configuration.metricHelper.OS}_token", token },
                };

            _requestCountlyHelper.AddToRequestQueue(requestParams);
            await _requestCountlyHelper.ProcessQueue();
        }

        /// <summary>
        /// Report Push Actions stored in local cache to Countly server.,
        /// </summary>
        private async Task<CountlyResponse> ReportPushActionAsync()
        {
            Log.Debug("[PushCountlyService] ReportPushActionAsync");

            if (!_consentService.CheckConsentInternal(Consents.Push)) {
                return new CountlyResponse { IsSuccess = false };
            }


            return await _notificationsService.ReportPushActionAsync();
        }

        #region override Methods
        internal override void OnInitializationCompleted()
        {
            EnableNotification();
        }
        internal override void ConsentChanged(List<Consents> updatedConsents, bool newConsentValue, ConsentChangedAction action)
        {
            if (updatedConsents.Contains(Consents.Push) && newConsentValue && !_isDeviceRegistered) {
                EnableNotification();
            }
        }
        #endregion
    }
}
