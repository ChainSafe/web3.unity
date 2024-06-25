using UnityEngine;

namespace ChainSafe.Gaming.WalletConnect
{
    public static class WalletConnectConfigExtensions
    {
        /// <summary>
        /// Sets <see cref="WalletConnectConfigAsset.RememberSession"/> property of this config object.
        /// </summary>
        /// <param name="config">The config object.</param>
        /// <param name="rememberSession">New value for RememberSession property.</param>
        /// <returns>Updated <see cref="WalletConnectConfigAsset"/> object.</returns>
        public static WalletConnectConfigAsset WithRememberSession(this WalletConnectConfigAsset config, bool rememberSession)
        {
            config.RememberSession = rememberSession;
            return config;
        }

        /// <summary>
        /// Sets <see cref="WalletConnectConfigAsset.ForceNewSession"/> property of this config object.
        /// </summary>
        /// <param name="config">The config object.</param>
        /// <param name="forceNewSession">New value for ForceNewSession property.</param>
        /// <returns>Updated <see cref="WalletConnectConfigAsset"/> object.</returns>
        public static WalletConnectConfigAsset WithForceNewSession(this WalletConnectConfigAsset config, bool forceNewSession)
        {
            config.ForceNewSession = forceNewSession;
            return config;
        }
    }
}