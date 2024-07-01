using System;
using System.Collections;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using ChainSafe.Gaming.Marketplace.Models;

namespace ChainSafe.Gaming.Marketplace
{
    /// <summary>
    /// Manages marketplace auth config.
    /// </summary>
    public class MarketplaceAuth : MonoBehaviour, IMarketplaceAuth
    {
        #region Properties

        public string BearerToken { get; private set; }
        public DateTime BearerTokenExpires { get; private set; }
        public string RefreshToken { get; private set; }
        public DateTime RefreshTokenExpires { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes class properties.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="marketplaceAuthSystemManagerConfigEventArgs">Property arguments.</param>
        public void InitializeConfig(object sender, EventManagerMarketplace.MarketplaceAuthSystemConfigEventArgs marketplaceAuthSystemManagerConfigEventArgs)
        {
            BearerToken = marketplaceAuthSystemManagerConfigEventArgs.BearerToken;
            BearerTokenExpires = marketplaceAuthSystemManagerConfigEventArgs.BearerTokenExpires;
            RefreshToken = marketplaceAuthSystemManagerConfigEventArgs.RefreshToken;
            RefreshTokenExpires = marketplaceAuthSystemManagerConfigEventArgs.RefreshTokenExpires;
            StartCoroutine(WaitForTokenExpiration());
        }
        
        /// <summary>
        /// Timer that wait for token expiration to execute refresh.
        /// </summary>
        private IEnumerator WaitForTokenExpiration()
        {
            DateTime currentTime = DateTime.UtcNow;
            TimeSpan timeToWait = BearerTokenExpires - currentTime;
            yield return new WaitForSeconds((float)timeToWait.TotalSeconds);
            Debug.Log("Refreshing Token, timer expired");
            RefreshExpiredToken();
        }
        
        /// <summary>
        /// Refreshes an expired bearer token.
        /// </summary>
        private async void RefreshExpiredToken()
        {
            if (RefreshToken == null)
            {
                throw new Exception("Refresh token not set");
            }

            var payload = new AuthPayload.RefreshPayload()
            {
                refresh = RefreshToken
            };
            var jsonPayload = JsonConvert.SerializeObject(payload);
            var request = new UnityWebRequest("https://api.chainsafe.io/api/v1/user/refresh", "POST");
            var bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            await request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error: {request.error}");
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                AuthSystemResponse.LoginResponse loginResponse = JsonConvert.DeserializeObject<AuthSystemResponse.LoginResponse>(jsonResponse);
                BearerToken = loginResponse.access_token.token;
                BearerTokenExpires = DateTime.Parse(loginResponse.access_token.expires);
                RefreshToken = loginResponse.refresh_token.token;
                RefreshTokenExpires = DateTime.Parse(loginResponse.refresh_token.expires);
            }
        }

        private void OnEnable()
        {
            EventManagerMarketplace.ConfigureAuthSystemManager += InitializeConfig;
        }

        private void OnDisable()
        {
            EventManagerMarketplace.ConfigureAuthSystemManager -= InitializeConfig;
        }

        #endregion
    }
}