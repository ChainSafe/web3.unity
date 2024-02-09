using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.WalletConnect.Models;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Environment;
using WalletConnectSharp.Common.Logging;

namespace ChainSafe.Gaming.WalletConnect.Wallets
{
    /// <summary>
    /// The registry of the wallets supported by WalletConnect.
    /// </summary>
    public class WalletRegistry : ILifecycleParticipant, IWalletRegistry // todo provide pre-downloaded registry with option to update
    {
        private readonly IHttpClient httpClient;
        private readonly IWalletConnectConfig config;

        private List<WalletModel> enabledWallets;

        public WalletRegistry(IHttpClient httpClient, IWalletConnectConfig config)
        {
            this.config = config;
            this.httpClient = httpClient;
        }

        public static string BuildRegistryUri(string projectId) =>
            $"https://explorer-api.walletconnect.com/v3/wallets?projectId={projectId}";

        async ValueTask ILifecycleParticipant.WillStartAsync()
        {
            var registryUri = !string.IsNullOrWhiteSpace(config.OverrideRegistryUri)
                ? config.OverrideRegistryUri
                : BuildRegistryUri(config.ProjectId);

            var response = await httpClient.Get<WalletRegistryResponse>(registryUri);
            var allWallets = response.AssertSuccess().Listings;

            if (config.EnabledWallets != null && config.EnabledWallets.Any())
            {
                enabledWallets = allWallets.Values
                    .Where(w => config.EnabledWallets.Contains(w.Name))
                    .ToList();

                return;
            }

            if (config.DisabledWallets != null && config.DisabledWallets.Any())
            {
                enabledWallets = allWallets.Values
                    .Where(w => !config.DisabledWallets.Contains(w.Name))
                    .ToList();

                return;
            }

            enabledWallets = allWallets.Values.ToList();
        }

        ValueTask ILifecycleParticipant.WillStopAsync() => new(Task.CompletedTask);

        public WalletModel GetWallet(string name) // todo refactor this class to use walletId for identification
        {
            return enabledWallets.Find(w => w.Name == name);
        }

        public IEnumerable<WalletModel> EnumerateSupportedWallets(Platform platform)
        {
            return enabledWallets.Where(w => IsAvailableForPlatform(w, platform));
        }

        /// <summary>
        /// Is a wallet available for <see cref="Platform"/>.
        /// </summary>
        /// <param name="platform">Platform to wallet app availability.</param>
        /// <returns>True if wallet is available for <see cref="Platform"/>.</returns>
        private bool IsAvailableForPlatform(WalletModel wallet, Platform platform)
        {
            switch (platform)
            {
                case Platform.Editor:
                case Platform.Desktop:
                    return CanUseNativeProtocol(false) || !string.IsNullOrEmpty(wallet.Desktop.UniversalUrl);

                case Platform.Android:
                case Platform.IOS:
                    return CanUseNativeProtocol(true) || !string.IsNullOrEmpty(wallet.Mobile.UniversalUrl);

                // currently Wallet Connect doesn't support WebGL.
                default:
                    WCLogger.Log($"Unsupported Platform {platform}");
                    return false;
            }

            bool CanUseNativeProtocol(bool isMobilePlatform)
            {
                var nativeUrl = GetLinkData(isMobilePlatform).NativeProtocol;
                return !string.IsNullOrWhiteSpace(nativeUrl) && nativeUrl != ":";
            }

            WalletLinkModel GetLinkData(bool isMobilePlatform)
            {
                return isMobilePlatform ? wallet.Mobile : wallet.Desktop;
            }
        }
    }
}