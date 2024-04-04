using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using Plugins.CountlySDK.Enums;
using Plugins.CountlySDK.Helpers;
using Plugins.CountlySDK.Models;
using Plugins.CountlySDK.Services;
using UnityEngine;

namespace Notifications.Impls
{
    public class ProxyNotificationsService : INotificationsService
    {
        private readonly CountlyLogHelper _logHelper;
        private readonly Transform _countlyGameObject;
        private readonly INotificationsService _service;
        public bool IsInitializedWithoutError { get; set; }

        internal ProxyNotificationsService(Transform countlyGameObject, CountlyConfiguration config, CountlyLogHelper logHelper, Action<IEnumerator> startCoroutine, EventCountlyService eventCountlyService)
        {
            _logHelper = logHelper;
            _logHelper.Debug("[ProxyNotificationsService] Initializing.");

            _countlyGameObject = countlyGameObject;

            if (config.NotificationMode == TestMode.None) {
                return;
            }

#if UNITY_ANDROID
            _service = new Notifications.Impls.Android.AndroidNotificationsService(_countlyGameObject, config, logHelper, eventCountlyService);
#elif UNITY_IOS
            _service = new Notifications.Impls.iOs.IOsNotificationsService(_countlyGameObject, config, logHelper, startCoroutine, eventCountlyService);
#endif
            IsInitializedWithoutError = true;
            if (_service != null && !_service.IsInitializedWithoutError) {
                _service = null;
                IsInitializedWithoutError = false;
            }
        }

        public void GetToken(Action<string> result)
        {
            _logHelper.Verbose("[ProxyNotificationsService] GetToken");

            if (_service != null) {
                _service.GetToken(result);
            }

        }

        public void OnNotificationClicked(Action<string, int> result)
        {
            _logHelper.Verbose("[ProxyNotificationsService] OnNotificationClicked");

            if (_service != null) {
                _service.OnNotificationClicked(result);
            }
        }


        public void OnNotificationReceived(Action<string> result)
        {
            _logHelper.Verbose("[ProxyNotificationsService] OnNotificationReceived");

            if (_service != null) {
                _service.OnNotificationReceived(result);
            }
        }

        public async Task<CountlyResponse> ReportPushActionAsync()
        {
            _logHelper.Verbose("[ProxyNotificationsService] ReportPushActionAsync");

            if (_service != null) {
                return await _service.ReportPushActionAsync();
            }

            return new CountlyResponse {
                IsSuccess = true,
            };
        }
    }
}