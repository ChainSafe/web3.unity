using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using ChainSafe.Gaming.Marketplace.Models;
using ChainSafe.Gaming.Marketplace.Interfaces;

namespace ChainSafe.Gaming.Marketplace
{
    /// <summary>
    /// Auth system manager to help with refresh tokens.
    /// </summary>
    public class MarketplaceAuth : IMarketplaceAuth
    {
        #region Properties

        private static string BearerToken { get; set; }
        public static DateTime BearerTokenExpires { get; private set; }
        private static string RefreshToken { get; set; }
        private static DateTime RefreshTokenExpires { get; set; }

        #endregion

        /// <summary>
        /// Refreshes an expired bearer token.
        /// </summary>
        public async void RefreshExpiredToken()
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
                AuthSystemResponse.LoginResponse loginResponse =
                    JsonConvert.DeserializeObject<AuthSystemResponse.LoginResponse>(jsonResponse);
                BearerToken = loginResponse.access_token.token;
                BearerTokenExpires = DateTime.Parse(loginResponse.access_token.expires);
                RefreshToken = loginResponse.refresh_token.token;
                RefreshTokenExpires = DateTime.Parse(loginResponse.refresh_token.expires);
            }
        }
    }
}
