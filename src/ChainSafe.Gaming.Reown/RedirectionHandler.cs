using System;
using ChainSafe.Gaming.Reown.Models;
using ChainSafe.Gaming.Reown.Wallets;
using ChainSafe.Gaming.Web3.Environment;

namespace ChainSafe.Gaming.Reown
{
    /// <summary>
    /// Component responsible for redirection to a wallet app.
    /// </summary>
    public class RedirectionHandler
    {
        private readonly IWalletRegistry walletRegistry;
        private readonly IOperatingSystemMediator osMediator;
        private readonly ILogWriter logWriter;

        public RedirectionHandler(IWalletRegistry walletRegistry, IOperatingSystemMediator osMediator, ILogWriter logWriter)
        {
            this.logWriter = logWriter;
            this.osMediator = osMediator;
            this.walletRegistry = walletRegistry;
        }

        /// <summary>
        /// Redirect for connection using the pre-selected wallet.
        /// </summary>
        /// <param name="connectionUri">The connection URI provided by Reown.</param>
        /// <param name="walletId">The name of the wallet to redirect user to.</param>
        public void RedirectConnection(string connectionUri, string walletId)
        {
            var walletData = walletRegistry.GetWallet(walletId);
            var deeplink = BuildConnectionDeeplink(walletData, connectionUri);

            logWriter.Log($"Generated deep link: {deeplink}");

            if (osMediator.IsMobilePlatform && osMediator.IsEditor)
            {
                logWriter.LogError("Can't open mobile deeplink in editor. Ignoring...");
                return;
            }

            osMediator.OpenUrl(deeplink);
        }

        /// <summary>
        /// Delegate redirection for connection to the OS.
        /// </summary>
        /// <param name="connectionUri">The connection URI provided by Reown.</param>
        public void RedirectConnectionOsManaged(string connectionUri)
        {
            if (osMediator.IsMobilePlatform && osMediator.IsEditor)
            {
                logWriter.LogError($"Can't open mobile connection URI in editor. Ignoring... Connection URI: {connectionUri}");
                return;
            }

            logWriter.Log($"Connection URI: {connectionUri}");

            osMediator.OpenUrl(connectionUri);
        }

        /// <summary>
        /// Redirect to the pre-selected wallet using the wallet name.
        /// </summary>
        public void Redirect(string walletId)
        {
            var walletData = walletRegistry.GetWallet(walletId);
            Redirect(walletData);
        }

        /// <summary>
        /// Redirect to the pre-selected wallet.
        /// </summary>
        public void Redirect(WalletModel walletData)
        {
            var deepLink = GetWalletAppDeeplink(walletData);
            osMediator.OpenUrl(deepLink);
        }

        /// <summary>
        /// Used to get a redirection deeplink for the <see cref="WalletModel"/>.
        /// </summary>
        /// <param name="walletData">Data of the wallet that we want to redirect to.</param>
        /// <returns>Deeplink for redirection.</returns>
        public string GetWalletAppDeeplink(WalletModel walletData)
        {
            return osMediator.IsMobilePlatform ? walletData.MobileLink : walletData.DesktopLink;
        }

        /// <summary>
        /// Used to get a connection deeplink for the particular wallet.
        /// </summary>
        /// <param name="walletData">Data of the wallet that we want to connect.</param>
        /// <param name="wcUri">The connection URI provided by Reown.</param>
        /// <returns>Deeplink for connection.</returns>
        /// <exception cref="ReownIntegrationException">Invalid format of deeplink registered for the provided wallet.</exception>
        public string BuildConnectionDeeplink(WalletModel walletData, string wcUri)
        {
            if (string.IsNullOrWhiteSpace(wcUri))
            {
                throw new ReownIntegrationException(
                    "Can not build a connection deep link. The connection URI is empty.");
            }

            var appLink = GetWalletAppDeeplink(walletData);

            if (string.IsNullOrWhiteSpace(appLink))
            {
                throw new ReownIntegrationException("[Linker] Native link cannot be empty.");
            }

            var safeAppUrl = appLink;
            if (!safeAppUrl.Contains("://"))
            {
                safeAppUrl = safeAppUrl.Replace("/", string.Empty).Replace(":", string.Empty);
                safeAppUrl = $"{safeAppUrl}://";
            }

            if (!safeAppUrl.EndsWith('/'))
            {
                safeAppUrl = $"{safeAppUrl}/";
            }

            var encodedWcUrl = Uri.EscapeDataString(wcUri);

            return $"{safeAppUrl}wc?uri={encodedWcUrl}";
        }
    }
}