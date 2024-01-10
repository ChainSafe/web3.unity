using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.WalletConnect.Models;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Environment;

namespace ChainSafe.Gaming.WalletConnect
{
    public class WalletRegistry : ILifecycleParticipant, IWalletRegistry
    {
        public const string DefaultRegistryUri = "https://registry.walletconnect.com/data/wallets.json";

        private readonly IHttpClient httpClient;
        private readonly IWalletConnectConfigNew config;

        private List<WalletConnectWalletModel> enabledWallets;

        public WalletRegistry(IHttpClient httpClient, IWalletConnectConfigNew config)
        {
            this.config = config;
            this.httpClient = httpClient;
        }

        async ValueTask ILifecycleParticipant.WillStartAsync()
        {
            var registryUri = !string.IsNullOrWhiteSpace(config.OverrideRegistryUri)
                ? config.OverrideRegistryUri
                : DefaultRegistryUri;

            var response = await httpClient.Get<Dictionary<string, WalletConnectWalletModel>>(registryUri);
            var allWallets = response.AssertSuccess();

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

        public WalletConnectWalletModel GetWallet(string name)
        {
            return enabledWallets.Find(w => w.Name == name);
        }

        public IEnumerable<WalletConnectWalletModel> EnumerateSupportedWallets(Platform platform)
        {
            return enabledWallets.Where(w => w.IsAvailableForPlatform(platform));
        }
    }
}