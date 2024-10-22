using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Reown.Models;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Environment;
using ChainSafe.Gaming.Web3.Environment.Http;

namespace ChainSafe.Gaming.Reown.Wallets
{
    /// <summary>
    /// The registry of the wallets supported by Reown.
    /// </summary>
    public class ReownWalletRegistry : ILifecycleParticipant, IWalletRegistry // todo provide pre-downloaded registry with option to update
    {
        private const string Host = "https://api.web3modal.com";

        private readonly IHttpClient reownHttpClient;
        private readonly IReownConfig config;
        private readonly IOperatingSystemMediator systemMediator;

        private List<WalletModel> platformWallets;

        public ReownWalletRegistry(ReownHttpClient reownHttpClient, IReownConfig config, IOperatingSystemMediator systemMediator)
        {
            this.systemMediator = systemMediator;
            this.config = config;
            this.reownHttpClient = reownHttpClient;
        }

        public static string RegistryUri => $"{Host}/getWallets";

        public IEnumerable<WalletModel> SupportedWallets => platformWallets.AsReadOnly();

        async ValueTask ILifecycleParticipant.WillStartAsync()
        {
            var registryUri = !string.IsNullOrWhiteSpace(config.OverrideRegistryUri)
                ? config.OverrideRegistryUri
                : RegistryUri;

            var parameters = new Dictionary<string, string>()
            {
                { "page", 1.ToString() },
                { "entries", 100.ToString() }, // get as many wallets as we can with one api call (when too many wallets returns 400)
                { "search", null },
                { "platform", GetPlatformFilter() },
                { "include", BuildWalletIdsFilter(config.IncludeWalletIds) },
                { "exclude", BuildWalletIdsFilter(config.ExcludeWalletIds) },
            };

            var parametersRaw = HttpUtils.BuildUriParameters(parameters);

            var response = await reownHttpClient.Get<WalletRegistryResponse>(registryUri + parametersRaw); // todo download all the available wallets in a loop
            var apiFilteredWallets = platformWallets = response.AssertSuccess().Data;
            platformWallets = apiFilteredWallets
                .Where(w =>
                {
                    switch (systemMediator.Platform)
                    {
                        case Platform.Editor:
                        case Platform.Desktop:
                            return !string.IsNullOrWhiteSpace(w.DesktopLink);
                        case Platform.IOS:
                        case Platform.Android:
                            return !string.IsNullOrWhiteSpace(w.MobileLink);
                        case Platform.WebGL:
                        default:
                            return true;
                    }
                })
                .ToList();

            /* todo implement and utilize IsWalletInstalled functionality
             *
                var walletClosure = wallet;
                var isWalletInstalled = WalletUtils.IsWalletInstalled(wallet);
             */

            return;

            string GetPlatformFilter()
            {
                return systemMediator.Platform switch
                {
                    Platform.Android => "android",
                    Platform.IOS => "ios",
                    _ => null
                };
            }

            string BuildWalletIdsFilter(IList<string> walletIds)
            {
                return walletIds is { Count: > 0 }
                    ? string.Join(",", walletIds)
                    : null;
            }
        }

        ValueTask ILifecycleParticipant.WillStopAsync() => new(Task.CompletedTask);

        public WalletModel GetWallet(string id)
        {
            return platformWallets.Find(w => w.Id == id);
        }
    }
}