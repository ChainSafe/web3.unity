using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugins.CountlySDK.Enums;
using Plugins.CountlySDK.Helpers;
using Plugins.CountlySDK.Models;
using Plugins.CountlySDK.Persistance.Entities;
using Plugins.iBoxDB;
using UnityEngine;
using UnityEngine.Networking;

namespace Plugins.CountlySDK.Services
{
    public class RemoteConfigCountlyService : AbstractBaseService
    {
        private readonly CountlyUtils _countlyUtils;
        private readonly Dao<ConfigEntity> _configDao;
        private readonly RequestBuilder _requestBuilder;
        private readonly RequestCountlyHelper _requestCountlyHelper;

        /// <summary>
        /// Get the remote config values.
        /// </summary>
        public Dictionary<string, object> Configs { private set; get; }

        private readonly StringBuilder _requestStringBuilder = new StringBuilder();

        internal RemoteConfigCountlyService(CountlyConfiguration configuration, CountlyLogHelper logHelper, RequestCountlyHelper requestCountlyHelper, CountlyUtils countlyUtils, Dao<ConfigEntity> configDao, ConsentCountlyService consentService, RequestBuilder requestBuilder) : base(configuration, logHelper, consentService)
        {
            Log.Debug("[RemoteConfigCountlyService] Initializing.");

            _configDao = configDao;
            _countlyUtils = countlyUtils;
            _requestBuilder = requestBuilder;
            _requestCountlyHelper = requestCountlyHelper;

            if (_consentService.CheckConsentInternal(Consents.RemoteConfig)) {
                Configs = FetchConfigFromDB();
            } else {
                _configDao.RemoveAll();
            }

        }

        /// <summary>
        /// Fetch fresh remote config values from server and initialize <code>Configs</code>
        /// </summary>
        internal async Task<CountlyResponse> InitConfig()
        {
            Log.Debug("[RemoteConfigCountlyService] InitConfig");

            if (_configuration.EnableTestMode) {
                return new CountlyResponse { IsSuccess = true };
            }

            return await Update();
        }

        /// <summary>
        /// Fetch locally stored remote config values.
        /// </summary>
        /// <returns>Stored Remote config</returns>
        private Dictionary<string, object> FetchConfigFromDB()
        {
            Dictionary<string, object> config = null;
            List<ConfigEntity> allConfigs = _configDao.LoadAll();
            if (allConfigs != null && allConfigs.Count > 0) {
                config = Converter.ConvertJsonToDictionary(allConfigs[0].Json, Log);
            }

            Log.Debug("[RemoteConfigCountlyService] FetchConfigFromDB : Configs = " + config);

            return config;
        }

        /// <summary>
        /// Fetch fresh remote config values from server and store locally.
        /// </summary>
        /// <returns></returns>
        public async Task<CountlyResponse> Update()
        {
            Log.Info("[RemoteConfigCountlyService] Update");

            if (!_consentService.CheckConsentInternal(Consents.RemoteConfig)) {
                return new CountlyResponse {
                    IsSuccess = false
                };
            }

            Dictionary<string, object> requestParams = _countlyUtils.GetBaseParams();

            requestParams.Add("method", "fetch_remote_config");

            string metricsJSON = _configuration.metricHelper.buildMetricJSON();
            requestParams.Add("metrics", metricsJSON);

            string data = _requestBuilder.BuildQueryString(requestParams);

            CountlyResponse response;
            if (_configuration.EnablePost) {
                response = await Task.Run(() => _requestCountlyHelper.PostAsync(_countlyUtils.ServerInputUrl, data));
            } else {
                response = await Task.Run(() => _requestCountlyHelper.GetAsync(_countlyUtils.ServerInputUrl, data));

            }
            if (response.IsSuccess) {
                _configDao.RemoveAll();
                ConfigEntity configEntity = new ConfigEntity {
                    Id = _configDao.GenerateNewId(),
                    Json = response.Data
                };
                _configDao.Save(configEntity);
                Configs = Converter.ConvertJsonToDictionary(response.Data, Log);

                Log.Debug("[RemoteConfigCountlyService] UpdateConfig: " + response.ToString());

            }

            return response;
        }

        #region override Methods
        internal override void ConsentChanged(List<Consents> updatedConsents, bool newConsentValue, ConsentChangedAction action)
        {
            if (updatedConsents.Contains(Consents.RemoteConfig) && !newConsentValue) {
                Configs = null;
                _configDao.RemoveAll();
            }
        }
        #endregion
    }
}
