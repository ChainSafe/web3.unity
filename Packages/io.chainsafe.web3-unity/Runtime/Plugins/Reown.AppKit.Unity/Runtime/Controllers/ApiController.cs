using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Networking;
using Reown.AppKit.Unity.Http;
using Reown.AppKit.Unity.Model;

namespace Reown.AppKit.Unity
{
    public class ApiController
    {
        private const string BasePath = "https://api.web3modal.com/";
        private const int TimoutSeconds = 5;

        private readonly string _includedWalletIdsString = AppKit.Config.includedWalletIds is { Length: > 0 }
            ? string.Join(",", AppKit.Config.includedWalletIds)
            : null;

        private readonly string _excludedWalletIdsString = AppKit.Config.excludedWalletIds is { Length: > 0 }
            ? string.Join(",", AppKit.Config.excludedWalletIds)
            : null;

        private readonly UnityHttpClient _httpClient = new(new Uri(BasePath), TimeSpan.FromSeconds(TimoutSeconds),
            new AppKitApiHeaderDecorator()
        );

        private const string Platform =
#if UNITY_ANDROID
            "android";
#elif UNITY_IOS
            "ios";
#else
            null;
#endif

        public async Task<GetWalletsResponse> GetWallets(int page, int count, string search = null)
        {
            if (page < 1)
                throw new ArgumentOutOfRangeException(nameof(page), "Page must be greater than 0");

            if (count < 1)
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than 0");


            return await _httpClient.GetAsync<GetWalletsResponse>("getWallets", new Dictionary<string, string>()
            {
                { "page", page.ToString() },
                { "entries", count.ToString() },
                { "search", search },
                { "platform", Platform },
                { "include", _includedWalletIdsString },
                { "exclude", _excludedWalletIdsString }
            });
        }

        public async Task<ApiGetAnalyticsConfigResponse> GetAnalyticsConfigAsync()
        {
            return await _httpClient.GetAsync<ApiGetAnalyticsConfigResponse>("getAnalyticsConfig");
        }
    }

    public class ApiGetAnalyticsConfigResponse
    {
        public bool isAnalyticsEnabled { get; set; }
        public bool isAppKitAuthEnabled { get; set; }
    }
}