using UnityEngine;

namespace ChainSafe.Gaming.WalletConnect
{
    public static class WalletConnectConfigExtensions
    {
        /// <summary>
        /// Sets <see cref="WalletConnectConfigSO.RememberSession"/> property of this config object.
        /// </summary>
        /// <param name="config">The config object.</param>
        /// <param name="rememberSession">New value for RememberSession property.</param>
        /// <returns>Updated <see cref="WalletConnectConfigSO"/> object.</returns>
        public static WalletConnectConfigSO WithRememberSession(this WalletConnectConfigSO config, bool rememberSession)
        {
            config.RememberSession = rememberSession;
            return config;
        }

        /// <summary>
        /// Sets <see cref="WalletConnectConfigSO.ForceNewSession"/> property of this config object.
        /// </summary>
        /// <param name="config">The config object.</param>
        /// <param name="forceNewSession">New value for ForceNewSession property.</param>
        /// <returns>Updated <see cref="WalletConnectConfigSO"/> object.</returns>
        public static WalletConnectConfigSO WithForceNewSession(this WalletConnectConfigSO config, bool forceNewSession)
        {
            config.ForceNewSession = forceNewSession;
            return config;
        }
    }
}